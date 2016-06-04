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
        public string sDate;
        public string eDate;
        public string parentId;
        public ActionResult Index()
        {
            
            //ListRepCaseModel model = new ListRepCaseModel();
            //model.currDate = DateTime.Today;

            //return View(model);
            return View();
        }

        public ActionResult repSummary(string startdate,string enddate,string catparentid)
        {
            Session["sDate"] = startdate;
            Session["eDate"] = enddate;
            Session["parentId"] = catparentid;

            DateTime startDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            DataTable dt = new CaseRepository().GetCaseSummaryReport(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(catparentid));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseSummary> rptCase = new List<RepCaseSummary>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseSummary()
                       {
                           catID = dr["catID"].ToString(),
                           catName = dr["catName"].ToString(),
                           catParrentID = dr["catParrentID"].ToString(),
                           counts = Convert.ToInt32(dr["counts"].ToString() == "" ?"0": dr["counts"].ToString()),
                           Percents = Convert.ToInt32(dr["Percents"].ToString()==""?"0": dr["Percents"].ToString()),
                           startDate = startdate,
                           endDate = enddate
                       }).ToList();


            rptListCase.list_repcasesum = rptCase;

            return View(rptListCase);
        }

        public ActionResult repSummaryChild(string startdate, string enddate, string catparentid)
        {
            DateTime startDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            DataTable dt = new CaseRepository().GetCaseSummaryReport(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(catparentid));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseSummary> rptCase = new List<RepCaseSummary>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseSummary()
                       {
                           catID = dr["catID"].ToString(),
                           catName = dr["catName"].ToString(),
                           catParrentID = dr["catParrentID"].ToString(),
                           counts = Convert.ToInt32(dr["counts"].ToString() == "" ? "0" : dr["counts"].ToString()),
                           Percents = Convert.ToInt32(dr["Percents"].ToString() == "" ? "0" : dr["Percents"].ToString()),
                           startDate = startdate,
                           endDate = enddate
                       }).ToList();


            rptListCase.list_repcasesum = rptCase;

            return View(rptListCase);
        }

        public ActionResult RepCaseSummaryExport(string startdate, string enddate, string catparentid)
        {
            catparentid = Session["parentId"] ==null ? "0" : Session["parentId"].ToString();
            startdate =  Session["sDate"] ==null ? "20/05/2016" : Session["sDate"].ToString();
            enddate = Session["eDate"] ==null ? DateTime.Now.ToString("dd/MM/yyyy"): Session["eDate"].ToString();
            DateTime startDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            DataTable dt = new CaseRepository().GetCaseSummaryReport(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(catparentid));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseSummary> rptCase = new List<RepCaseSummary>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseSummary()
                       {
                           catID = dr["catID"].ToString(),
                           catName = dr["catName"].ToString(),
                           catParrentID = dr["catParrentID"].ToString(),
                           counts = Convert.ToInt32(dr["counts"].ToString() == "" ? "0" : dr["counts"].ToString()),
                           Percents = Convert.ToInt32(dr["Percents"].ToString() == "" ? "0" : dr["Percents"].ToString()),
                           startDate = startdate,
                           endDate = enddate
                       }).ToList();


            rptListCase.list_repcasesum = rptCase;

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename = ExcelFile.xls");
            return View("repSummary", rptListCase);
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
