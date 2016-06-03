using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Models;
using System.Data;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class RepCaseController : Controller
    {
        //
        // GET: /RepCase/

        public ActionResult Index()
        {
            ListRepCaseModel model = new ListRepCaseModel();
            model.currDate = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        public ActionResult getQuery()
        {
            DateTime startDate = DateTime.ParseExact(Request.Form["startdate"], "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(Request.Form["enddate"], "dd/MM/yyyy", null);
            int caseIDLevel1 = Request.Form["casIDLevel1"] == null || Request.Form["casIDLevel1"].ToString().Replace(",", "") == "" ? 0 : int.Parse(Request.Form["casIDLevel1"].ToString().Replace(",", ""));
            int caseIDLevel2 = Request.Form["casIDLevel2"] == null || Request.Form["casIDLevel2"].ToString().Replace(",", "") == "" ? 0 : int.Parse(Request.Form["casIDLevel2"].ToString().Replace(",", ""));
            int caseIDLevel3 = Request.Form["casIDLevel3"] == null || Request.Form["casIDLevel3"].ToString().Replace(",", "") == "" ? 0 : int.Parse(Request.Form["casIDLevel3"].ToString().Replace(",", ""));
            int caseIDLevel4 = Request.Form["casIDLevel4"] == null || Request.Form["casIDLevel4"].ToString().Replace(",", "") == "" ? 0 : int.Parse(Request.Form["casIDLevel4"].ToString().Replace(",", ""));
            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            //DataTable dt = new CaseRepository().GetCaseReport("2016-05-20", "2016-05-31");
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casID = Convert.ToInt32(dr["casID"]),
                           chnID = dr["chnID"].ToString(),
                           casIDName = dr["casIDName"].ToString(),
                           casSummary = dr["casSummary"].ToString(),
                           cssName = dr["cssName"].ToString(),
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casOwnerByName = dr["casOwnerByName"].ToString()
                       }).ToList();


            rptListCase.list_repcase = rptCase;
            return PartialView(rptListCase);
        }
    }
}
