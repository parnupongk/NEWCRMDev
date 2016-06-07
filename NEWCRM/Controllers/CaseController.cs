using NEWCRM.Common;
using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.IO;
using System.Net.Mime;
using System.Configuration;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class CaseController : ControllerBase
    {
        #region bom
        public ActionResult getCaseByCasID(int id)
        {
            var cases = new CaseRepository().GetByID(id);

            return PartialView("CaseParent", cases);

        }

        [HttpPost]
        public JsonResult CaseURLAccAuto(string Prefix,string casIDLevel3)
        {
            try {
                var list = new CaseRepository().GetGroupURLAccount();
                var accName = (from N in list
                               where N.casLevel6.Contains(Prefix) && (N.casIDLevel2 == int.Parse(casIDLevel3))
                               select new { N.casLevel6, N.casID });
                return Json(accName, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Aeh
        public ActionResult NewCase(int? id)
        {
            var model = new CaseViewModelLocalization();
            var rep_priority = new PriorityRepository();
            var rep_chn = new ChannelsRepository();

            try
            {
                if (AppUtils.Session.Activity == null)
                {
                    throw new Exception("Activity data is empty");
                }

                if (id.HasValue)
                {
                    AppUtils.Session.Activity.ctaID = Convert.ToInt32(id);
                }
                //else 
                //{
                //    AppUtils.Session.Activity.ctaID = null;
                //}

                model.list_priority = rep_priority.getAll();
                model.list_channels = rep_chn.getAll();
                model.list_status = new CaseRepository().GetStatus(string.Empty);
                model.commerceType = ConfigurationManager.AppSettings["CASE_Commerce_Type"].ToString().Split('|').ToList();
                model.productCategory = ConfigurationManager.AppSettings["CASE_Product_Category"].ToString().Split('|').ToList();
                model.serviceCategory = ConfigurationManager.AppSettings["CASE_Service_Category"].ToString().Split('|').ToList();
                model.deliveryType = ConfigurationManager.AppSettings["CASE_Delivery_Type"].ToString().Split('|').ToList();
                model.paymentType = ConfigurationManager.AppSettings["CASE_Payment_Type"].ToString().Split('|').ToList();
                model.valueRange = ConfigurationManager.AppSettings["CASE_Value_Range"].ToString().Split('|').ToList();
                model.conversationChannel = ConfigurationManager.AppSettings["CASE_Conversation_Channel"].ToString().Split('|').ToList();
                model.list_status_reason = ConfigurationManager.AppSettings["CASE_Status_Reason"].ToString().Split('|').ToList();
            }
            catch (Exception err)
            {
                return goExceptionPage(err.Message, err.StackTrace);
            }

            return View("~/Views/Case/NewCase.cshtml", model);
        }

        public ActionResult NewCaseFromContact(int id)
        {
            var act = new Activities()
            {
                chnID = AppUtils.Channels.Inbound,
                atsID = 0,
                actChannelData = string.Empty,
                actCreatedOn = DateTime.Now,
                actCreatedBy = AppUtils.Session.User.usrID,
                actModifiedOn = DateTime.Now,
                actModifiedBy = AppUtils.Session.User.usrID,
                actStart = DateTime.Now,
                ctaID = id
            };

            act = new ActivitiesRepository().AddByEntity(act);
            AppUtils.Session.Activity = act;
            return NewCase(id);
        }

        [HttpPost]
        public void ChangedChannel(string chnID)
        {
            if (AppUtils.Session.Activity != null)
            {
                AppUtils.Session.Activity.chnID = chnID;
            }
        }

        public ContentResult NewCaseSave(NewCaseModel model)
        {
            if (model != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(model.casNote))
                    {
                        throw new Exception("Case Note is empty");
                    }

                    var act_data = AppUtils.Session.Activity;

                    if (!act_data.ctaID.HasValue)
                    {
                        throw new Exception("Please created new contact");
                    }

                    #region Add Case
                    var casGroup = NEWCRM.Common.AppUtils.Session.User.grpID;
                    var _CreatedDate = DateTime.Now;
                    var _CreatedBy = AppUtils.Session.User.usrID;
                    model.casSummary = string.Empty;
                    if (model.casIDLevel1.HasValue)
                    {
                        model.casSummary = string.Format("{0}", model.casLevel1);
                        model.casIDSummary = model.casIDLevel1;
                    }
                    if (model.casIDLevel2.HasValue)
                    {
                        model.casSummary += string.Format(" > {0}", model.casLevel2);
                        model.casIDSummary = model.casIDLevel2;
                    }
                    if (model.casIDLevel3.HasValue)
                    {
                        //model.casSummary += string.Format(" > {0}", model.casLevel3);
                        model.casIDSummary = model.casIDLevel3;
                    }
                    if (model.casIDLevel4.HasValue)
                    {
                        model.casSummary += string.Format(" > {0}", model.casLevel4);
                        model.casIDSummary = model.casIDLevel4;
                    }
                    if (model.casIDLevel5.HasValue)
                    {
                        model.casSummary += string.Format(" > {0}", model.casLevel5);
                        model.casIDSummary = model.casIDLevel5;
                    }
                    //if (!string.IsNullOrEmpty(model.casURLAccount))
                    //{
                        //model.casSummary += string.Format(" > {0}", model.casURLAccount);
                    //}
                    if (model.casSLA.HasValue && model.casSLA > 0)
                    {
                        model.casDueDate = _CreatedDate.AddMinutes((double)model.casSLA);
                    }
                    if (model.cssID != 1) casGroup = 0;
                    Case data = new Case()
                    {
                        actID = act_data.actID,
                        ctaID = Convert.ToInt32(act_data.ctaID),
                        casParentID = model.casParentID,
                        casIDLevel1 = model.casIDLevel1,
                        casIDLevel2 = model.casIDLevel2,
                        casIDLevel3 = model.casIDLevel3,
                        casIDLevel4 = model.casIDLevel4,
                        casIDLevel5 = model.casIDLevel5,
                        casLevel1 = model.casLevel1,
                        casLevel2 = model.casLevel2,
                        casLevel3 = model.casLevel3,
                        casLevel4 = model.casLevel4,
                        casLevel5 = model.casLevel5,
                        casLevel6 = model.casURLAccount,
                        casIDSummary = Convert.ToInt32(model.casIDSummary),
                        casSummary = model.casSummary,
                        casNote = model.casNote,
                        casOwnerID = _CreatedBy,
                        casCreatedOn = _CreatedDate,
                        casCreatedBy = _CreatedBy,
                        casModifiedOn = _CreatedDate,
                        casModifiedBy = _CreatedBy,
                        priID = Convert.ToInt32(model.priID),
                        cssID = Convert.ToInt32(model.cssID),
                        cbtID = model.cbtID,
                        cbcID = model.cbcID,
                        CallbackInfo = model.CallbackInfo,
                        casSLA = model.casSLA,
                        casFav = model.casFav,
                        casDueDate = model.casDueDate,
                        casGroupID= casGroup,
                        cascommerceType = model.commerceType,
                        casproductCategory = model.productCategory,
                        casserviceCategory = model.serviceCategory,
                        casdeliveryType = model.deliveryType,
                        casvalueRange = model.valueRange,
                        casconversationChannel = model.conversationChannel,
                        casreferenceDetail = model.txtRefDetail,
                        casdetail = model.txtDetail,
                        casstatusReason = model.cssStatusReason,
                        //casGroupID = AppUtils.Session.User.grpID
                    };

                    if (new CaseRepository().AddByEntity(data) < 1)
                    {
                        throw new Exception("Save [Case] incompleted");
                    }
                    else
                    {
                        string casIDName = string.Format("C-{0}", data.casID.ToString().PadLeft(8, '0'));
                        new CaseRepository().UpdatedCaseIDName(casIDName, data.casID);

                    }

                    #endregion

                    #region Added CaseLogs
                    CaseLog csl = new CaseLog()
                    {
                        actID = 0,
                        casID = data.casID,
                        cltID = 1,
                        cslNote = data.casSummary,
                        cslCreatedOn = _CreatedDate,
                        cslCreatedBy = _CreatedBy,
                        cslModifiedOn = _CreatedDate,
                        cslModifiedBy = _CreatedBy
                    };
                    new CaseLogsRepository().AddByEntity(csl);
                    #endregion

                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    Response.TrySkipIisCustomErrors = true; 
                    return Content(ex.Message, "text/html");
                }
            }

            return Content(string.Empty);
        }

        public ActionResult NewCaseList()
        {
            var list = new CaseRepository().getNewCaseList(AppUtils.Session.Activity.actID);
            return PartialView("NewCaseList", list);
        }

        public ActionResult NewCaseRemoveById()
        {
            if (Request != null)
            {
                int casID = Convert.ToInt32(Request["id"]);
                new CaseRepository().RemoveByID(casID);
            }
            return NewCaseList();
        }

        public ActionResult NewCaseFavorite()
        { 
            if(Request != null)
            {
                int casID = Convert.ToInt32(Request["id"]);
                var cas = new CaseRepository().GetByID(casID);
                bool casFav = !cas.casFav;
                updateFavourite(casID, casFav);

                //if (AppUtils.Session.NewCase != null)
                //{
                //    List<NewCaseModel> newcase_list = AppUtils.Session.NewCase;
                //    var item = newcase_list.SingleOrDefault(r => r.id == _id);
                //    if (item.casFav)
                //    {
                //        item.casFav = false;
                //    }
                //    else {
                //        item.casFav = true;
                //    }                    
                //    AppUtils.Session.NewCase = newcase_list;                
                //}
            }
            return NewCaseList();
        }

        //public ContentResult NewCaseEndCall()
        //{
        //    string ctaID = string.Empty;

        //    if (Request != null)
        //    {
        //        try
        //        {
        //            var casEmoticon = Request["casEmoticon"];
        //            var newcase_list = AppUtils.Session.NewCase;
        //            var rep_cas = new CaseRepository();
        //            var rep_act = new ActivitiesRepository();                    

        //            #region Updated Activity
        //            var act_data = AppUtils.Session.Activity;
        //            if (act_data != null)
        //            {

        //                //act_data.actEmo = Convert.ToInt32(casEmoticon);
        //                //act_data.actStop = DateTime.Now;
        //                //act_data.atsID = 1; // Status completed                        
        //                if (rep_act.EndCallUpdated(act_data.actID, Convert.ToInt32(casEmoticon), Convert.ToInt32(act_data.ctaID), AppUtils.Session.User.usrID, act_data.chnID) == 0)
        //                {
        //                    throw new Exception("Save [Activities] incompleted");
        //                }                       
        //            }
        //            else 
        //            {
        //                throw new Exception("[Activities] data incorrect or empty");
        //            }
        //            #endregion
                   
        //            foreach (NewCaseModel item in newcase_list)
        //            {
        //                #region Added Cases
        //                Cases data = new Cases()
        //                {
        //                    actID = act_data.actID,
        //                    ctaID = Convert.ToInt32(act_data.ctaID),                            
        //                    casIDLevel1 = item.casIDLevel1,
        //                    casIDLevel2 = item.casIDLevel2,
        //                    casIDLevel3 = item.casIDLevel3,
        //                    casIDLevel4 = item.casIDLevel4,
        //                    casLevel1 = item.casLevel1,
        //                    casLevel2 = item.casLevel2,
        //                    casLevel3 = item.casLevel3,
        //                    casLevel4 = item.casLevel4,
        //                    casIDSummary = Convert.ToInt32(item.casIDSummary),
        //                    casSummary = item.casSummary,
        //                    casNote = item.casNote,
        //                    casOwnerID = item.casOwnerID,
        //                    casCreatedOn = Convert.ToDateTime(item.casCreatedOn),
        //                    casCreatedBy = Convert.ToInt32(item.casCreatedBy),
        //                    casModifiedOn = Convert.ToDateTime(item.casCreatedOn),
        //                    casModifiedBy = Convert.ToInt32(item.casCreatedBy),
        //                    priID = Convert.ToInt32(item.priID),
        //                    cssID = Convert.ToInt32(item.cssID),
        //                    cbtID = item.cbtID,
        //                    cbcID = item.cbcID,
        //                    CallbackInfo = item.CallbackInfo,
        //                    casSLA = item.casSLA,
        //                    casFav = item.casFav,
        //                    casDueDate = item.casDueDate                            
        //                };

        //                if (rep_cas.AddByEntity(data) < 1)
        //                {
        //                    throw new Exception("Save [Case] incompleted");
        //                }
        //                else 
        //                {
        //                    string casIDName = string.Format("C-{0}", data.casID.ToString().PadLeft(8,'0'));
        //                    rep_cas.UpdatedCaseIDName(casIDName, data.casID);                                                    
        //                }
        //                #endregion

        //                #region Added CaseLogs
        //                var rep_csl = new CaseLogsRepository();
        //                CaseLogs csl = new CaseLogs() { 
        //                    actID = 0,
        //                    casID = data.casID,
        //                    cltID = 1,
        //                    cslNote = data.casSummary,
        //                    cslCreatedOn = DateTime.Now,
        //                    cslCreatedBy = AppUtils.Session.User.usrID,
        //                    cslModifiedOn = DateTime.Now,
        //                    cslModifiedBy = AppUtils.Session.User.usrID                        
        //                };
        //                rep_csl.AddByEntity(csl);
        //                #endregion
                       
        //            }
                    
        //            #region Updated Contact Emoticon
        //            if (AppUtils.Session.Activity.ctaID.HasValue)
        //            {
        //                var rep_cta = new ContactRepository();
        //                rep_cta.updateEmoticon(Convert.ToInt32(casEmoticon), Convert.ToInt32(AppUtils.Session.Activity.ctaID));
        //                //if (rep_cta.updateEmoticon(Convert.ToInt32(casEmoticon), Convert.ToInt32(AppUtils.Session.Activity.ctaID)) == 0)
        //                //{
        //                //    throw new Exception("Cannot update contact emoticon");
        //                //}                        
        //            }
        //            #endregion

        //            ctaID = Convert.ToString(AppUtils.Session.Activity.ctaID);

        //            AppUtils.Session.Activity = null;
        //            AppUtils.Session.NewCase = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.StatusCode = 500;
        //            Response.TrySkipIisCustomErrors = true; 
        //            return Content(ex.Message, "text/html");
        //        }
        //    }

        //    return Content(ctaID);
        //}

        public ContentResult NewCaseEndCall()
        {
            string ctaID = string.Empty;

            if (Request != null)
            {
                try
                {
                    var casEmoticon = Request["casEmoticon"];                    
                    var rep_act = new ActivitiesRepository();

                    #region Updated Activity
                    var act_data = AppUtils.Session.Activity;
                    if (act_data != null)
                    {                                          
                        if (rep_act.EndCallUpdated(act_data.actID, Convert.ToInt32(casEmoticon), Convert.ToInt32(act_data.ctaID), AppUtils.Session.User.usrID, act_data.chnID) == 0)
                        {
                            throw new Exception("Save [Activities] incompleted");
                        }
                    }
                    else
                    {
                        throw new Exception("[Activities] data incorrect or empty");
                    }
                    #endregion                  

                    #region Updated Contact Emoticon
                    if (AppUtils.Session.Activity.ctaID.HasValue)
                    {
                        var rep_cta = new ContactRepository();
                        rep_cta.updateEmoticon(Convert.ToInt32(casEmoticon), Convert.ToInt32(AppUtils.Session.Activity.ctaID));                                    
                    }
                    #endregion

                    ctaID = Convert.ToString(AppUtils.Session.Activity.ctaID);

                    AppUtils.Session.Activity = null;
                    AppUtils.Session.NewCase = null;
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    Response.TrySkipIisCustomErrors = true;
                    return Content(ex.Message, "text/html");
                }
            }

            return Content(ctaID);
        }

        public ContentResult SearchByNo()
        {
            string caseno = Request["headerSearch"];
            string casid = string.Empty;
            if (!string.IsNullOrEmpty(caseno)) 
            {
                var rep = new CaseRepository();
                Case cas = rep.GetByCaseNo(caseno.Trim());
                if (cas != null)
                {
                    casid = Convert.ToString(cas.casID);
                }
            }

            return Content(casid);
        }

        public ActionResult CaseInfo(int id)
        {
            CaseModels d = new CaseModels();
            d.CaseDetails = new CaseRepository().GetCaseDetailById(id, "");
            var cas = new CaseRepository().GetByID(id);
            var role = AppUtils.Session.User.grpID;

            // Case Function
            ViewBag.Favourite = false;
            ViewBag.Edit = false;
            ViewBag.AddNote = false;
            ViewBag.Dispatch = false;
            ViewBag.Assign = false;
            ViewBag.Closed = false;
            ViewBag.ChangeStatus = false;
            ViewBag.Email = false;
            ViewBag.Accept = false;
            ViewBag.Reject = false;

            if (role == 1 || role == 2)
            {
                ViewBag.Favourite = true;
                ViewBag.Edit = true;
                ViewBag.AddNote = true;
                ViewBag.Dispatch = true;
                ViewBag.Assign = true;
                ViewBag.Closed = true;
                ViewBag.ChangeStatus = true;
                ViewBag.Email = true;

                #region Admin or Supervisor 
                if (!cas.assignTo.HasValue && !cas.casGroupID.HasValue)
                {
                    
                } else {
                    if (cas.casGroupID.HasValue)
                    {
                        if (cas.casGroupID == AppUtils.Session.User.grpID)
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Accept = true;
                        }
                        else
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Reject = true;
                        }
                    }
                    if (cas.assignTo.HasValue)
                    {
                        if (cas.assignTo == AppUtils.Session.User.usrID)
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Accept = true;
                        }
                        else
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Reject = true;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Agent
                if (!cas.assignTo.HasValue && !cas.casGroupID.HasValue)
                {
                    // If Owner
                    if (cas.casOwnerID == AppUtils.Session.User.usrID)
                    {
                        ViewBag.Favourite = true;
                        ViewBag.Edit = true;
                        ViewBag.AddNote = true;
                        ViewBag.Dispatch = true;
                        ViewBag.Assign = true;
                        ViewBag.Closed = true;
                        ViewBag.ChangeStatus = true;
                        ViewBag.Email = true;
                    }
                    else
                    {
                        ViewBag.Favourite = true;
                        ViewBag.AddNote = true;
                    }
                }
                else
                {
                    if (cas.casGroupID.HasValue)
                    {
                        if (cas.casGroupID == AppUtils.Session.User.grpID)
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Accept = true;
                        }
                        else
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                        }
                    }
                    if (cas.assignTo.HasValue)
                    {
                        if (cas.assignTo == AppUtils.Session.User.usrID)
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                            ViewBag.Accept = true;
                        }
                        else
                        {
                            ViewBag.Favourite = true;
                            ViewBag.AddNote = true;
                        }
                    }
                }
                #endregion
            }

            if ( cas.cssID == 3 || cas.cssID == 4) // close or cancel
            {
                ViewBag.Edit = false;
                ViewBag.Closed = false;
                ViewBag.Dispatch = false;
                ViewBag.Assign = false;
                if (role == 1 || role == 2) // Sup allow to change status all case
                {
                    ViewBag.ChangeStatus = true;
                }
            }

            if (cas.casParentID != null) ViewBag.Email = false;
                    
            return PartialView(d);
        }

        public ActionResult DetailTimeline(int id)
        {
            var rep = new CaseRepository();
            var model = new CaseViewModelLocalization();

            model.list_timeline = rep.GetDetailTimeline(id);
            return PartialView(model);
        }
        
        public ActionResult setFavourite(bool? bFav)
        {
            if (bFav.HasValue)
            {
                ViewData["Flag"] = bFav;
            }
            else
            {
                int id = Convert.ToInt32(Request["id"]);
                bool isFav = Convert.ToBoolean(Request["value"]);
                updateFavourite(id, isFav);
                ViewData["Flag"] = isFav;
            }
            return PartialView();
        }

        public void updateFavourite(int id, bool isFav)
        {
            new CaseRepository().updateFav(id, isFav);
            return;
        }
        
        public string getCaseFormProvince()
        {
            return AppUtils.Province.getAll4SelectOption();
        }        

        public ActionResult CaseDetailEdit(int id)
        {
            var c = new CaseRepository().GetByID(id);
            return View(c);
        }

        public JsonResult CaseDetailEditSave(NewCaseModel model)
        {
            string strSuccess = string.Empty;
            string strError = string.Empty;
            try
            {
                var c = new CaseRepository().GetByID(Convert.ToInt32(model.casID));
                var _now = DateTime.Now;
                //if (c.casSLA == 0)  // Can changed casetype if old case not have SLA
                //{
                    model.casIDSummary = null;
                    model.casSummary = string.Format("{0}", model.casLevel1);

                    if (model.casIDLevel2.HasValue)
                    {
                        model.casIDSummary = model.casIDLevel2;
                        if (string.IsNullOrEmpty(model.casLevel2))
                        {
                            model.casLevel2 = c.casLevel2;
                        }
                        model.casSummary += string.Format(" > {0}", model.casLevel2);
                    }
                    if (model.casIDLevel3.HasValue)
                    {
                        model.casIDSummary = model.casIDLevel3;
                        if (string.IsNullOrEmpty(model.casLevel3))
                        {
                            model.casLevel3 = c.casLevel3;
                        }
                        model.casSummary += string.Format(" > {0}", model.casLevel3);
                    }
                    if (model.casIDLevel4.HasValue)
                    {
                        model.casIDSummary = model.casIDLevel4;
                        if (string.IsNullOrEmpty(model.casLevel4))
                        {
                            model.casLevel4 = c.casLevel4;
                        }
                        model.casSummary += string.Format(" > {0}", model.casLevel4);
                    }
                    if (model.casIDLevel5.HasValue)
                    {
                        model.casIDSummary = model.casIDLevel5;
                        if (string.IsNullOrEmpty(model.casLevel5))
                        {
                            model.casLevel5 = c.casLevel5;
                        }
                        model.casSummary += string.Format(" > {0}", model.casLevel5);
                    }

                    //if (model.casSLA.HasValue && model.casSLA > 0)
                    //{
                    //    model.casDueDate = _now.AddMinutes((double)model.casSLA);
                    //}

                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        actID = 0,
                        casID = Convert.ToInt32(model.casID),
                        cltID = EnumType.LogType.EditCase,
                        cslNote = string.Format("From: {0}<br/>To: {1}", c.casSummary, model.casSummary),
                        cslDesc = string.Empty,
                        cslCreatedOn = _now,
                        cslCreatedBy = AppUtils.Session.User.usrID,
                        cslModifiedOn = _now,
                        cslModifiedBy = AppUtils.Session.User.usrID
                    });
                //}
                //else
                //{
                //    // set prev data
                //    model.casIDLevel1 = c.casIDLevel1;
                //    model.casIDLevel2 = c.casIDLevel2;
                //    model.casIDLevel3 = c.casIDLevel3;
                //    model.casIDLevel4 = c.casIDLevel4;
                //    model.casIDLevel5 = c.casIDLevel5;
                //    model.casLevel1 = c.casLevel1;
                //    model.casLevel2 = c.casLevel2;
                //    model.casLevel3 = c.casLevel3;
                //    model.casLevel4 = c.casLevel4;
                //    model.casLevel5 = c.casLevel5;
                //    model.casIDSummary = c.casIDSummary;
                //    model.casSummary = c.casSummary;
                //    model.casSLA = c.casSLA;
                //    model.casDueDate = c.casDueDate;

                //    new CaseLogsRepository().AddByEntity(new CaseLog
                //    {
                //        actID = 0,
                //        casID = Convert.ToInt32(model.casID),
                //        cltID = EnumType.LogType.EditCase,
                //        cslNote = "แก้ไข Note",
                //        cslDesc = string.Empty,
                //        cslCreatedOn = _now,
                //        cslCreatedBy = AppUtils.Session.User.usrID,
                //        cslModifiedOn = _now,
                //        cslModifiedBy = AppUtils.Session.User.usrID
                //    });
                //}

                model.casModifiedOn = _now;
                model.casModifiedBy = AppUtils.Session.User.usrID;

                new CaseRepository().DetailUpdate(model);

                strSuccess = "Success";
                //model.casIDSummary = null;
                //model.casSummary = string.Format("{0}", model.casLevel1);

                //if (model.casIDLevel2.HasValue)
                //{
                //    model.casIDSummary = model.casIDLevel2;
                //    if (string.IsNullOrEmpty(model.casLevel2))
                //    {
                //        model.casLevel2 = c.casLevel2;
                //    }
                //    model.casSummary += string.Format(" > {0}", model.casLevel2);
                //}

                //if (model.casSLA.HasValue && model.casSLA > 0)
                //{
                //    model.casDueDate = _now.AddMinutes((double)model.casSLA);
                //}                                       
                //}
                //else 
                //{
                //    // set prev data
                //    model.casIDLevel1 = c.casIDLevel1;
                //    model.casIDLevel2 = c.casIDLevel2;
                //    model.casLevel1 = c.casLevel1;
                //    model.casLevel2 = c.casLevel2;
                //    model.casIDSummary = c.casIDSummary;
                //    model.casSummary = c.casSummary;
                //    model.casSLA = c.casSLA;
                //    model.casDueDate = c.casDueDate;

                //    new CaseLogsRepository().AddByEntity(new CaseLogs
                //    {
                //        actID = 0,
                //        casID = Convert.ToInt32(model.casID),
                //        cltID = EnumType.LogType.EditCase,
                //        cslNote = "แก้ไข Note",
                //        cslDesc = string.Empty,
                //        cslCreatedOn = _now,
                //        cslCreatedBy = AppUtils.Session.User.usrID,
                //        cslModifiedOn = _now,
                //        cslModifiedBy = AppUtils.Session.User.usrID
                //    });
                //}               
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }

            return Json(new { strSuccess = strSuccess, strError = strError }, 0);
        }

        private DateTime? CalcDueDate(NewCaseModel model, DateTime _dt)
        {            
            return _dt;
        }

        public ActionResult listFiles(int id)
        {
            var list = new CaseAttachFilesRepository().getFiles(id);
            return PartialView(list);
        }

        public void uploadFiles()
        {            
            if (Request["casID"] != null)
            {
                int casID = Convert.ToInt32(Request["casID"]);       
                var cas = new CaseRepository().GetByID(casID);

                foreach (string s in Request.Files)
                {
                    try
                    {
                        var file = Request.Files[s];
                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            string newfilename = string.Format("{0}_{1}", cas.casIDName, fileName);
                            string savefilepath = Server.MapPath("~/Uploadfiles/") + newfilename;                          
                            // Upload File
                            file.SaveAs(savefilepath);
                            var newFile = new FileInfo(savefilepath);
                            //While File is not accesable because of writing process
                            while (IsFileLocked(newFile)) { }

                            CaseAttachFile data = new CaseAttachFile() { 
                                casID = casID,
                                cafFilename = newfilename,
                                cafFileType = fileExtension,
                                cafCreatedOn = DateTime.Now,
                                cafCreatedBy = AppUtils.Session.User.usrID
                            };
                            if (new CaseAttachFilesRepository().AddByEntity(data) > 0)
                            {
                                new CaseLogsRepository().AddByEntity(new CaseLog
                                {
                                    actID = 0,
                                    casID = casID,
                                    cltID = EnumType.LogType.AttachFile,
                                    cslNote = string.Format("Add file: {0}", newfilename),
                                    cslDesc = string.Empty,
                                    cslCreatedBy = AppUtils.Session.User.usrID,
                                    cslCreatedOn = DateTime.Now,
                                    cslModifiedOn = DateTime.Now,
                                    cslModifiedBy = AppUtils.Session.User.usrID
                                });
                            }

                            //    Response.Write(savefilename);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
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

        [HttpPost]
        public void removeFile(int id)
        {
            var data = new CaseAttachFilesRepository().GetByID(id);
            if (data != null) 
            {                
                string filepath = Server.MapPath("~/Uploadfiles/") + data.cafFilename;
                // remove file
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                // add log
                new CaseLogsRepository().AddByEntity(new CaseLog
                {
                    actID = 0,
                    casID = data.casID,
                    cltID = EnumType.LogType.AttachFile,
                    cslNote = string.Format("Remove file: {0}", data.cafFilename),
                    cslDesc = string.Empty,
                    cslCreatedBy = AppUtils.Session.User.usrID,
                    cslCreatedOn = DateTime.Now,
                    cslModifiedOn = DateTime.Now,
                    cslModifiedBy = AppUtils.Session.User.usrID
                });

                // delete data
                new CaseAttachFilesRepository().RemoveByID(id);   
            }

        }

        #endregion

        #region Aood
        public ActionResult CaseDetail(decimal Id)
        {
            CaseModels d = new CaseModels();
            d.CaseDetails = new CaseRepository().GetCaseDetailById(Id, "");            
            //CaseModels d = new CaseRepository().GetCaseDetailById(Id, "");            
            return View("CaseDetail", d);
        }

        public ActionResult AddNote()
        {

            return View("AddNote");
        }

        public JsonResult DoSaveNote(int CaseId, string Remark)
        {
            new CaseRepository().DoSaveNote(CaseId, Remark, AppUtils.Session.User.usrID, AppUtils.Session.User.usrID);
            return Json(new { }, 0);
        }

        public ActionResult ChangeStatus()
        {
            CaseModels d = new CaseModels();
            d.CaseStatus = new CaseRepository().GetStatus("");
            d.CaseStatus.Insert(0, new sp_GetStatus_Result
            {
                cssID = -1,
                cssName = "เลือก Status"
            });
            return View("ChangeStatus", d);
        }

        public JsonResult DoSaveStatus(int CaseId, int StatusId, string Remark)
        {
            new CaseRepository().DoSaveStatus(CaseId, StatusId, Remark, 10, AppUtils.Session.User.usrID,AppUtils.Session.User.usrID);
            return Json(new { }, 0);

        }

        public JsonResult DoSaveAccept(int CaseId)
        {
            new CaseRepository().DoSaveAccept(CaseId, AppUtils.Session.User.usrID, Convert.ToInt32(AppUtils.Session.User.grpID));
            // Remove from notifications
            var list_noti = new NotificationRepository().getByReceivedCase(CaseId, AppUtils.Session.User.usrID);
            if (list_noti != null && list_noti.Count() > 0)
            {
                foreach (var item in list_noti)
                {
                    new NotificationRepository().setDisabled(item.notID);
                }
            }
            return Json(new { }, 0);
        }

        public JsonResult DoSaveReject(int CaseId)
        {
            new CaseRepository().DoSaveReject(CaseId, AppUtils.Session.User.usrID, 0);
            var list_noti = new NotificationRepository().getBySenderCase(CaseId, AppUtils.Session.User.usrID);
            if (list_noti != null && list_noti.Count() > 0)
            {
                foreach (var item in list_noti)
                {
                    new NotificationRepository().setDisabled(item.notID);
                }
            }
            return Json(new { }, 0);

        }

        public ActionResult CloseCase()
        {

            var model = new CaseViewModelLocalization();
            model.list_status_reason = ConfigurationManager.AppSettings["CASE_Status_Reason"].ToString().Split('|').ToList();
            return View("CloseCase",model);
        }

        public JsonResult DoSaveCloseCase(int CaseId, string Status, string Remark)
        {
            new CaseRepository().DoSaveClosed(CaseId, Status, Remark, 11, AppUtils.Session.User.usrID, AppUtils.Session.User.usrID);
            return Json(new { }, 0);
        }

        public ActionResult Dispatch()
        {
            CaseModels d = new CaseModels();
            d.UserGroups = new CaseRepository().GetGroupUsers();
            d.UserGroups.Insert(0, new sp_GetGroupUser_Result
            {
                grpID = -1,
                grpName = "เลือก Group"

            });
            return View("Dispatch", d);
        }

        public JsonResult DoSaveDispatch(int CaseId, int GroupId, string Remark)
        {
            new CaseRepository().DoSaveDispatch(CaseId, GroupId, Remark, AppUtils.Session.User.usrID, AppUtils.Session.User.usrID);
            return Json(new { }, 0);
        }

        public ActionResult AssignCase()
        {
            CaseModels d = new CaseModels();
            d.Users = new CaseRepository().GetStatus();
            d.Users.Insert(0, new sp_GetUser_Result
            {
                usrID = -1,
                usrFirstName = "เลือก User"
            });
            return View("Assign", d);
        }

        public JsonResult DoSaveAssign(int CaseId, int UserId, string Remark)
        {
            new CaseRepository().DoSaveAssign(CaseId, UserId, Remark, AppUtils.Session.User.usrID, AppUtils.Session.User.usrID);

            var c = new CaseRepository().GetByID(CaseId);

            // Notification
            var not = new Notification()
            {
                nttID = AppUtils.NotificationType.AssignCase,
                Sender = AppUtils.Session.User.usrID,
                refID = CaseId,
                Receiver = UserId,
                notNote = Remark,
                notDesc = string.Format("{0} has assign to you", c.casIDName),
                notRead = false,
                notActive = true,
                notCreatedOn = DateTime.Now,
                notCreatedBy = AppUtils.Session.User.usrID,
                notModifiedOn = DateTime.Now,
                notModifiedBy = AppUtils.Session.User.usrID
            };
            new NotificationRepository().AddByEntity(not);

            return Json(new { }, 0);
        }

        public ActionResult SendEmail(int? id)
        {
            var cas = new CaseRepository().GetByID(Convert.ToInt32(id));
            string contactname = string.Empty;
            string phone = new ActivitiesRepository().GetByID(cas.actID).actChannelData;
            var user = new UserRepository().getByID(Convert.ToInt32(cas.casOwnerID));
            if (cas.ctaID > 0)
            {
                var contact = new ContactRepository().GetById(cas.ctaID);
                contactname = contact.ctaFullName;
            }
            string subject = string.Format("{0} : {1} : {2}", cas.casSummary, cas.casIDName, contactname);
           
            CaseModels d = new CaseModels();
            d.MailContents = new MailContent();
            d.MailContents.MailSubject = subject;
            d.MailContents.MailTo = getMailTo(cas.casIDLevel2);
            d.MailContents.MailCc = getMailCC(cas.casIDLevel2);
            d.MailContents.MailAttachments = new CaseAttachFilesRepository().getFiles(Convert.ToInt32(id));

            string body = "CASE ID :" + cas.casIDName + "</br>"
                        + "CASE :" + cas.casSummary + "</br>"
                        + "CONTACT :" + contactname + "</br>"
                        + "PHONE :" + phone + "</br>"
                        + "DATE :" + cas.casCreatedOn + "</br>"
                        + "AGENT :" + user.usrFirstName + " " + user.usrLastName + "</br>"
                        + "DUE DATE :" + cas.casDueDate + "</br>"
                        + "Channel :" + cas.casLevel3 + "</br>"
                        +"Channel :" + cas.casLevel6 + "</br>";


            // Add wording to end of body in this cases below
            if (cas.casIDLevel1 == 1) // สอบถามข้อมูลที่วไป
            {
                body += "Commerce Type :" + cas.cascommerceType + "</br>";
                body += "Product Category :" + cas.casproductCategory + "</br>";
                body += "Service Category :" + cas.casserviceCategory + "</br>";
                body += "Delivery Type :" + cas.casdeliveryType + "</br>";
                body += "Value Range :" + cas.casvalueRange + "</br>";
                body += "Conversation Channel :" + cas.casconversationChannel + "</br>";
                body += "Reference Detail :" + cas.casreferenceDetail + "</br>";
                body += "Detail :" + cas.casdetail + "</br>";
                body += "NOTE :" + cas.casNote + "</br></br>";
                body += "รบกวนผู้เกี่ยวข้อง กรุณา Feedback ผลหรือสถานะการดำเนินการ ให้ Call Center ทราบภายใน 3 วัน นับจากวันที่ได้รับเรื่อง";
            }
            else //if (cas.casIDLevel1 == 2 || cas.casIDLevel1 == 3) // ร้องเรียน / บริการ
            {
                body += "Reference Detail :" + cas.casreferenceDetail + "</br>";
                body += "Detail :" + cas.casdetail + "</br>";
                body += "NOTE :" + cas.casNote + "</br></br>";
                body += "รบกวนผู้เกี่ยวข้อง กรุณา Feedback ผลหรือสถานะการดำเนินการ ให้ Call Center ทราบภายใน 1 วัน นับจากวันที่ได้รับเรื่อง";
            }

            d.MailContents.MailBody = body;
            //d.MailContents = MailUtils.createdMail(Convert.ToInt32(id));
            return View("SendEmail", d);
        }

        private string getMailTo(int? casIDLevel2)
        {
            string mail = ConfigurationManager.AppSettings["CASE_Email_ETDA_To"];
            if (casIDLevel2 > 140 && casIDLevel2 < 153)
            {
                if (casIDLevel2 == 141 || casIDLevel2 == 143 || casIDLevel2 == 144) mail = ConfigurationManager.AppSettings["CASE_Email_PACC_To"];
                else if (casIDLevel2 == 142) mail = ConfigurationManager.AppSettings["CASE_Email_PACC_To"] + "," + ConfigurationManager.AppSettings["CASE_Email_TCSD_To"];
                else if (casIDLevel2 == 145 || casIDLevel2 == 147 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_To"];
                else if (casIDLevel2 == 145 || casIDLevel2 == 147 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_To"];
                else if (casIDLevel2 == 146 || casIDLevel2 == 148 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_To"] + "," + ConfigurationManager.AppSettings["CASE_Email_FDA_To"];
                else if (casIDLevel2 == 151) mail = ConfigurationManager.AppSettings["CASE_Email_FDA_To"];
                //else mail = ConfigurationManager.AppSettings["CASE_Email_ETDA_To"];
            }
            else if (casIDLevel2 > 152 && casIDLevel2 < 162) mail =  ConfigurationManager.AppSettings["CASE_Email_ThaiCERT_To"];
            //else if (casIDLevel2 > 161 && casIDLevel2 < 174) return ConfigurationManager.AppSettings["CASE_Email_ETDA_To"];
            else if (casIDLevel2 > 173) mail =  ConfigurationManager.AppSettings["CASE_Email_OTO _To"];

            return mail;
            //}
        }
        private string getMailCC(int? casIDLevel2)
        {
            string mail = ConfigurationManager.AppSettings["CASE_Email_ETDA_CC"];
            if (casIDLevel2 > 140 && casIDLevel2 < 153)
            {
                if (casIDLevel2 == 141 || casIDLevel2 == 143 || casIDLevel2 == 144) mail = ConfigurationManager.AppSettings["CASE_Email_PACC_CC"];
                else if (casIDLevel2 == 142) mail = ConfigurationManager.AppSettings["CASE_Email_PACC_CC"] + "," + ConfigurationManager.AppSettings["CASE_Email_TCSD_CC"];
                else if (casIDLevel2 == 145 || casIDLevel2 == 147 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_CC"];
                else if (casIDLevel2 == 145 || casIDLevel2 == 147 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_CC"];
                else if (casIDLevel2 == 146 || casIDLevel2 == 148 || casIDLevel2 == 149 || casIDLevel2 == 150) mail = ConfigurationManager.AppSettings["CASE_Email_TCSD_CC"] + "," + ConfigurationManager.AppSettings["CASE_Email_FDA_CC"];
                else if (casIDLevel2 == 151) mail = ConfigurationManager.AppSettings["CASE_Email_FDA_CC"];
                //else mail = ConfigurationManager.AppSettings["CASE_Email_ETDA_CC"];
            }
            else if (casIDLevel2 > 152 && casIDLevel2 < 162) mail = ConfigurationManager.AppSettings["CASE_Email_ThaiCERT_CC"];
            //else if (casIDLevel2 > 161 && casIDLevel2 < 174) return ConfigurationManager.AppSettings["CASE_Email_ETDA_CC"];
            else if (casIDLevel2 > 173) mail = ConfigurationManager.AppSettings["CASE_Email_OTO _CC"];

            return mail;
        }

        //[HttpPost]
        //public ActionResult SaveUploadedFile(/*IEnumerable<HttpPostedFileBase>*/ HttpPostedFileBase httpFileCollection)
        //{
        //    List<HttpPostedFileBase> fAtt = new List<HttpPostedFileBase>();
        //    if (Session["HttpPostedFileBase"] != null)
        //        fAtt = (List<HttpPostedFileBase>)Session["HttpPostedFileBase"];
        //    fAtt.Add(httpFileCollection);
        //    Session["HttpPostedFileBase"] = fAtt;
        //    return Json(new { }, 0);
        //}

        //public JsonResult UploadRemoveFile(string FileName)
        //{
        //    List<HttpPostedFileBase> fAtt = new List<HttpPostedFileBase>();
        //    if (Session["HttpPostedFileBase"] != null)
        //        fAtt = (List<HttpPostedFileBase>)Session["HttpPostedFileBase"];
        //    var fd = (from f in fAtt
        //              where f.FileName == FileName
        //              select f).FirstOrDefault();
        //    if (fd != null)
        //        fAtt.Remove(fd);
        //    return Json(new { }, 0);
        //}

        public JsonResult SendMail(MailContent m)
        {
            string strSuccess = string.Empty;
            string strError = string.Empty;
            #region MailLog
            MailLog log = new MailLog();
            log.mailTo = m.MailTo;
            log.mailCC = m.MailCc;
            log.mailSubject = m.MailSubject;
            log.mailBody = HttpUtility.UrlDecode(m.MailBody);
            log.mailSending = DateTime.Now;
            #endregion

            try
            {                            
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(AppUtils.AppConfig.SMTPMAILFROM, "CRM ORIGIN");
                    mail.To.Add(m.MailTo.Trim().Replace(";", ","));

                    //if (!string.IsNullOrWhiteSpace(m.MailCc))
                        mail.CC.Add(m.MailCc.Trim().Replace(";", ","));
                    //if (!string.IsNullOrWhiteSpace(m.MailBcc))
                        mail.Bcc.Add(m.MailBcc.Trim().Replace(";", ","));
                    //set the content
                    mail.Subject = m.MailSubject;
                    mail.Body = HttpUtility.UrlDecode(m.MailBody);
                    mail.IsBodyHtml = true;
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;

                    if (!string.IsNullOrEmpty(m.attachFiles))
                    {                        
                        var attachFiles_selected = m.attachFiles.Split('&');
                        foreach (var selected_file in attachFiles_selected)
                        {
                            var selected_id = selected_file.Split('=')[1];
                            var attachFile = new CaseAttachFilesRepository().GetByID(Convert.ToInt32(selected_id));

                            if (attachFile != null)
                            {
                                string path_file = Server.MapPath("~/Uploadfiles/") + attachFile.cafFilename;
                                Attachment attachment = new Attachment(path_file, MediaTypeNames.Application.Octet);
                                ContentDisposition disposition = attachment.ContentDisposition;
                                disposition.CreationDate = System.IO.File.GetCreationTime(path_file);
                                disposition.ModificationDate = System.IO.File.GetLastWriteTime(path_file);
                                disposition.ReadDate = System.IO.File.GetLastAccessTime(path_file);
                                disposition.FileName = Path.GetFileName(path_file);
                                disposition.Size = new FileInfo(path_file).Length;
                                disposition.DispositionType = DispositionTypeNames.Attachment;
                                mail.Attachments.Add(attachment);                               
                            }
                        }                                                
                    }

                    using (var smtp = new SmtpClient(AppUtils.AppConfig.SMTPSERVER))
                    {
                        smtp.Port = int.Parse(AppUtils.AppConfig.SMTPPORT);
                        smtp.UseDefaultCredentials = false;
                        smtp.Send(mail);

                        new CaseLogsRepository().AddByEntity(new CaseLog
                        {
                            actID = 0,
                            casID = m.casID,
                            cltID = EnumType.LogType.Sendmail,
                            cslNote = string.Format("To: {0}<br/>CC: {1}<br/>Subject: {2}", m.MailTo, m.MailCc, m.MailSubject),
                            cslDesc = string.Empty,
                            cslCreatedBy = AppUtils.Session.User.usrID,
                            cslCreatedOn = DateTime.Now,
                            cslModifiedOn = DateTime.Now,
                            cslModifiedBy = AppUtils.Session.User.usrID
                        });                        
                    }
                }

                strSuccess = "Send Mail Success";
                log.mailStatus = "S";
            }
            catch (FormatException fe)
            {
                // model.Msg = MsgResultInfo.FromException(fe);                
                strError = fe.Message.ToString();
                log.mailStatus = "F";
                log.mailException = fe.Message;
            }
            catch (SmtpException se)
            {
                // model.Msg = MsgResultInfo.FromException(se);
                strError = se.Message.ToString();
                log.mailStatus = "F";
                log.mailException = se.Message;
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                log.mailStatus = "F";
                log.mailException = ex.Message;
            }
            finally
            {
                new MailLogRepository().AddByEntity(log);
            }

            return Json(new { strSuccess = strSuccess, strError = strError }, 0);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //TODO Handle the uploaded file here
            return View();
        }

        public ActionResult RemindFlow(int id)
        {
            CaseModels d = new CaseModels();
            var cas = new CaseRepository().GetByID(id);

            int flow_id = 1;
            if (cas.casRemindID.HasValue)
            {
                flow_id = Convert.ToInt32(cas.casRemindID) + 1; // Next Flow
                if (flow_id > 7)
                {
                    flow_id = 7;
                }
            }

            //d.RemindFlow = new CaseRemindFlowRepository().getByID(flow_id);
            //// Current Flow is call to L1 get Phone from Engineer Support
            //if (flow_id <= 3)
            //{
            //    var now = DateTime.Now;
            //    var es = new EngineerSupportRepository().getByCurrentDateTime(now);
            //    if (es != null)
            //    {
            //        d.RemindFlow.crfFlowPhone = es.espPhone;
            //    }
            //}

            return View("RemindFlow", d);
        }

        public JsonResult RemindFlowSubmit(int casID, int crfID, string status, int crftime)
        {
            // Updated Case
            new CaseRepository().UpdatedRemindFlow(casID, crfID, status);

            var _createdOn = DateTime.Now;
            var _nexttime = _createdOn.AddMinutes(crftime);
            // Add Case Log
            var crf = new CaseRemindFlowRepository().getByID(crfID);            
            new CaseLogsRepository().AddByEntity(new CaseLog
            {
                actID = 0,
                casID = casID,
                cltID = EnumType.LogType.RemindCase,
                cslNote = string.Format("Phone No: {0}<br/>Received: {1}", crf.crfFlowPhone, status),
                cslDesc = crf.crfFlowName,
                cslCreatedBy = AppUtils.Session.User.usrID,
                cslCreatedOn = _createdOn,
                cslModifiedOn = _createdOn,
                cslModifiedBy = AppUtils.Session.User.usrID
            });

            if (status.Equals("N"))
            {
                // Remove Previous Notification
                var notList = new NotificationRepository().getByRefID(casID);
                if (notList != null && notList.Count() > 0)
                {
                    foreach (var item in notList)
                    {
                        new NotificationRepository().setDisabled(item.notID);
                    }
                }

                // Notification
                var cas = new CaseRepository().GetByID(casID);
                var not = new Notification()
                {
                    nttID = AppUtils.NotificationType.RemindCase,
                    refID = casID,
                    notNote = string.Format("{0} has remind again", cas.casIDName),
                    notDesc = string.Format("at {0}", _nexttime),
                    notRead = false,
                    notActive = true,
                    notCreatedOn = _createdOn,
                    notCreatedBy = AppUtils.Session.User.usrID,
                    notModifiedOn = _createdOn,
                    notModifiedBy = AppUtils.Session.User.usrID
                };
                new NotificationRepository().AddByEntity(not);
            }
            return Json(new { }, 0);
        }

        #endregion
    }
}
