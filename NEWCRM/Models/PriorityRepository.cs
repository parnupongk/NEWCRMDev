using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class PriorityRepository : RepositoryBase
    {
        public List<Priority_Locale> getAll()
        {
            var db = this.GetDBContext();
            return db.Priority_Locale.OrderByDescending(p => p.priID).ToList();
        }
    }
}