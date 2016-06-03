using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class AddressRepository : RepositoryBase
    {
        public int AddByEntity(Address data)
        {
            var db = this.GetDBContext();
            db.Address.Add(data);
            return db.SaveChanges();
        }

        public List<Address> getByContact(int id)
        {
            var db = this.GetDBContext();
            return db.Address.Where(a => a.ctaID == id && a.adrActive == true).OrderBy(a => a.adrCreatedOn).ToList();
        }

    }
}