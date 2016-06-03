using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ProvinceRepository : RepositoryBase
    {
        public List<Province_Locale> getAll()
        {
            using (var db = GetDBContext())
            {
                var result = db.Province_Locale.OrderBy(p => p.pvnName);
                return result.ToList();
            }    
        }

        public Province_Locale getByID(int _id)
        {
            using (var db = GetDBContext())
            {
                return db.Province_Locale.Single(p => p.pvnID == _id);
            }
        }
    }
}