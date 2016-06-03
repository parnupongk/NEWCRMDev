using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class EngineerSupportRepository : RepositoryBase
    {
        public int AddByEntity(EngineerSupport data)
        {
            using (var db = GetDBContext())
            {
                db.EngineerSupports.Add(data);
                return db.SaveChanges();
            }
        }

        public EngineerSupport getByID(int id)
        {
            using (var db = GetDBContext())
            {
                return db.EngineerSupports.SingleOrDefault(e => e.espID == id);
            }
        }

        public void UpdateEntity(EngineerSupport data)
        {
            using (var db = GetDBContext())
            {
                var new_data = db.EngineerSupports.SingleOrDefault(e => e.espID == data.espID);
                if (new_data != null)
                {
                    new_data.espStart = data.espStart;
                    new_data.espEnd = data.espEnd;
                    new_data.espPhone = data.espPhone;
                    new_data.espEmail = data.espEmail;
                    new_data.espModifiedOn = data.espModifiedOn;
                    new_data.espModifiedBy = data.espModifiedBy;
                    db.SaveChanges();
                }          
            }        
        }

        public List<EngineerSupport> getList()
        {
            using (var db = GetDBContext())
            {
                return db.EngineerSupports.Where(e => e.espActive == true).OrderByDescending(e => e.espStart).ToList();
            }
        }

        public void setRemove(int id)
        {
            using (var db = GetDBContext())
            {
                var data = db.EngineerSupports.Single(e => e.espID == id);
                data.espActive = false;
                db.SaveChanges();
            }
        }

        public EngineerSupport getByCurrentDateTime(DateTime _now)
        {
            using (var db = GetDBContext())
            {
                var data = db.EngineerSupports.Where(e => e.espActive == true && e.espStart <= _now && e.espEnd >= _now).FirstOrDefault();
                return data;
            }
        }
    }
}