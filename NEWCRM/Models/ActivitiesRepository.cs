using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ActivitiesRepository : RepositoryBase
    {
        public Activities AddByEntity(Activities data)
        {
            using (var db = this.GetDBContext())
            {
                db.Activities.Add(data);
                db.SaveChanges();
                return data;
            }
        }

        public Activities GetByID(int id)
        {
            using (var db = this.GetDBContext())
            {
                return db.Activities.SingleOrDefault(x => x.actID == id);
            }
        }

        public int EndCallUpdated(int id, int emo, int ctaID, int usrID, string chnID)
        {
            using (var db = this.GetDBContext())
            {
                var act = db.Activities.Single(a => a.actID == id);
                act.actEmo = emo;
                act.ctaID = ctaID;
                act.chnID = chnID;
                act.atsID = 1; // Status completed
                act.actStop = DateTime.Now;
                act.actModifiedOn = DateTime.Now;
                act.actModifiedBy = usrID;

                return db.SaveChanges();
            }
        }
    }
}