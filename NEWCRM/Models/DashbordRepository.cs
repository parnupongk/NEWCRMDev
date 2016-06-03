using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class DashbordRepository : RepositoryBase
    {
        #region Aood
        public st_Get_Dashbord_CaseStatus_Result Get_Dashbord_CaseStatus(string CreateDate)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("CREATEDATE", CreateDate, ParamStyle.General),

                };
                var result = this.ExecStoredProcedure<st_Get_Dashbord_CaseStatus_Result>("st_Get_Dashbord_CaseStatus", ps);
                if (result.Count > 0)
                    return result[0];
                return new st_Get_Dashbord_CaseStatus_Result();
            }
        }

        public sp_Get_Casepending_ByAgent_Result Get_Dashbord_CasePendingByAgent(string CreateDate, int UserId)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("CREATEDATE", CreateDate, ParamStyle.General),
                    this.GenObjectParam("USERID", UserId, ParamStyle.General),

                };
                var result = this.ExecStoredProcedure<sp_Get_Casepending_ByAgent_Result>("sp_Get_Casepending_ByAgent", ps);
                if (result.Count > 0)
                    return result[0];
                return new sp_Get_Casepending_ByAgent_Result();
            }
        }

        public sp_Get_Casepending_Result Get_Dashbord_CasePending(string _runtime)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_Now", _runtime, ParamStyle.General)                   
                };
                var result = this.ExecStoredProcedure<sp_Get_Casepending_Result>("sp_Get_Casepending", ps);
                if (result.Count > 0)
                    return result[0];
                return new sp_Get_Casepending_Result();
            }
        }

        #endregion


    }
}