using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Models;

namespace NEWCRM.Controllers
{
    public class CaseRemindFlowController : ControllerBase
    {
        //
        // GET: /CaseRemindFlow/

        public ActionResult Index()
        {
            var list = new CaseRemindFlowRepository().getList();

            return View(list);
        }

        public ContentResult SaveAll(CaseRemindFlow data)
        {
            try
            {
                string xxx = "";

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                return Content(ex.Message, "text/html");
            }
        }

    }
}
