using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class AuthenLogRepository : RepositoryBase
    {
        public int AddByEntity(AuthenLog data)
        {
            using (var db = this.GetDBContext()) {
                db.AuthenLogs.Add(data);
                return db.SaveChanges();
            }
        }
    }
}