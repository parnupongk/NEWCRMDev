using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class PhoneRepository : RepositoryBase
    {
        public int AddByEntity(Phone data)
        {
            var db = this.GetDBContext();
            db.Phones.Add(data);
            return db.SaveChanges();
        }       

        public List<Phone> getByContact(int id)
        {
            var db = this.GetDBContext();
            return db.Phones.Where(p => p.ctaID == id && p.phnActive == true).OrderByDescending(p => p.phnIsMain).ToList();
        }

        //public int UpdateByEntity(Phones data)
        //{
        //    var db = this.GetDBContext();
        //    db.Phones.up
        //}
    }
}