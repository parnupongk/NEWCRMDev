using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Common;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class TodolistController : ControllerBase
    {
      #region Aood

        public ActionResult Index()
        {
            var model = new TodolistViewModel();
            model.list_total = new TodolistRepository().getTotalByChannel(AppUtils.Session.User.usrID);
            return View(model);
        }

        public ActionResult CaseList(string WorkType, string Channel, string Pages)
        {
            try
            {
                TodoListModels m = new TodoListModels();
                m.WorkType = WorkType;
                m.Pages = Pages;              
                m.Channel = Channel;
                return View("CaseList", m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public ActionResult CaseList()
        //{
        //    try
        //    {
        //        TodoListModels m = new TodoListModels();
        //        m.WorkType = Request["WorkType"];
        //        m.Pages = Request["Pages"];
        //        return View("CaseList", m);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public JsonResult DoloadCaseData(Cri m)
        {
            List<sp_GetCaseList_Result> d = new List<sp_GetCaseList_Result>();
            var startDate = Request["StartDate"];
            var endDate = Request["EndDate"];           

            if (m.IsCancel)
                m.Cancel = RepositoryBase.CaseStatus.Cancel;
            if (m.IsOpen)
                m.Open = RepositoryBase.CaseStatus.Open;
            if (m.IsPending)
                m.Pending = RepositoryBase.CaseStatus.Pending;
            if (m.IsClosed)
                m.Closed = RepositoryBase.CaseStatus.Closed;

            if (m.strIsDate == "Created" && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                m.StrCreateStartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                m.StrCreateEndDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            }
            if (m.strIsDate == "Modified" && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                m.StrModifiedStartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                m.StrModifiedEndDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            }
            //if (m.strIsDate == "Closed" && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            //{
            //    m.StrClosedStartDate = m.StartDate.Value.ToString("yyyyddMM");
            //    m.StrClosedEndDate = m.EndDate.Value.ToString("yyyyddMM");
            //}

            if (m.Pages == "Dasbord")
            {
                //m.StrCreateStartDate = DateTime.Now.ToString("yyyyMMdd");
                //m.StrCreateEndDate = DateTime.Now.ToString("yyyyMMdd");
                // m.StrCreateStartDate = "20151001";
                m.Open = RepositoryBase.CaseStatus.Open;
                m.Pending = RepositoryBase.CaseStatus.Pending;
                m.OwenerId = NEWCRM.Common.AppUtils.Session.User.usrID;
            }
            else
            {
                if (m.WorkType == "MyWork")
                {
                    m.Open = RepositoryBase.CaseStatus.Open;
                    m.Pending = RepositoryBase.CaseStatus.Pending;
                    m.OwenerId = NEWCRM.Common.AppUtils.Session.User.usrID;                  
                }
                if (m.WorkType == "MyGroup")
                {
                    m.Open = RepositoryBase.CaseStatus.Open;
                    m.Pending = RepositoryBase.CaseStatus.Pending;
                    m.GroupId = NEWCRM.Common.AppUtils.Session.User.grpID;                    
                    //m.GroupUserId = NEWCRM.Common.AppUtils.Session.User.usrID;
                }
            }

            d = new TodolistRepository().getCaseList(m);
            return Json(d, 0);
        }
        #endregion
    }

    public class TodolistViewModel
    {
        public List<sp_getTotalTodolistByChannel_Result> list_total { get; set; }
    }
}
