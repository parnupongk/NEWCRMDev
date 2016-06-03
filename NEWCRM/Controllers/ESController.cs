using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Common;
using NEWCRM.Models;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class ESController : ControllerBase
    {        
        public ActionResult Index()
        {
            if (AppUtils.Session.User.grpID > 2)
            {
                return goExceptionPage("Permission Denied", "You not have to access this section");                
            }
            return View();            
        }

        public ContentResult AddNew(EngineerSupport data)
        {
            try
            {
                var espStart = Request["espStart"];
                var espEnd = Request["espEnd"];

                if (string.IsNullOrEmpty(espStart.ToString()))
                {
                    throw new Exception("#espStart");
                }
                if (string.IsNullOrEmpty(espEnd.ToString()))
                {
                    throw new Exception("#espEnd");
                }
                if (string.IsNullOrWhiteSpace(data.espEmail))
                {
                   throw new Exception("#espEmail");                   
                }
                if (string.IsNullOrWhiteSpace(data.espPhone))
                {                   
                   throw new Exception("#espPhone");                   
                }

                data.espStart = DateTime.ParseExact(string.Format("{0} 08:00:00",espStart), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                data.espEnd = DateTime.ParseExact(string.Format("{0} 07:59:59",espEnd), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                data.espCreatedOn = DateTime.Now;
                data.espCreatedBy = AppUtils.Session.User.usrID;
                data.espActive = true;

                new EngineerSupportRepository().AddByEntity(data);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                return Content(ex.Message);
            }
        }

        public ActionResult getList()
        {
            var list = new EngineerSupportRepository().getList();
            return PartialView(list);
        }

        public ActionResult Edit(int id)
        {
            var data = new EngineerSupportRepository().getByID(id);
            return PartialView(data);
        }

        public ContentResult EditSave(EngineerSupport data)
        {
            try
            {
                var espStart = Request["espStart"];
                var espEnd = Request["espEnd"];

                if (string.IsNullOrEmpty(espStart.ToString()))
                {
                    throw new Exception("#espStart");
                }
                if (string.IsNullOrEmpty(espEnd.ToString()))
                {
                    throw new Exception("#espEnd");
                }
                if (string.IsNullOrWhiteSpace(data.espEmail))
                {
                    throw new Exception("#espEmail");
                }
                if (string.IsNullOrWhiteSpace(data.espPhone))
                {
                    throw new Exception("#espPhone");
                }

                data.espStart = DateTime.ParseExact(string.Format("{0} 08:00:00", espStart), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                data.espEnd = DateTime.ParseExact(string.Format("{0} 07:59:59", espEnd), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                data.espModifiedOn = DateTime.Now;
                data.espModifiedBy = AppUtils.Session.User.usrID;

                new EngineerSupportRepository().UpdateEntity(data);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                return Content(ex.Message);
            }
        }

        public ContentResult Remove(int id)
        {
            try
            {
                new EngineerSupportRepository().setRemove(id);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                return Content(ex.Message);
            }
        }
    }
}
