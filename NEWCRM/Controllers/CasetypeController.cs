using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class CasetypeController : Controller
    {

        #region bom
        public ActionResult GetCaseGroup1(CaseViewModelLocalization model)
        {
            return PartialView("CaseGroup1", model);
        }

        public ActionResult GetSourceType()
        {

            var model = new SourcetypeViewModel();
            model.sourceType = ConfigurationManager.AppSettings["CASE_SOURCE_TYPE"].ToString().Split('|').ToList();
            return PartialView("SourceTypeNode", model);
        }

        public ActionResult getCaseTypeNodeSelectionRepCase(int id, int? selectedID = null)
        {
            var list_casetype = Common.AppUtils.Session.CaseTypes;
            var nodes = from ct in list_casetype
                        where ct.catParrentID == id
                        orderby ct.catName
                        select ct;

            var model = new CasetypeViewModel();
            model.list_node = nodes.ToList();
            model.selectedID = selectedID;

            if (model.list_node.Count() > 0)
            {
                if (model.list_node[0].catParrentID > 0)
                {
                    ViewData["node_label"] = "ประเภทเรื่องรอง";
                }
                else
                {
                    ViewData["node_label"] = "ประเภทเรื่องหลัก";
                }

                ViewData["ct_level"] = model.list_node[0].catLevel;
                ViewData["node_id"] = string.Format("ctnode_{0}", id);
            }

            return PartialView("CaseTypeNodeSelectionRepCase", model);
        }

    
    #endregion

    //
    // GET: /Casetype/
    public ActionResult getCaseTypeNodeSelection(int id, int? selectedID = null)
        {
            var list_casetype = Common.AppUtils.Session.CaseTypes;
            var nodes = from ct in list_casetype
                       where ct.catParrentID == id
                       orderby ct.catName
                       select ct;

            var model = new CasetypeViewModel();
            model.list_node = nodes.ToList();
            model.selectedID = selectedID;

            if (model.list_node.Count() > 0)
            {
                if (model.list_node[0].catParrentID > 0)
                {
                    ViewData["node_label"] = "ประเภทเรื่องรอง";
                }
                else
                {
                    ViewData["node_label"] = "ประเภทเรื่องหลัก";
                }

                ViewData["ct_level"] = model.list_node[0].catLevel;
                ViewData["node_id"] = string.Format("ctnode_{0}", id);
            }

            return PartialView("CaseTypeNodeSelection", model);
        }       
        
    }
    
}
