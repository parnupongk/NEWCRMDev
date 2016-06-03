using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Common;
using NEWCRM.Models;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class UserController : Controller
    {
        //
        // GET: /User/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult getProfile()
        {
            var rep = new UserRepository();
            var model = new UserViewModel();

            model.item_profile = rep.getProfile(AppUtils.Session.User.usrID);

            return View(model);
        }

        public ActionResult getTimeline()
        {
            var rep = new UserRepository();
            var model = new UserViewModel();

            model.list_timeline = rep.getProfileTimeline(AppUtils.Session.User.usrID);

            return PartialView(model);
        }

        [HttpPost]
        public ContentResult changePassword()
        {
            var password_old = Request["password_old"];
            var password_new = Request["password_new"];
            var password_confirm = Request["password_confirm"];

            try
            {
                var rep = new UserRepository();
                var usr = rep.getByID(AppUtils.Session.User.usrID);

                if (!password_old.Equals(usr.usrPassword)) {
                    throw new Exception("Old password incorrect");                
                }

                if (password_old.Equals(password_new))
                {
                    throw new Exception("New password incorrect");
                }

                if (!password_new.Equals(password_confirm)) {
                    throw new Exception("Confirm password incorrect");
                }

                rep.changePassword(password_new, usr.usrID);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        public void uploadAvatar()
        {            
            foreach (string s in Request.Files)
            {
                try
                {
                    var usrID = AppUtils.Session.User.usrID;
                    var file = Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string savefilename = string.Format("{0}_{1}", usrID, fileName);
                        string savefilepathe = Server.MapPath("~/Images/users/") + savefilename;

                        // Updated contact
                        if (new UserRepository().updateAvatar(Convert.ToInt32(AppUtils.Session.User.usrID), savefilename) > 0)
                        {
                            // Updated Session User
                            AppUtils.Session.User = new UserRepository().getByID(usrID); 
                        }

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
    }
}
