using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ContactLogsRepository : RepositoryBase
    {
        public int AddByEntity(ContactLog data)
        {
            using (var db = this.GetDBContext()) {

                db.ContactLogs.Add(data);

                return db.SaveChanges();                                    
            }        
        }
    }
}