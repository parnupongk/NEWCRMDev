using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class MailLogRepository : RepositoryBase
    {
        public void AddByEntity(MailLog data)
        {
            using (var db = GetDBContext())
            {
                db.MailLogs.Add(data);
                db.SaveChanges();
            }
        }
    }
}