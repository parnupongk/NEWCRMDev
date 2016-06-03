using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ContactViewLogRepository : RepositoryBase
    {
        public void AddByEntity(ContactViewLog data)
        {
            using (var db = this.GetDBContext())
            {
                db.ContactViewLogs.Add(data);
                db.SaveChanges();
            }                 
        }

        public List<sp_GetContactViewLog_Result> getLastView(int p_usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_UsrID", p_usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetContactViewLog_Result>("sp_GetContactViewLog", ps);
                return result;
            }
        }
    }
}