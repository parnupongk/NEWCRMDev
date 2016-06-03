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
    public class NotificationController : ControllerBase
    {
        //
        // GET: /Notification/
        public string getTotalByUserID()
        {
            var usrID = AppUtils.Session.User.usrID;
            var rep = new NotificationRepository();
            var total = rep.getTotal(usrID);
            return Convert.ToString(total);
        }

        [HttpPost]
        public void setDisabled(int id)
        {
            new NotificationRepository().setDisabled(id);
        }

        public ActionResult NotificationList()
        {
            var rep = new NotificationRepository();            
            var list = rep.getByUser(AppUtils.Session.User.usrID);

            // Mark all as read
            rep.updateRead(AppUtils.Session.User.usrID);

            return PartialView(list);        
        }
    }
}
