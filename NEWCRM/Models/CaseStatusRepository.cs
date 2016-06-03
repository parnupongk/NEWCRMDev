using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CaseStatusRepository : RepositoryBase
    {
        public CaseStatus_Locale getEntityByID(int _id)
        {
            using (var db = GetDBContext())
            {
                return db.CaseStatus_Locale.SingleOrDefault(s => s.cssID == _id);
            }
        }
    }
}