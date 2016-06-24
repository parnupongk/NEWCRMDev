using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
namespace NEWCRM.Models
{

    #region Aood
    public class TodolistRepository : RepositoryBase
    {
        public List<sp_GetCaseList_Result> getCaseList(Cri param)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("CaseId", param.CaseNo, ParamStyle.LikeContain),
                    this.GenObjectParam("StartCreateOn",param.StrCreateStartDate, ParamStyle.General),
                    this.GenObjectParam("EndCreateOn", param.StrCreateEndDate, ParamStyle.General),
                    this.GenObjectParam("StartModified", param.StrModifiedStartDate, ParamStyle.General),
                    this.GenObjectParam("EndModified", param.StrModifiedEndDate, ParamStyle.General),
                    this.GenObjectParam("StartClosed", param.StrClosedStartDate, ParamStyle.General),
                    this.GenObjectParam("EndClosed", param.StrClosedEndDate, ParamStyle.General),
                    this.GenObjectParam("Open", param.Open, ParamStyle.General),
                    this.GenObjectParam("Closed", param.Closed, ParamStyle.General),
                    this.GenObjectParam("Cancel", param.Cancel, ParamStyle.General),
                    this.GenObjectParam("Pending", param.Pending, ParamStyle.General),
                    this.GenObjectParam("USERID",param.OwenerId,ParamStyle.General),
                    this.GenObjectParam("GROUPID",param.GroupId,ParamStyle.General),
                    this.GenObjectParam("GROUPUSERID",param.GroupUserId,ParamStyle.General),
                    this.GenObjectParam("Channel",param.Channel,ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetCaseList_Result>("sp_GetCaseList",ps);
                return result;
            }
        }

        public List<todoListClass> getTotalByChannel(int id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), CommandType.StoredProcedure, "sp_getTotalTodolistByChannel",new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@usrID",id) });

            List<todoListClass> todo = new List<todoListClass>();
            todo = (from DataRow dr in ds.Tables[0].Rows
                       select new todoListClass()
                       {
                           myworkinbound = Convert.ToInt32(dr["myworkinbound"]),
                           myworkoutbound = Convert.ToInt32(dr["myworkoutbound"]),
                           myworkemail = Convert.ToInt32(dr["myworkemail"]),
                           mygroupinbound = Convert.ToInt32(dr["mygroupinbound"]),
                           mygroupoutbound = Convert.ToInt32(dr["mygroupoutbound"]),
                           mygroupemail = Convert.ToInt32(dr["mygroupemail"])
                       }).ToList();

            return todo;
            /*using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("usrID", id, ParamStyle.General)                    
                };

                var result = this.ExecStoredProcedure<sp_getTotalTodolistByChannel_Result>("sp_getTotalTodolistByChannel", ps);
                return result;
            }*/
        }
    }

    public class Cri
    {
        public bool IsClosed { get; set; }
        public bool IsOpen { get; set; }
        public bool IsPending { get; set; }
        public bool IsCancel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCreateDate { get; set; }
        public bool IsModifiedDate { get; set; }
        public bool IsClosedDate { get; set; }
        public string CaseNo { get; set; }

        public string StrCreateStartDate { get; set; }
        public string StrCreateEndDate { get; set; }
        public string StrModifiedStartDate { get; set; }
        public string StrModifiedEndDate { get; set; }
        public string StrClosedStartDate { get; set; }
        public string StrClosedEndDate { get; set; }
        public string strIsDate { get; set; }
        public string Open { get; set; }
        public string Closed { get; set; }
        public string Cancel { get; set; }
        public string Pending { get; set; }

        public string WorkType { get; set; }
        public string Pages { get; set; }

        public int? OwenerId { get; set; }
        public int? GroupId { get; set; }
        public int? GroupUserId { get; set; }

        public string Channel { get; set; }
    
    
    }

    public class todoListClass
    {
        public Nullable<int> myworkinbound { get; set; }
        public Nullable<int> myworkoutbound { get; set; }
        public Nullable<int> myworkemail { get; set; }
        public Nullable<int> mygroupinbound { get; set; }
        public Nullable<int> mygroupoutbound { get; set; }
        public Nullable<int> mygroupemail { get; set; }
    }

    #endregion
}