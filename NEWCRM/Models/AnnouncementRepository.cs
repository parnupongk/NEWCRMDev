using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class AnnouncementRepository : RepositoryBase
    {
        public int AddByEntity(Announcement data)
        {
            using (var db = this.GetDBContext())
            {
                db.Announcements.Add(data);
                return db.SaveChanges();
            }
        }

        public List<Announcement> getAll()
        {
            using (var db = this.GetDBContext())
            {
                var ls = db.Announcements.Where(a => a.annActive == true).ToList();
                return ls;
            }
        }
    }
}