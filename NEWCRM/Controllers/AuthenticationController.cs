using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NEWCRM.Common;
using NEWCRM.Models;

namespace NEWCRM.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public ContentResult Signin()
        {
            try
            {
                string username = Request["username"];
                string password = Request["password"];

                if (string.IsNullOrEmpty(username)) {
                    throw new Exception("Username incorrect");
                }
                if (string.IsNullOrEmpty(password)) {
                    throw new Exception("Password incorrect");
                }

                var rep = new UserRepository();
                var usr = rep.getBySignin(username, password);
                if (usr == null)
                {
                    throw new Exception("Username or Password incorrect");
                }
                else 
                {
                    var rep_log = new AuthenLogRepository();
                    rep_log.AddByEntity(new AuthenLog() { 
                        usrID = usr.usrID,
                        authCreatedOn = DateTime.Now
                    });

                    AppUtils.Session.User = usr;

                    //#region Aood
                    //FormsAuthentication.SetAuthCookie(usr.usrID.ToString(), false);                   
                    //if (OnlineUser.userObj.Where(item => item.sessionId == HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                    //    OnlineUser.userObj.Remove(OnlineUser.userObj.Where(item => item.sessionId == HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());
                    //OnlineUser.AddOnlineUser("", AppUtils.Session.User.usrFirstName, AppUtils.Session.User.usrID.ToString(), HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), true);
                    //#endregion
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true; 
                return Content(ex.Message, "text/html");
            }

        }

        public ActionResult Signout()
        {
            AppUtils.Session.ClearSession();
            return RedirectToAction("Index", "Authentication");
        }

        public ActionResult LockScreen()
        {

            return PartialView();
        }

        public ActionResult G8Signin()
        {
            try
            {  
                int usrId = Convert.ToInt32(Request["usrid"]);
                var rep = new UserRepository();
                var usr = rep.getByID(usrId);
                if (usr == null)
                {
                    throw new Exception("User agent not found");
                }
                else
                {
                    var rep_log = new AuthenLogRepository();
                    rep_log.AddByEntity(new AuthenLog()
                    {
                        usrID = usr.usrID,
                        authCreatedOn = DateTime.Now
                    });

                    AppUtils.Session.User = usr;

                    //#region Aood
                    //FormsAuthentication.SetAuthCookie(usr.usrID.ToString(), false);
                    //if (OnlineUser.userObj.Where(item => item.sessionId == HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                    //    OnlineUser.userObj.Remove(OnlineUser.userObj.Where(item => item.sessionId == HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());
                    //OnlineUser.AddOnlineUser("", AppUtils.Session.User.usrFirstName, AppUtils.Session.User.usrID.ToString(), HttpContext.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), true);
                    //#endregion
                }

                // return Content(string.Empty);

                return RedirectToAction("Index", "Dashboard");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                return Content(ex.Message, "text/html");
            }

            //return RedirectToAction("Index", "Dashboard");

        }
    }
}
