using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CalendarRepository : RepositoryBase
    {
        public int AddByEntity(Calendar data)
        {
            using (var db = this.GetDBContext())
            {
                db.Calendars.Add(data);
                return db.SaveChanges();
            }        
        }

        public List<Calendar> getListByUser(int usrID)
        {            
            using (var db = this.GetDBContext()) {
                var res = db.Calendars.Where(c => c.cldCreatedBy == usrID && c.cldActive == true);
                return res.ToList();
            }        
        }

        public int UpdateByEntity(Calendar data)
        {
            using (var db = this.GetDBContext())
            {
                var entity = db.Calendars.Single(c => c.cldID == data.cldID);
                entity.cldDate = data.cldDate;
                entity.cldTimestart = data.cldTimestart;
                entity.cldTimeend = data.cldTimeend;
                entity.cldMessage = data.cldMessage;
                entity.cldColor = data.cldColor;
                entity.cldModifiedOn = data.cldModifiedOn;
                entity.cldModifiedBy = data.cldModifiedBy;
                return db.SaveChanges();
            }
        }

        public int UpdateDateTime(Calendar data)
        {
            using (var db = this.GetDBContext()) {
                var entity = db.Calendars.Single(c => c.cldID == data.cldID);
                entity.cldDate = data.cldDate;
                entity.cldTimestart = data.cldTimestart;
                entity.cldTimeend = data.cldTimeend;
                entity.cldModifiedOn = data.cldModifiedOn;
                entity.cldModifiedBy = data.cldModifiedBy;
                return db.SaveChanges();
            }        
        }

        public int UpdateRemove(Calendar data) {
            using (var db = this.GetDBContext())
            {
                var en = db.Calendars.Single(c => c.cldID == data.cldID);
                en.cldActive = false;
                en.cldModifiedBy = data.cldModifiedBy;
                en.cldModifiedOn = data.cldModifiedOn;
                return db.SaveChanges();
            }        
        }

        public Calendar getByID(int id)
        {
            using (var db = this.GetDBContext()) {
                return db.Calendars.Single(c => c.cldID == id);
            }
        }

    }

    public class CalendarViewModel
    {
        public List<Calendar> list_item { get; set; }
    
    }
}