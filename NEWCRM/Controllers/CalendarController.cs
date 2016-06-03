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
    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/

        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult getCalendar()
        {
            var rep = new CalendarRepository();
            //var model = new CalendarViewModel();


            var list = rep.getListByUser(AppUtils.Session.User.usrID);
            List<string> allEvents = new List<string>();
            foreach (var item in list)
            {
                string str = string.Format("id:'{9}', title: '{0}',  start: new Date({1}, {2}, {3}, {4}, {5}), end: new Date({1}, {2}, {3}, {6}, {7}), allDay: false, className: '{8}'", HttpUtility.JavaScriptStringEncode(item.cldMessage), item.cldDate.Year, item.cldDate.Month - 1, item.cldDate.Day, item.cldTimestart.Value.Hours, item.cldTimestart.Value.Minutes, item.cldTimeend.Value.Hours, item.cldTimeend.Value.Minutes, item.cldColor, item.cldID);
                //string str = string.Format("{'{0}',{1}, {2}, {3}, {4}, {5}}", item.cldMessage, item.cldDate.Year, item.cldDate.Month, item.cldDate.Day, item.cldTimestart.Value.Hours, item.cldTimestart.Value.Minutes);
                allEvents.Add("{ " + str + " }");
            }



            //string ev = " { title: '11111',   start: new Date(2015, 10, 27, 11, 0), end: new Date(2015, 10, 27, 14, 0), allDay: false, className: 'event-warning' } ";
            //ev += ",{ title: '22222',   start: new Date(2015, 10, 28, 8, 30), end: new Date(2015, 10, 28, 10, 30), allDay: false, className: 'event-success' }";
            //ev += ", { title: 'Test 3',   start: new Date(2015, 9, 28, 19, 0), end: new Date(2015, 9, 28, 22, 30), allDay: false, className: 'event-danger' }";
            //ev += ", { title: 'Test 4',   start: new Date(2015, 9, 29, 19, 0), end: new Date(2015, 9, 29, 22, 30), allDay: false, className: 'event-warning' }";
            //ev += ", { title: 'Test 5',   start: new Date(2015, 9, 30, 9, 0), end: new Date(2015, 9, 30, 18, 0), allDay: false, className: 'event-default' }";

            string evt = string.Empty;
            if (allEvents.Count() > 0)
            {
                evt = string.Join(",", allEvents.ToArray());
            }

            ViewData["evt"] = evt;
            return PartialView();
        }

        public ContentResult TaskSave(Calendar data)
        {
            var rep = new CalendarRepository();
            try
            {
                var reqFormDate = Request["cldFormDate"];
                data.cldDate = DateTime.ParseExact(reqFormDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                if (data.cldID > 0)
                {
                    data.cldModifiedOn = DateTime.Now;
                    data.cldModifiedBy = AppUtils.Session.User.usrID;
                    if (rep.UpdateByEntity(data) < 1)
                    {
                        throw new Exception("Save updated task failure");
                    }                    
                }
                else
                {
                    data.cldCreatedOn = DateTime.Now;
                    data.cldCreatedBy = AppUtils.Session.User.usrID;
                    data.cldActive = true;
                    if (rep.AddByEntity(data) < 1)
                    {
                        throw new Exception("Save new task failure");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
            return Content(string.Empty);
        }

        public void updateTask()
        {
            var id = Request["id"];
            var newBegin = Request["start"];
            DateTime cldStart = Convert.ToDateTime(newBegin);
            var newEnd = Request["end"];
            DateTime cldEnd = Convert.ToDateTime(newEnd);

            Calendar data = new Calendar();
            data.cldID = Convert.ToInt32(id);
            data.cldDate = cldStart.Date;
            data.cldTimestart = cldStart.TimeOfDay;
            data.cldTimeend = cldEnd.TimeOfDay;
            data.cldModifiedBy = AppUtils.Session.User.usrID;
            data.cldModifiedOn = DateTime.Now;

            var rep = new CalendarRepository();
            rep.UpdateDateTime(data);

            return;
        }

        [HttpPost]
        public ActionResult getTask(int id)
        {
            //string sData = "{ id: 1, cldMessage: 'Test', cldDate: '2015-10-05' }";
            //return Content(sData, "application/json");
            //Calendar data = new Calendar() { 
            //    cldID = 1,
            //    cldDate = DateTime.Now,
            //    cldMessage = "Test"
            //};
            var rep = new CalendarRepository();
            var data = rep.getByID(id);

            var dict = new Dictionary<string, string>();
            dict["id"] = data.cldID.ToString();
            dict["date"] = data.cldDate.Date.ToString("dd/MM/yyyy");
            dict["message"] = data.cldMessage;
            dict["timestart"] = data.cldTimestart.Value.ToString(@"hh\:mm");
            dict["timeend"] = data.cldTimeend.Value.ToString(@"hh\:mm");
            dict["color"] = data.cldColor;

            //var dict = new Dictionary<string, string>();
            //dict["id"] = "1";
            //dict["date"] = "30/10/2015";
            //dict["message"] = "Test";
            //dict["timestart"] = "08:00";
            //dict["timeend"] = "10:00";
            //dict["color"] = "event-success";

            return Json(dict, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Remove(int id)
        {
            var rep = new CalendarRepository();
            Calendar data = new Calendar();
            data.cldID = id;
            data.cldModifiedBy = AppUtils.Session.User.usrID;
            data.cldModifiedOn = DateTime.Now;
            rep.UpdateRemove(data);
        }

    }
}
