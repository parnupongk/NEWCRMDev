using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NEWCRM.Controllers
{
    public class ExceptionController : Controller
    {
        //
        // GET: /Exception/

        public ActionResult Index()
        {
            ViewBag.errmsg = TempData["errmsg"];
            ViewBag.errdesc = TempData["errdesc"];
            return View();
        }

    }
}
