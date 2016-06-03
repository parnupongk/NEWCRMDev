using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Common;
using System.Web.Routing;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class InboundController : ControllerBase
    {
        //
        // GET: /Inbound/        
        public ActionResult Index()
        {
            if (Request.Params["phone"] != null)
            {
                // Clear Session
                ClearSession();

                string conid = null;
                int dnis = 0;
                string DNIS_Name = "";

                if (Request.Params["conid"] != null)
                {
                    conid = Request.Params["conid"];
                }


                if (Request.Params["dnis"] != null)
                {
                    dnis = Convert.ToInt32(Request.Params["dnis"]);
                    var rep = new DNISRepository();
                    var d = rep.GetById(dnis);

                    try
                    {
                        DNIS_Name = d.DNIS_Name;

                    }
                    catch (Exception ex)
                    {
                        DNIS_Name = "Unknow";
                    }

                }

                ViewBag.DNIS_Name = DNIS_Name;

                //check data from batch or markcall
                var dataChn = AppUtils.Channels.Inbound;


                //bool b = Request.Params["phone"].Contains("F-");
                //if (b)
                //{
                //    dataChn = AppUtils.Channels.Batch;
                //}
                //else
                //{
                //    dataChn = AppUtils.Channels.Inbound;
                //}

                var act = new Activities()
                {
                    chnID = dataChn,
                    atsID = 0,
                    actChannelData = Request.Params["phone"],  //if dataChn = bt : actChannelData = OBJFORMID
                    actCreatedOn = DateTime.Now,
                    actCreatedBy = AppUtils.Session.User.usrID,
                    actModifiedOn = DateTime.Now,
                    actModifiedBy = AppUtils.Session.User.usrID,
                    actStart = DateTime.Now,
                    conID = conid
                };

                var rep_act = new ActivitiesRepository();
                rep_act.AddByEntity(act);

                // New Session
                AppUtils.Session.Activity = act;
                AppUtils.Session.Activity.conID = conid;
            }
            return View();
            //if (Request.Params["phone"] != null)
            //{   
            //    // Clear Session
            //    ClearSession();                

            //    var act = new Activities()
            //    {
            //        chnID = AppUtils.Channels.Inbound,
            //        atsID = 0,
            //        actChannelData = Request.Params["phone"],
            //        actCreatedOn = DateTime.Now,
            //        actCreatedBy = AppUtils.Session.User.usrID,
            //        actModifiedOn = DateTime.Now,
            //        actModifiedBy = AppUtils.Session.User.usrID,
            //        actStart = DateTime.Now
            //    };
               
            //    var rep_act = new ActivitiesRepository();
            //    rep_act.AddByEntity(act);

            //    // New Session
            //    AppUtils.Session.Activity = act;
            //}
            //return View();
        }

        public ActionResult MakeCall()
        {
            return View();
        }

        public ActionResult doMakeCall()
        {
            var phoneno = Request["phoneno"];
            //return RedirectToAction("Index", "Inbound", "?phone=" + phoneno);
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Inbound", action = "Index", phone = phoneno }));
        }
       
        public ActionResult getContactByPhone()
        {
            if (!string.IsNullOrEmpty(AppUtils.Session.Activity.actChannelData))
            {
                var rep = new ContactRepository();
                ViewData["list_contact"] = rep.getInboundContact(AppUtils.Session.Activity.actChannelData);                
            }
            return PartialView("getContact");
        }

        public ActionResult getContactBySearch()
        {
            if (Request != null)
            {
                var rep = new ContactRepository();
                if (Request["search"] != null)
                {
                    ViewData["list_contact"] = rep.getInboundContact(null, Request["search"]);
                }
            }
            return PartialView("getContact");
        }       
        
        [HttpPost]
        public void ClearSession()
        {
            AppUtils.Session.Activity = null;
            AppUtils.Session.CaseTypes = null;
            AppUtils.Session.NewCase = null;
        }
    }
}
