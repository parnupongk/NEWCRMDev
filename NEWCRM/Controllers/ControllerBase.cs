using NEWCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NEWCRM.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void ExecuteCore()
        {
            CheckAuthenticate();

            //int culture = 0;
            //if (this.Session == null || this.Session["CurrentCulture"] == null)
            //{

            //    int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
            //    this.Session["CurrentCulture"] = culture;
            //}
            //else
            //{
            //    culture = (int)this.Session["CurrentCulture"];
            //}
            //// calling CultureHelper class properties for setting  
            //CultureHelper.CurrentCulture = culture;

            base.ExecuteCore();
        }

        //protected override bool DisableAsyncSupport
        //{
        //    get { return true; }
        //}

        //public ActionResult ChangeCurrentCulture(int id)
        //{
        //    //  
        //    // Change the current culture for this user.  
        //    //  
        //    CultureHelper.CurrentCulture = id;
        //    //  
        //    // Cache the new current culture into the user HTTP session.   
        //    //  
        //    Session["CurrentCulture"] = id;
        //    //  
        //    // Redirect to the same page from where the request was made!   
        //    //  
        //    return Redirect(Request.UrlReferrer.ToString());
        //}

        public ActionResult CheckAuthenticate()
        {
            if (AppUtils.Session.User == null) {
                return goExceptionPage("Session Timeout", string.Empty);
            }
            return null;
        }

        public ActionResult goExceptionPage(string errmsg, string errdesc)
        {
            TempData["errmsg"] = errmsg;
            TempData["errdesc"] = errdesc;
            return RedirectToAction("Index", "Exception");
        }

        //public static string List2JSON_DataTable(IEnumerable<object> obj)
        //{
        //    JavaScriptSerializer json = new JavaScriptSerializer();
        //    string output = json.Serialize(GetObjectArray(obj));
        //    output = "{ \"data\":" + output + "}";
        //    return output;            
        //}

        //private static IEnumerable<object> GetObjectArray<T>(IEnumerable<T> obj)
        //{
        //    return obj.Select(o => o.GetType().GetProperties().Select(p => p.GetValue(o, null)));
        //}

        //public ActionResult PageRender(PagingModel model)
        //{           
        //    return PartialView("PagingUC", model);
        //}      
    }

    public class LoginExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // check  sessions here
            if (AppUtils.Session.User == null)
            {               
                filterContext.Result = new RedirectResult("~/Authentication/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
