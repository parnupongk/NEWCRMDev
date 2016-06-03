using NEWCRM.Common;
using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class DashboardController : ControllerBase
    {
        //
        // GET: /Dashboard/
        #region Aeh
        public ActionResult TaskCalendar()
        {
            var rep = new CalendarRepository();
            var model = new CalendarViewModel();
            var list = rep.getListByUser(AppUtils.Session.User.usrID);
            model.list_item = list.OrderByDescending(d => d.cldDate).ThenBy(t => t.cldTimestart).Take(4).ToList();
            return PartialView(model);
        }

        public ActionResult Favourite()
        {
            var list = new CaseRepository().getFavourite(AppUtils.Session.User.usrID);
            return PartialView(list);
        }

        [HttpPost]
        public string getPieCaseStatus()
        {
            string _json = string.Empty;
            var result = new NEWCRM.Models.DashbordRepository().Get_Dashbord_CaseStatus(DateTime.Now.Date.ToString("yyyyMMdd"));
            if (result != null)
            {
                _json = new JavaScriptSerializer().Serialize(result);
            }
            return _json;
        }

        [HttpPost]
        public string getPendingCases()
        {
            string _json = string.Empty;
            var result = new NEWCRM.Models.DashbordRepository().Get_Dashbord_CasePending(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            if (result != null)
            {
                _json = new JavaScriptSerializer().Serialize(result);
            }
            return _json;        
        }

        #endregion

        #region Aood
        public ActionResult Index()
        {

            DashBordModels d = new DashBordModels();
            if (AppUtils.Session.User != null)
            {
                d.UserGroup = new UserRepository().GetUserInGroupById(AppUtils.Session.User.usrID);
                // Annoncement
                var ann = new AnnouncementRepository().getAll();
                d.annoucement = string.Join("," , ann.Select(a => a.annText).ToArray());
            }
            return View(d);
        }
        #endregion


    }
}
