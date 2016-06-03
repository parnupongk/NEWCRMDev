using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWCRM.Common;
using NEWCRM.Models;

namespace NEWCRM.Controllers
{
    [LoginExpireAttribute]
    public class CasequeryController : ControllerBase
    {
        //
        // GET: /Casequery/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getQuery()
        {
            var rep = new CaseRepository();
            var model = new CaseViewModelLocalization();

            try
            {
                if (Request != null)
                {
                    List<string> types = new List<string>();
                    List<string> fields = new List<string>();
                    List<string> conditions_text = new List<string>();
                    List<string> conditions_date = new List<string>();
                    List<string> conditions_status = new List<string>();
                    List<string> values_text = new List<string>();
                    List<string> values_date = new List<string>();
                    List<string> values_status = new List<string>();
                    List<string> operators = new List<string>();

                    foreach (string key in Request.Form.AllKeys)
                    {

                        if (key.StartsWith("data-type-"))
                        {
                            types.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("field-"))
                        {
                            fields.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("condition-text-"))
                        {
                            conditions_text.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("condition-date-"))
                        {
                            conditions_date.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("condition-status-"))
                        {
                            conditions_status.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("value-text-"))
                        {
                            values_text.Add(Request.Form[key]);                        
                        }
                        else if (key.StartsWith("value-date-"))
                        {                           
                            values_date.Add(Request.Form[key]);                                                   
                        }
                        else if (key.StartsWith("value-status-"))
                        {
                            values_status.Add(Request.Form[key]);
                        }
                        else if (key.StartsWith("operator-"))
                        {
                            operators.Add(Request.Form[key]);
                        }
                    }  // end foreach

                    List<string> wheres = new List<string>();
                    string wh = string.Empty;
                    for (int i = 0; i < types.Count(); i++)
                    {

                        if (fields[i].ToString().Equals("casCreatedByName"))
                        {
                            wh = "CONCAT(crt.usrFirstName, ' ',crt.usrLastName) ";
                        }
                        else if (fields[i].ToString().Equals("casModifiedByName"))
                        {
                            wh = "CONCAT(mdf.usrFirstName, ' ',mdf.usrLastName) ";
                        }
                        else if (fields[i].ToString().Equals("casOwnerByName"))
                        {
                            wh = "CONCAT(Own.usrFirstName, ' ',Own.usrLastName) ";
                        }
                        else
                        {
                            wh = fields[i].ToString() + " ";
                        }

                        if (types[i].ToString().Equals("text"))
                        {
                            if (string.IsNullOrEmpty(values_text[i].Trim())) {
                                throw new Exception("Filter value incorrect");
                            }

                            if (conditions_text[i].StartsWith("like"))
                            {
                                wh += string.Format(conditions_text[i].Replace("$", "'"), values_text[i]);
                            }
                            else
                            {
                                wh += string.Format(conditions_text[i], "'" + values_text[i] + "'");
                            }
                        }
                        else if (types[i].ToString().Equals("status")) 
                        {
                            wh += string.Format(conditions_status[i], values_status[i]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(values_date[i].Trim()))
                            {
                                throw new Exception("Filter value incorrect");
                            }

                            wh = wh.Replace("casCreatedOn", "LEFT(CONVERT(VARCHAR, casCreatedOn, 120), 10) ").Replace("casModifiedOn", "LEFT(CONVERT(VARCHAR, casModifiedOn, 120), 10)");
                            wh += string.Format(conditions_date[i], " convert(datetime,'" + values_date[i] + "',103)");
                        }

                        if (i < types.Count() - 1)  // Ignore last operators
                        {
                            wh += operators[i];
                        }

                        wheres.Add(wh);
                    }   // end for         

                    string where = string.Join(" ", wheres.ToList());
                    model.list_query = rep.getQuery(where);

                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true; 
                return Content(ex.Message, "text/html");
            }
            
            return PartialView(model);
        }

    }
}
