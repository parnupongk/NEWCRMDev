using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Routing;
using NEWCRM.Common;
using MvcPaging;
using System.IO;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class ContactController : ControllerBase
    {
        private const int defaultPageSize = 10; 

        #region Dev by Nantawit

        public ActionResult Index()
        {
            if (HttpContext.Session["searchcontent"] != null)
            {
                HttpContext.Session["searchcontent"] = null;
            }

            var rep_ctv = new ContactViewLogRepository();
            var model = new ContactViewModel();

            model.list_lastview = rep_ctv.getLastView(AppUtils.Session.User.usrID);
            

            return View(model);
        }

        public ActionResult LastContact()
        {
            var model = new ContactViewModel();
            var list = new ContactRepository().getLastActivityContact(AppUtils.Session.User.usrID);
            IList<sp_getContactSearch_Result> list_items = new List<sp_getContactSearch_Result>();

            foreach (var item in list)
            {
                sp_getContactSearch_Result item_target = new sp_getContactSearch_Result() { 
                    ctaID = item.ctaID,
                    ctaFullName = item.ctaFullName,
                    ctaEmail = item.ctaEmail,
                    ctaAvatar = item.ctaAvatar,
                    phnMain = item.phnMain,
                    phnOther = item.phnOther,
                    adrSummary = item.adrSummary                                
                };
                list_items.Add(item_target);            
            }

            ViewData["p_name"] = string.Empty;
            ViewData["p_cate"] = string.Empty;
            ViewData["p_order"] = string.Empty;
            list_items = list_items.ToPagedList(1, defaultPageSize);

            return PartialView("Search", list_items);
        }


        public ActionResult Search(ContactSearchCriteria _search)
        {
            HttpContext.Session["searchcontent"] = _search;

            ViewData["p_name"] = _search.p_name;
            ViewData["p_cate"] = _search.p_cate;
            ViewData["p_order"] = _search.p_order;
            int currentPageIndex = _search.page.HasValue ? _search.page.Value : 1;

            var rep = new ContactRepository();
            var model = new ContactViewModel();

            IList<sp_getContactSearch_Result> list_items = rep.getList(_search);
            list_items = list_items.ToPagedList(currentPageIndex, defaultPageSize);
            
            return PartialView(list_items);
        }

        public ActionResult Detail(int id)
        {
            var rep = new ContactRepository();
            var rep_ctv = new ContactViewLogRepository();
            var model = new ContactViewModel();

            model.contact = rep.GetById(id);
            //model.list_lastview = rep_ctv.getLastView(AppUtils.Session.User.usrID);

            #region Added ContactViewLog            
            rep_ctv.AddByEntity(new ContactViewLog() { 
                ctaID = id,
                usrID = AppUtils.Session.User.usrID,
                ctvCreatedOn = DateTime.Now
            });            
            #endregion

            return View(model);
        }

        public ActionResult getLastView()
        {
            var list = new ContactViewLogRepository().getLastView(AppUtils.Session.User.usrID);
            return PartialView(list);
        }

        public ActionResult DetailSearch(ContactSearchCriteria _search)
        {            
            var rep_ctv = new ContactViewLogRepository();
            var model = new ContactViewModel();
            
            model.search = _search;
            model.list_lastview = rep_ctv.getLastView(AppUtils.Session.User.usrID);
            HttpContext.Session["searchcontent"] = _search;
            
            return View("Index", model);
        }

        public ActionResult Back()
        {
            //var rep_cc = new ContactCategoryRepository();
            //var rep_cdv = new ContactDetailViewRepository();
            var rep_ctv = new ContactViewLogRepository();
            var model = new ContactViewModel();

            //model.list_contactcategories = rep_cc.getListWithTotal();
            //model.list_lastview_items = rep_cdv.GetList(Convert.ToInt32(AppUtils.Session.User.OBJID));
            model.list_lastview = rep_ctv.getLastView(AppUtils.Session.User.usrID);

            if (HttpContext.Session["searchcontent"] != null)
            {
                model.search = (ContactSearchCriteria)HttpContext.Session["searchcontent"];
            }

            return View("Index", model);
        }

        public ActionResult TimeLine(int id)
        {
            var rep = new ContactRepository();
            var model = new ContactViewModel();
            model.list_timeline = rep.getTimeline(id);
            model.sum_timeline_case = model.list_timeline.Where(t => t.LogType.Equals("Case")).Count();
            return PartialView(model);
        }

        public ActionResult ContactInfo()
        {           
            if (AppUtils.Session.Activity != null && AppUtils.Session.Activity.ctaID.HasValue)
            {
                var rep = new ContactRepository();
                var rep_cas = new CaseRepository();
                var model = new ContactViewModel();
                model.contact = rep.GetById(Convert.ToInt32(AppUtils.Session.Activity.ctaID));
                model.list_lastcases = rep_cas.GetLastCases(Convert.ToInt32(AppUtils.Session.Activity.ctaID));
                string view = "ContactInfo";
                return PartialView(view, model);
            }
            else
            {
                var model = new CaseViewModelLocalization();
                string view = "QuickAdd";
                return PartialView(view, model);
            }            
        }

        [HttpPost]
        public ActionResult CommonDetail(int? id)
        {
            var model = new ContactViewModel();
            var rep = new ContactRepository();
            var rep_phn = new PhoneRepository();
            int ctaId = 0;

            if (id.HasValue) {
                ctaId = Convert.ToInt32(id);
            } else if (AppUtils.Session.Activity.ctaID.HasValue)
            {
                ctaId = Convert.ToInt32(AppUtils.Session.Activity.ctaID);
            }
           
            model.ctaID = ctaId;
            model.list_phones = rep_phn.getByContact(ctaId);
            model.commondetail = rep.getContactCommonDetail(ctaId);
            if (!string.IsNullOrEmpty(model.commondetail.Address))
            {
                ViewBag.MapAddress = model.commondetail.Address.Replace(" ", "+");
            }
            return PartialView(model);
        }

        public ActionResult listPhones(int id)
        {
            var model = new ContactViewModel();
            var rep = new PhoneRepository();
            model.list_phones = rep.getByContact(id);            
            return PartialView(model);
        }

        public ActionResult listAddress(int id)
        {
            var model = new ContactViewModel();
            var rep = new AddressRepository();
            model.list_address = rep.getByContact(id);
            return PartialView(model);
        
        }

        public void QuickAdd()
        {
            var model = new ContactViewModel();
            if (Request != null)
            {
                #region Added Contact
                Contact contact = new Contact() {                    
                    ctyID = 0,
                    ctaFullName = string.Format("{0} {1}", Request["firstname"].Trim(), Request["lastname"].Trim()),
                    ctaFirstName = Request["firstname"].Trim(),
                    ctaLastName = Request["lastname"].Trim(),
                    ctaDoNotEmail = true,
                    ctaDoNotPhone = true,
                    ctaDoNotFax = true,
                    ctaDoNotPostalMail = true,
                    ctaCreatedOn = DateTime.Now,
                    ctaCreatedBy = AppUtils.Session.User.usrID,
                    ctaModifiedOn = DateTime.Now,
                    ctaModifiedBy = AppUtils.Session.User.usrID                    
                };

                var rep = new ContactRepository();
                rep.AddByEntity(contact);
                // Set New ctaID to Activity Session
                if (AppUtils.Session.Activity != null)
                {
                    AppUtils.Session.Activity.ctaID = contact.ctaID;
                }
                #endregion

                #region Added Contact Log
                this.AddContactLogs(new ContactLog()
                {
                    ctaID = contact.ctaID,
                    cltID = AppUtils.CaseLogType.CreatedContact,
                    ctlNote = contact.ctaFullName,
                    ctlCreatedOn = DateTime.Now,
                    ctlCreatedBy = AppUtils.Session.User.usrID,
                    ctlModifiedOn = DateTime.Now,
                    ctlModifiedBy = AppUtils.Session.User.usrID
                });

                #endregion

                #region Added Phones
                var rep_phn = new PhoneRepository();
                // Created Main Phone
                var phn = new Phone() {
                    ctaID = contact.ctaID,
                    phtID = 1,
                    phnNumber = AppUtils.Session.Activity.actChannelData,
                    phnIsMain = true,
                    phnActive = true,
                    phnCreatedOn = DateTime.Now,
                    phnCreatedBy = AppUtils.Session.User.usrID,
                    phnModifiedOn = DateTime.Now,
                    phnModifiedBy = AppUtils.Session.User.usrID                  
                };
                rep_phn.AddByEntity(phn);               
                #endregion              

                model.contact = contact;
            }

            return;
            //return AppUtils.Session.Activity.ctaID.ToString();
            //return RedirectToAction("NewCase", "Case", new { id = AppUtils.Session.Activity.ctaID });
            //return PartialView("ContactInfo", model);
        }        
        
        public string AutoComplete()
        {
            var rep = new ContactRepository();
            var search = Request["search"];

            var list = rep.getContactAutoComplete(search, 5);            
            JavaScriptSerializer json = new JavaScriptSerializer();
            var str = json.Serialize(list);
            return str;         
        }

        [HttpPost]
        public void CommonDetailUpdate(ContactCommonData data)
        {            
            var rep = new RepositoryBase();                
            var db = rep.GetDBContext();

            if (data.datatable.ToUpper().Equals("PHONES"))
            {                
                if(data.dataid.Equals("new"))
                {
                    if (string.IsNullOrEmpty(data.datavalue)) 
                    {
                        return;
                    }

                    var rep_phn = new PhoneRepository();
                    Phone phn = new Phone()
                    {
                        ctaID = data.ctaID,
                        phtID = 0,
                        phnNumber = data.datavalue,
                        phnIsMain = false,
                        phnActive = true,
                        phnCreatedOn = DateTime.Now,
                        phnCreatedBy = AppUtils.Session.User.usrID,
                        phnModifiedOn = DateTime.Now,
                        phnModifiedBy = AppUtils.Session.User.usrID
                    };
                    rep_phn.AddByEntity(phn);

                    #region Added Contact Log
                    this.AddContactLogs(new ContactLog()
                    {
                        ctaID = data.ctaID,
                        cltID = AppUtils.CaseLogType.UpdatedContact,
                        ctlNote = string.Format("Add new phone [{0}]", data.datavalue),
                        ctlCreatedOn = DateTime.Now,
                        ctlCreatedBy = AppUtils.Session.User.usrID,
                        ctlModifiedOn = DateTime.Now,
                        ctlModifiedBy = AppUtils.Session.User.usrID
                    });
                    #endregion

                } else {
                    int id = Convert.ToInt32(data.dataid);
                    Phone phn = db.Phones.Single(p => p.phnID == id);
                    var ctlNote = string.Empty;
                    if (!string.IsNullOrEmpty(data.datavalue))
                    {
                        ctlNote = string.Format("Updated phone [{0}] -> [{1}]", phn.phnNumber, data.datavalue);
                        phn.phnNumber = data.datavalue;                        
                    }
                    else {
                        ctlNote = string.Format("Updated phone [{0}] Inactive", phn.phnNumber);
                        phn.phnActive = false; 
                    }

                    db.SaveChanges();

                    #region Added Contact Log
                    this.AddContactLogs(new ContactLog()
                    {
                        ctaID = data.ctaID,
                        cltID = AppUtils.CaseLogType.UpdatedContact,
                        ctlNote = ctlNote,
                        ctlCreatedOn = DateTime.Now,
                        ctlCreatedBy = AppUtils.Session.User.usrID,
                        ctlModifiedOn = DateTime.Now,
                        ctlModifiedBy = AppUtils.Session.User.usrID
                    });
                    #endregion
                }               
            }
            else if(data.datatable.ToUpper().Equals("ADDRESS"))
            {
                if (data.dataid.Equals("new"))
                {
                    if (string.IsNullOrEmpty(data.datavalue))
                    {
                        return;
                    }
                    var rep_adr = new AddressRepository();
                    Address adr = new Address()
                    {
                        ctaID = data.ctaID,
                        adtID = 0,
                        adrSummary = data.datavalue,
                        adrCreatedOn = DateTime.Now,
                        adrCreatedBy = AppUtils.Session.User.usrID,
                        adrModifiedOn = DateTime.Now,
                        adrModifiedBy = AppUtils.Session.User.usrID,
                        adrActive = true
                    };
                    rep_adr.AddByEntity(adr);

                    #region Added Contact Log
                    this.AddContactLogs(new ContactLog()
                    {
                        ctaID = data.ctaID,
                        cltID = AppUtils.CaseLogType.UpdatedContact,
                        ctlNote = string.Format("Add new address [{0}]", data.datavalue),
                        ctlCreatedOn = DateTime.Now,
                        ctlCreatedBy = AppUtils.Session.User.usrID,
                        ctlModifiedOn = DateTime.Now,
                        ctlModifiedBy = AppUtils.Session.User.usrID
                    });
                    #endregion
                }
                else 
                {
                    int id = Convert.ToInt32(data.dataid);
                    Address adr = db.Address.Single(a => a.adrID == id);
                    string ctlNote = string.Empty;
                    if (!string.IsNullOrEmpty(data.datavalue))
                    {
                        ctlNote = string.Format("Updated address [{0}] -> [{1}]", adr.adrSummary, data.datavalue);
                        adr.adrSummary = data.datavalue;
                    }
                    else {
                        ctlNote = string.Format("Updated address [{0}] Inactive", adr.adrSummary);
                        adr.adrActive = false;
                    }         

                    db.SaveChanges();

                    #region Added Contact Log
                    this.AddContactLogs(new ContactLog()
                    {
                        ctaID = data.ctaID,
                        cltID = AppUtils.CaseLogType.UpdatedContact,
                        ctlNote = ctlNote,
                        ctlCreatedOn = DateTime.Now,
                        ctlCreatedBy = AppUtils.Session.User.usrID,
                        ctlModifiedOn = DateTime.Now,
                        ctlModifiedBy = AppUtils.Session.User.usrID
                    });
                    #endregion
                }            
            }
            else if (data.datatable.ToUpper().Equals("CONTACTS"))
            {
                int id = data.ctaID;
                Contact cta = db.Contacts.Single(c => c.ctaID == id);
                string ctlNote = string.Empty;
                if(data.datafield.Equals("ctaEmail")) 
                {
                    ctlNote = string.Format("Updated email [{0}] -> [{1}]", cta.ctaEmail, data.datavalue);
                    cta.ctaEmail = data.datavalue;
                }
                if (data.datafield.Equals("ctaDOB"))
                {
                    ctlNote = string.Format("Updated birthday [{0}] -> [{1}]", cta.ctaDOB, data.datavalue);
                    cta.ctaDOB = Convert.ToDateTime(data.datavalue);
                }
                if (data.datafield.Equals("gndID")) {
                    string oldgnd = "Unknow";
                    string newgnd = "Unknow";

                    if (cta.gndID.HasValue)
                    {
                        if (cta.gndID == 1)
                        {
                            oldgnd = "Male";
                        }
                        else if (cta.gndID == 2)
                        {
                            oldgnd = "Female";
                        }
                    }

                    if (data.datavalue.Equals("1")) {
                        newgnd = "Male";
                    }
                    else if (data.datavalue.Equals("2")) {
                        newgnd = "Female";
                    }

                    ctlNote = string.Format("Updated gender [{0}] -> [{1}]", oldgnd, newgnd);
                    cta.gndID = Convert.ToInt32(data.datavalue);
                }

                db.SaveChanges();

                #region Added Contact Log
                this.AddContactLogs(new ContactLog()
                {
                    ctaID = data.ctaID,
                    cltID = AppUtils.CaseLogType.UpdatedContact,
                    ctlNote = ctlNote,
                    ctlCreatedOn = DateTime.Now,
                    ctlCreatedBy = AppUtils.Session.User.usrID,
                    ctlModifiedOn = DateTime.Now,
                    ctlModifiedBy = AppUtils.Session.User.usrID
                });
                #endregion
            }
        }

        [HttpPost]
        public void updateContactName()
        {
            int ctaID = Convert.ToInt32(Request["ctaID"]);
            Contact c = new ContactRepository().GetById(ctaID);
            string firstname = Request["firstname"].Trim();
            string lastname = Request["lastname"].Trim();
            new ContactRepository().updateName(ctaID, firstname, lastname);            

            #region Added Contact Log            
            string ctlNote = string.Format("Updated name [{0}] -> [{1}]", c.ctaFullName, firstname + " " + lastname);
            this.AddContactLogs(new ContactLog()
            {
                ctaID = ctaID,
                cltID = AppUtils.CaseLogType.UpdatedContact,
                ctlNote = ctlNote,
                ctlCreatedOn = DateTime.Now,
                ctlCreatedBy = AppUtils.Session.User.usrID,
                ctlModifiedOn = DateTime.Now,
                ctlModifiedBy = AppUtils.Session.User.usrID
            });
            #endregion
        }

        //[HttpPost]
        //public ActionResult CommonDetailAddressUpdated(Address data)
        //{
        //    var rep = new RepositoryBase();
        //    var db = rep.GetDBContext();
        //    Address adr = db.Address.Single(a => a.adrID == data.adrID);
        //    adr.adrSummary = data.adrSummary;
        //    adr.pvnID = data.pvnID;
        //    adr.dstID = data.dstID;
        //    adr.sdtID = data.sdtID;
        //    db.SaveChanges();

        //    return Content(data.adrSummary);
        //}

        public void AddContactLogs(ContactLog data)
        {
            var rep = new ContactLogsRepository();
            rep.AddByEntity(data);        
        }

        public ActionResult AddNew(ContactAddNewModel frm)
        {
            try
            {
                if (frm.isSave)
                {
                    var rep_cta = new ContactRepository();
                    var rep_ctl = new ContactLogsRepository();
                    var rep_phn = new PhoneRepository();
                    var rep_adr = new AddressRepository();
                 
                    #region Added Contact
                    Contact cta = new Contact()
                    {
                        ctyID = 0,
                        ctaFullName = string.Format("{0} {1}", frm.ctaFirstName, frm.ctaLastName),
                        ctaFirstName = frm.ctaFirstName,
                        ctaLastName = frm.ctaLastName,
                        ctaDoNotEmail = true,
                        ctaDoNotPhone = true,
                        ctaDoNotFax = true,
                        ctaDoNotPostalMail = true,
                        ctaDOB = (!string.IsNullOrEmpty(frm.ctaDOB)) ? Convert.ToDateTime(frm.ctaDOB) : (DateTime?)null,
                        gndID = frm.gndID,
                        ctaEmail = frm.ctaEmail,
                        ctaCreatedOn = DateTime.Now,
                        ctaCreatedBy = AppUtils.Session.User.usrID,
                        ctaModifiedOn = DateTime.Now,
                        ctaModifiedBy = AppUtils.Session.User.usrID
                    };
                    if (rep_cta.AddByEntity(cta) < 1)
                    {
                        throw new Exception("Can't add new contact");
                    }
                    #endregion

                    #region Added Contact Log
                    rep_ctl.AddByEntity(new ContactLog()
                    {
                        ctaID = cta.ctaID,
                        cltID = AppUtils.CaseLogType.CreatedContact,
                        ctlNote = string.Empty,
                        ctlCreatedOn = DateTime.Now,
                        ctlCreatedBy = AppUtils.Session.User.usrID,
                        ctlModifiedOn = DateTime.Now,
                        ctlModifiedBy = AppUtils.Session.User.usrID
                    });
                    #endregion

                    #region Added Main Phone
                    Phone phnM = new Phone() { 
                            ctaID = cta.ctaID,
                            phtID = 0,
                            phnNumber = frm.phnMNumber,
                            phnIsMain = true,
                            phnActive = true,
                            phnCreatedOn = DateTime.Now,
                            phnCreatedBy = AppUtils.Session.User.usrID,
                            phnModifiedOn = DateTime.Now,
                            phnModifiedBy = AppUtils.Session.User.usrID
                        };
                        if (rep_phn.AddByEntity(phnM) < 1)
                        {
                            throw new Exception("Can't add new main phone");
                        }
                        #endregion

                    #region Added Other Phone
                        if (!string.IsNullOrEmpty(frm.phnNumber))
                        {
                            Phone phn = new Phone()
                            {
                                ctaID = cta.ctaID,
                                phtID = 0,
                                phnNumber = frm.phnNumber,
                                phnIsMain = false,
                                phnActive = true,
                                phnCreatedOn = DateTime.Now,
                                phnCreatedBy = AppUtils.Session.User.usrID,
                                phnModifiedOn = DateTime.Now,
                                phnModifiedBy = AppUtils.Session.User.usrID
                            };
                            if (rep_phn.AddByEntity(phn) < 1)
                            {
                                throw new Exception("Can't add new other phone");
                            }
                        }
                        #endregion

                    #region Added Address

                        string adrSummary = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", frm.adrHomeNo, frm.adrMoo, frm.adrBuilding, frm.adrRoom, frm.adrMooban, frm.adrSoi, frm.adrRoad, frm.adrSubDistrict, frm.adrDistrict, frm.adrProvince, frm.adrZip);

                        Address adr = new Address() { 
                            ctaID = cta.ctaID,
                            adtID = 0,
                            adrHomeNo = frm.adrHomeNo,
                            adrMoo = frm.adrMoo,
                            adrBuilding = frm.adrBuilding,
                            adrRoom = frm.adrRoom,
                            adrMooban = frm.adrMooban,
                            adrSoi = frm.adrSoi,
                            adrRoad = frm.adrRoad,
                            adrSubDistrict = frm.adrSubDistrict,
                            adrDistrict = frm.adrDistrict,
                            adrProvince = frm.adrProvince,
                            adrZip = frm.adrZip,
                            adrSummary = adrSummary,
                            adrActive = true,
                            adrCreatedOn = DateTime.Now,
                            adrCreatedBy = AppUtils.Session.User.usrID,
                            adrModifiedOn = DateTime.Now,
                            adrModifiedBy = AppUtils.Session.User.usrID
                        };

                        if (rep_adr.AddByEntity(adr) < 1)
                        {
                            throw new Exception("Can't add new address");
                        }
                        #endregion                  

                    return Content(Convert.ToString(cta.ctaID));
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }

            return View();
        }

        public ActionResult ChannelSummaryList()
        {
            var rep = new ContactRepository();
            return PartialView("ChannelSummaryList", rep.getActivityChannel());
        }

        public void uploadAvatar()
        {
            var ctaID = Request["ctaID"];
            foreach (string s in Request.Files)
            {
                try
                {
                    var file = Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string savefilename = string.Format("{0}_{1}", ctaID, fileName);
                        string savefilepathe = Server.MapPath("~/Images/avatars/") + savefilename;

                        // Updated contact
                        new ContactRepository().updateAvatar(Convert.ToInt32(ctaID), savefilename);

                        // Upload File
                        file.SaveAs(savefilepathe);

                        var newFile = new FileInfo(savefilepathe);
                        //While File is not accesable because of writing process
                        while (IsFileLocked(newFile)) { }
                           
                        Response.Write(savefilename);                                               
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }                
            }            
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }

        public ActionResult showMap(int id)
        {
            var model = new ContactViewModel();
            var rep = new ContactRepository();
            model.commondetail = rep.getContactCommonDetail(id);
            if (!string.IsNullOrEmpty(model.commondetail.Address))
            {
                ViewBag.MapAddress = model.commondetail.Address.Replace(" ", "+");
            }
            return PartialView();
        }

        #endregion
    }
}
