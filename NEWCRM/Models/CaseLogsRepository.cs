using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CaseLogsRepository : RepositoryBase
    {
        public int AddByEntity(CaseLog data)
        {
            var db = GetDBContext();
            db.CaseLogs.Add(data);
            return db.SaveChanges();
        }

    }
}