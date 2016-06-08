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
        

        public void RepCaseSummaryExport(string startdate, string enddate, string catparentid)
        {
            catparentid = Session["parentId"] ==null ? "0" : Session["parentId"].ToString();
            startdate =  Session["sDate"] ==null ? "20/05/2016" : Session["sDate"].ToString();
            enddate = Session["eDate"] ==null ? DateTime.Now.ToString("dd/MM/yyyy"): Session["eDate"].ToString();
            DateTime startDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
            DataTable dt = new CaseRepository().GetCaseSummaryReport(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(catparentid));
            /*ListRepCaseModel rptListCase = new ListRepCaseModel();
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


            rptListCase.list_repcasesum = rptCase;*/

            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CaseSummaryReport_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Case Summary Report</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + " </td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Case Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Number of Case</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">%</th></tr></thead>");

            foreach( DataRow dr in dt.Rows )
            {
                Response.Write("<tr>");
                Response.Write("<td>" + dr["catName"].ToString() + "</td>");
                Response.Write("<td>" + dr["counts"].ToString() + "</td>");
                Response.Write("<td>" + dr["Percents"].ToString() + "</td>");
                Response.Write("</tr>");

                DataTable dtDetail = new CaseRepository().GetCaseSummaryReport(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), int.Parse(dr["catID"].ToString()));
                int row = 1;
                foreach(DataRow drDetail in dtDetail.Rows)
                {
                    Response.Write("<tr>");
                    Response.Write("<td style=\"padding - left:50px; \">" + row.ToString() +" " + drDetail["catName"].ToString() + "</td>");
                    Response.Write("<td>" + drDetail["counts"].ToString() + "</td>");
                    Response.Write("<td>" + drDetail["Percents"].ToString() + "</td>");
                    Response.Write("</tr>");
                    row++;
                }
            }

            Response.Write("</tbody></table></td></tr></table>");

            //return View("repSummary", rptListCase);
        }

        [HttpPost]
        public ActionResult getReport()
        {
            DateTime startDate = DateTime.ParseExact(Request.Form["startdate"], "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(Request.Form["enddate"], "dd/MM/yyyy", null);
            int caseIDLevel1 = 0;
            int caseIDLevel2 = 0;
            int caseIDLevel3 = 0;
            int caseIDLevel4 = 0;

            if (Request.Form["reptype"].ToString() == "1") { return RedirectToAction("rptCallHour", new{startDate = startDate,endDate = endDate });}
            else if (Request.Form["reptype"].ToString() == "2") { return RedirectToAction("rptCallDay", new { startDate = startDate });}
            else if (Request.Form["reptype"].ToString() == "3") { return RedirectToAction("rptCaseRep", new { startDate = startDate, endDate = endDate, caseIDLevel1 = caseIDLevel1, caseIDLevel2 = caseIDLevel2, caseIDLevel3 = caseIDLevel3, caseIDLevel4 = caseIDLevel4 }); }
            else if (Request.Form["reptype"].ToString() == "4") { return RedirectToAction("rptCaseOnl", new { startDate = startDate, endDate = endDate, caseIDLevel1 = 1, caseIDLevel2 = caseIDLevel2, caseIDLevel3 = caseIDLevel3, caseIDLevel4 = caseIDLevel4 }); }
            else if (Request.Form["reptype"].ToString() == "5") { return RedirectToAction("rptCaseRaw", new { startDate = startDate, endDate = endDate, caseIDLevel1 = 2, caseIDLevel2 = caseIDLevel2, caseIDLevel3 = caseIDLevel3, caseIDLevel4 = caseIDLevel4 }); }
            else if (Request.Form["reptype"].ToString() == "6") { return RedirectToAction("rptCaseCyb", new { startDate = startDate, endDate = endDate, caseIDLevel1 = 3, caseIDLevel2 = caseIDLevel2, caseIDLevel3 = caseIDLevel3, caseIDLevel4 = caseIDLevel4 }); }
            else if (Request.Form["reptype"].ToString() == "7") { return RedirectToAction("repSummary", new { startDate = Request.Form["startdate"], endDate = Request.Form["enddate"], catparentid = caseIDLevel1 }); }
            else { return RedirectToAction("getQuery", new { startDate = startDate, endDate = endDate, caseIDLevel1 = caseIDLevel1, caseIDLevel2 = caseIDLevel2, caseIDLevel3 = caseIDLevel3, caseIDLevel4 = caseIDLevel4 }); }
        }

        public ActionResult getQuery(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
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


            //rptListCase.list_repcase = rptCase;
            return PartialView(rptListCase);
        }

        public ActionResult rptCallHour(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new CaseRepository().GetCaseReport_CALLPERHOUR(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<CALLPERHOUR> call = new List<CALLPERHOUR>();

            Session["rowCount"] = dt.Rows.Count;

            call = (from DataRow dr in dt.Rows
               select new CALLPERHOUR()
               {
                   period = dr["period"].ToString(),
                   entered = Convert.ToInt32( dr["entered"].ToString()),
                   transfer = Convert.ToInt32(dr["transfer"].ToString()),
                   accepted_agent = Convert.ToInt32(dr["accepted_agent"].ToString()),
                   abandoned = Convert.ToInt32(dr["abandoned"].ToString()),
                   avg_engage_time = dr["avg_engage_time"].ToString(),
                   engage_time = dr["engage_time"].ToString()
               }).ToList();

            rptListCase.list_call = call;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            return PartialView(rptListCase);
        }
        public void excelCallHour(DateTime startDate, DateTime endDate)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CallPerformanceReportByHour_"+DateTime.Now.ToString("yyyyMMdd")+".xls");


            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Call Performance Report By Hour (Daily)</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + "</td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Incoming Call</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Transfer to Agent</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Answer Call</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Abandoned</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">% Abandoned</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Average talk time</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Total talk time</th></tr></thead><tbody><tbody>");
            


            DataTable dt = new CaseRepository().GetCaseReport_CALLPERHOUR(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<CALLPERHOUR> call = new List<CALLPERHOUR>();

            call = (from DataRow dr in dt.Rows
                    select new CALLPERHOUR()
                    {
                        period = dr["period"].ToString(),
                        entered = Convert.ToInt32(dr["entered"].ToString()),
                        transfer = Convert.ToInt32(dr["transfer"].ToString()),
                        accepted_agent = Convert.ToInt32(dr["accepted_agent"].ToString()),
                        abandoned = Convert.ToInt32(dr["abandoned"].ToString()),
                        avg_engage_time = dr["avg_engage_time"].ToString(),
                        engage_time = dr["engage_time"].ToString()
                    }).ToList();
            int irows = 0;
            foreach (var item in call)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td>" + irows + "</td>");
                Response.Write("<td>" + item.period + "</td>");
                Response.Write("<td>" + item.entered + "</td>");
                Response.Write("<td>" + item.transfer + "</td>");
                Response.Write("<td>" + item.accepted_agent + "</td>");
                Response.Write("<td>" + item.abandoned + "</td>");
                Response.Write("<td>" + item.avg_engage_time + "</td>");
                Response.Write("<td>" + item.engage_time + "</td>");
                Response.Write("</tr>");
            }

            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }

        public ActionResult rptCallDay(DateTime startDate)
        {

            //DateTime startDate = DateTime.ParseExact(Request.Form["startdate"], "dd/MM/yyyy", null);
            DataTable dt = new CaseRepository().GetCaseReport_CALLPERDAY(startDate.Month.ToString("00"), startDate.Year.ToString());
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<CALLPERHOUR> call = new List<CALLPERHOUR>();
            Session["rowCount"] = dt.Rows.Count;
            call = (from DataRow dr in dt.Rows
                    select new CALLPERHOUR()
                    {
                        period = dr["period"].ToString(),
                        entered = Convert.ToInt32(dr["entered"].ToString()),
                        transfer = Convert.ToInt32(dr["transfer"].ToString()),
                        accepted_agent = Convert.ToInt32(dr["accepted_agent"].ToString()),
                        abandoned = Convert.ToInt32(dr["abandoned"].ToString()),
                        avg_engage_time = dr["avg_engage_time"].ToString(),
                        engage_time = dr["engage_time"].ToString()
                    }).ToList();

            rptListCase.list_call = call;
            ViewBag.startDate = startDate;
            return PartialView(rptListCase);
        }
        public void excelCallDay(DateTime startDate)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CallPerformanceReportByDay_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
    
            //Response.Write("</tbody></table></td></tr></table>");

            DataTable dt = new CaseRepository().GetCaseReport_CALLPERDAY(startDate.Month.ToString("00"), startDate.Year.ToString());
            List<CALLPERHOUR> call = new List<CALLPERHOUR>();

            call = (from DataRow dr in dt.Rows
                    select new CALLPERHOUR()
                    {
                        period = dr["period"].ToString(),
                        entered = Convert.ToInt32(dr["entered"].ToString()),
                        transfer = Convert.ToInt32(dr["transfer"].ToString()),
                        accepted_agent = Convert.ToInt32(dr["accepted_agent"].ToString()),
                        abandoned = Convert.ToInt32(dr["abandoned"].ToString()),
                        avg_engage_time = dr["avg_engage_time"].ToString(),
                        engage_time = dr["engage_time"].ToString()
                    }).ToList();

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Call Performance Report By Day</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.Month.ToString("00") + " " + startDate.Year.ToString() + "</td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Incoming Call</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Transfer to Agent</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Answer Call</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Abandoned</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">% Abandoned</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Average talk time</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Total talk time</th></tr></thead><tbody>");

            int irows = 0;
            foreach (var item in call)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td>" + irows + "</td>");
                Response.Write("<td>" + item.period + "</td>");
                Response.Write("<td>" + item.entered + "</td>");
                Response.Write("<td>" + item.transfer + "</td>");
                Response.Write("<td>" + item.accepted_agent + "</td>");
                Response.Write("<td>" + item.abandoned + "</td>");
                Response.Write("<td>" + item.avg_engage_time + "</td>");
                Response.Write("<td>" + item.engage_time + "</td>");
                Response.Write("</tr>");
            }


            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }

        public ActionResult rptCaseRep(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            DataTable dt = new CaseRepository().GetCaseReport(999, 0, 0, 0, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casIDName = dr["casIDName"].ToString(),
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           chnID = dr["chnID"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casSummary = dr["casSummary"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           casstatusReason = dr["casstatusReason"].ToString(),
                           casOwnerByName = dr["casOwnerByName"].ToString()
                       }).ToList();
            rptListCase.list_repcase = rptCase;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.caseIDLevel1 = 999;
            ViewBag.caseIDLevel2 = caseIDLevel2;
            ViewBag.caseIDLevel3 = caseIDLevel3;
            ViewBag.caseIDLevel4 = caseIDLevel4;
            return PartialView(rptListCase);
        }

        public void excelCaseRep(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CaseDetailReport_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casIDName = dr["casIDName"].ToString(),
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           chnID = dr["chnID"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casSummary = dr["casSummary"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           casstatusReason = dr["casstatusReason"].ToString(),
                           casOwnerByName = dr["casOwnerByName"].ToString()
                       }).ToList();

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Case Detail Report</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + " </td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">No.</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">CaseID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">CreatedDate</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">ContactName</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">CaseType</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">PhoneNumber</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Detail</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Status</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">CreatedBy</th></tr></thead><tbody>");
            int irows = 0;
            foreach (var item in rptCase)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + irows + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casIDName + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.chnID + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedByName + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casSummary + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\"></td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casdetail + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casstatusReason + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casOwnerByName + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }

        public ActionResult rptCaseOnl(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           chnID = dr["chnID"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casLevel1 = dr["casLevel1"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           cascommerceType = dr["cascommerceType"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casproductCategory = dr["casproductCategory"].ToString(),
                           casserviceCategory = dr["casserviceCategory"].ToString(),
                           casdeliveryType = dr["casdeliveryType"].ToString(),
                           casvalueRange = dr["casvalueRange"].ToString(),
                           casconversationChannel = dr["casconversationChannel"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           casstatusReason = dr["casstatusReason"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();


            rptListCase.list_repcase = rptCase;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.caseIDLevel1 = caseIDLevel1;
            ViewBag.caseIDLevel2 = caseIDLevel2;
            ViewBag.caseIDLevel3 = caseIDLevel3;
            ViewBag.caseIDLevel4 = caseIDLevel4;
            return PartialView(rptListCase);
        }

        public void excelCaseOnl(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CaseDetailReportOnline_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           chnID = dr["chnID"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casLevel1 = dr["casLevel1"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           cascommerceType = dr["cascommerceType"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casproductCategory = dr["casproductCategory"].ToString(),
                           casserviceCategory = dr["casserviceCategory"].ToString(),
                           casdeliveryType = dr["casdeliveryType"].ToString(),
                           casvalueRange = dr["casvalueRange"].ToString(),
                           casconversationChannel = dr["casconversationChannel"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           casstatusReason = dr["casstatusReason"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">CASE DETAIL REPORT (ร้องเรียนซื้อ ขายออนไลน์)</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + " </td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">No.</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Case ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Created Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact Name</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Transaction Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Phone Number</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Main Case Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Sub Case Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Commerce Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Commerce Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Product Category</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Service Category</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Delivery Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Payment Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Value Range</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Conversation Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Vendor ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Detail</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Reference</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Status</th></tr></thead><tbody>");
            int irows = 0;
            foreach (var item in rptCase)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + irows + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casIDName + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.chnID + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedByName + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.ctaNumber + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casLevel1 + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casLevel2 + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.cascommerceType + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casLevel6 + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casproductCategory + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casserviceCategory + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casdeliveryType + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\"></td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casvalueRange + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casconversationChannel + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casLevel6 + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casdetail + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casreferenceDetail + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casstatusReason + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }

        public ActionResult rptCaseRaw(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           chnID = dr["chnID"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           cascommerceType = dr["cascommerceType"].ToString(),
                           casLevel3 = dr["casLevel3"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           ctaEmail = dr["ctaEmail"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();


            rptListCase.list_repcase = rptCase;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.caseIDLevel1 = caseIDLevel1;
            ViewBag.caseIDLevel2 = caseIDLevel2;
            ViewBag.caseIDLevel3 = caseIDLevel3;
            ViewBag.caseIDLevel4 = caseIDLevel4;
            return PartialView(rptListCase);
        }

        public void excelCaseLaw(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CaseDetailReportLaw_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           chnID = dr["chnID"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           cascommerceType = dr["cascommerceType"].ToString(),
                           casLevel3 = dr["casLevel3"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           ctaEmail = dr["ctaEmail"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Case Detail Report (ร้องเรียนกระทาผิด พรบ/ เว็บไซต์ผิดกฎหมาย)</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + " </td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">No.</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Created Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Event Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Case ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Case Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" colspan=\"4\">Contact Info</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" colspan=\"3\">Reference Content</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Detail</th></tr><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact Name</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact email</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Phone Number</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Source Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Reference ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Reference Detail</th></tr></thead><tbody>");
            int irows = 0;
            foreach (var item in rptCase)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + irows + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casIDName + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.chnID + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casLevel2 + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casCreatedByName + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.cascommerceType + "</td>");
                Response.Write("<td style=\"text-align:center;vertical-align:top;\">" + item.ctaEmail + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.ctaNumber + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casLevel3 + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casLevel6 + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casreferenceDetail + "</td>");
                Response.Write("<td style=\"text-align:left;vertical-align:top;\">" + item.casdetail + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }

        public ActionResult rptCaseCyb(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           chnID = dr["chnID"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           casLevel3 = dr["casLevel3"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           ctaEmail = dr["ctaEmail"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();


            rptListCase.list_repcase = rptCase;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.caseIDLevel1 = caseIDLevel1;
            ViewBag.caseIDLevel2 = caseIDLevel2;
            ViewBag.caseIDLevel3 = caseIDLevel3;
            ViewBag.caseIDLevel4 = caseIDLevel4;
            return PartialView(rptListCase);
        }

        public void excelCaseCyb(DateTime startDate, DateTime endDate, int caseIDLevel1, int caseIDLevel2, int caseIDLevel3, int caseIDLevel4)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("Content-Disposition", "attachment;filename=CaseDetailReportCyber_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

            DataTable dt = new CaseRepository().GetCaseReport(caseIDLevel1, caseIDLevel2, caseIDLevel3, caseIDLevel4, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ListRepCaseModel rptListCase = new ListRepCaseModel();
            List<RepCaseModel> rptCase = new List<RepCaseModel>();
            rptCase = (from DataRow dr in dt.Rows
                       select new RepCaseModel()
                       {
                           casCreatedOn = Convert.ToDateTime(dr["casCreatedOn"].ToString()),
                           casID = Convert.ToInt32(dr["casID"]),
                           casIDName = dr["casIDName"].ToString(),
                           chnID = dr["chnID"].ToString(),
                           casLevel2 = dr["casLevel2"].ToString(),
                           casCreatedByName = dr["casCreatedByName"].ToString(),
                           cascommerceType = dr["cascommerceType"].ToString(),
                           casLevel3 = dr["casLevel3"].ToString(),
                           casLevel6 = dr["casLevel6"].ToString(),
                           casreferenceDetail = dr["casreferenceDetail"].ToString(),
                           casdetail = dr["casdetail"].ToString(),
                           ctaEmail = dr["ctaEmail"].ToString(),
                           ctaNumber = dr["phnNumber"].ToString()
                       }).ToList();

            Response.Write("<table cellpadding=\"5\" width=\"100%\" align=\"center\"><tr><td align=\"left\" style=\"font-size:20pt;font-weight:bold;vertical-align:middle;height:50px;\">ETDA Call Center</td><td align=\"right\"><img src=\"http://www.hostwebdd.com/logo_in_excel.jpg\" /></td></tr><tr><td colspan=\"2\" align=\"left\" style=\"font-size:16pt;font-weight:bold;vertical-align:middle;height:40px;\">Case Detail Report (ร้องเรียนภัยคุกคาม CYBER)</td></tr><tr><td colspan=\"2\" align=\"left\" style=\"vertical-align:middle;height:30px;\">Report of " + startDate.ToString("dd MMM yyy") + " to " + endDate.ToString("dd MMM yyy") + " </td></tr><tr><td colspan=\"2\"><table border=\"1\" width=\"100%\" cellpadding=\"5\"><thead><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">No.</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Created Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Event Date</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Case ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Channel</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" colspan=\"4\">Contact Info</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Case Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" colspan=\"4\">Reference Content</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan=\"2\">Detail</th></tr><tr><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact Name</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Contact email</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Phone Number</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Source Type</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Reference ID</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Reference Detail</th><th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Attach file</th></tr></thead><tbody>");
            int irows = 0;
            foreach (var item in rptCase)
            {
                irows++;
                Response.Write("<tr>");
                Response.Write("<td>" + irows + "</td>");
                Response.Write("<td>" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td>" + item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                Response.Write("<td>" + item.casIDName + "</td>");
                Response.Write("<td>" + item.chnID + "</td>");
                Response.Write("<td>" + item.casLevel2 + "</td>");
                Response.Write("<td>" + item.casCreatedByName + "</td>");
                Response.Write("<td>" + item.cascommerceType + "</td>");
                Response.Write("<td>" + item.ctaEmail + "</td>");
                Response.Write("<td>" + item.ctaNumber + "</td>");
                Response.Write("<td>" + item.casLevel3 + "</td>");
                Response.Write("<td>" + item.casLevel6 + "</td>");
                Response.Write("<td>" + item.casreferenceDetail + "</td>");
                Response.Write("<td>" + item.casreferenceDetail + "</td>");
                Response.Write("<td>" + item.casdetail + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</tbody></table></td></tr></table>");
            Response.End();
        }
    }
}
