using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class NotificationRepository : RepositoryBase
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public int AddByEntity(Notification data)
        {
            using (var db = this.GetDBContext())
            {
                db.Notifications.Add(data);
                return db.SaveChanges();
            }
        }

        public IEnumerable<sp_GetNotification_Result> getByUser(int usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("usrID", usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetNotification_Result>("sp_GetNotification", ps);
                return result;
            }
        }

        public int getTotal(int usrID)
        {
            int Total = 0;
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(string.Format("sp_GetNotification_Total {0}", usrID), connection))
                {
                    //command.Notification = null;

                    //var dependency = new SqlDependency(command);
                    //dependency.OnChange += new OnChangeEventHandler(dependency_OngetTotal);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    Total = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return Total;
        }

        public void updateRead(int usrID)
        {
            using (var db = this.GetDBContext()) {
                var ntf = db.Notifications.Where(n => n.Receiver == usrID).ToList();
                ntf.ForEach(a => a.notRead = true);
                db.SaveChanges();
            }        
        }

        public void setDisabled(int notID)
        {
            using (var db = this.GetDBContext())
            {
                var ntf = db.Notifications.Single(n => n.notID == notID);
                ntf.notActive = false;
                db.SaveChanges();
            }
        }

        public List<Notification> getByRefID(int refID)
        {
            using (var db = this.GetDBContext())
            {
                return db.Notifications.Where(n => n.refID == refID).ToList();
            }
        }

        public List<Notification> getByReceivedCase(int casID, int usrID)
        {
            using (var db = this.GetDBContext())
            {
                return db.Notifications.Where(nt => nt.refID == casID && nt.Receiver == usrID && nt.notActive == true).ToList();
            }

        }

        public List<Notification> getBySenderCase(int casID, int usrID)
        {
            using (var db = this.GetDBContext())
            {
                return db.Notifications.Where(nt => nt.refID == casID && nt.Sender == usrID && nt.notActive == true).ToList();
            }

        }
    }
}