using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CasetypeRepository : RepositoryBase
    {
        public int AddByEntity(CaseType data)
        {
            var db = this.GetDBContext();
            db.CaseTypes.Add(data);
            return db.SaveChanges();
        }

        public List<CaseType> getAll()
        {
            var db = this.GetDBContext();
            return db.CaseTypes.ToList();        
        }
    }

    public class CasetypeViewModel
    {
        public List<CaseType> list_node { get; set; }
        public int? selectedID { get; set; }
    }

    public class SourcetypeViewModel
    {
        public  List<string> sourceType  { get; set; }        
    }
}