using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class UserRepository : RepositoryBase
    {
        #region Aeh
        public sp_getUserProfile_Result getProfile(int p_usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_usrID", p_usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getUserProfile_Result>("sp_getUserProfile", ps);
                if (result.Count > 0) {
                    return result[0];
                }

                return null;
            }
        }

        public List<sp_getUserProfileTimeline_Result> getProfileTimeline(int p_usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_usrID", p_usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getUserProfileTimeline_Result>("sp_getUserProfileTimeline", ps);
                return result;
            }
        }

        public User getByID(int usrID)
        {
            using (var db = this.GetDBContext()) 
            {
                var usr = db.Users.Single(u => u.usrID == usrID);
                return usr;
            }        
        }

        public User getBySignin(string username, string password)
        {

                
                using (var db = this.GetDBContext())
                {
                    var usr = db.Users.FirstOrDefault(u => u.usrUsername == username && u.usrPassword == password);
                    return usr;
                }
 
        }

        public int changePassword(string newPassword, int usrID)
        {
            using (var db = this.GetDBContext())
            {
                var usr = db.Users.Single(u => u.usrID == usrID);
                usr.usrPassword = newPassword;
                return db.SaveChanges();
            }
        }

        public int updateAvatar(int usrID, string filename)
        {
            using (var db = this.GetDBContext())
            {
                var usr = db.Users.Single(u => u.usrID == usrID);
                usr.usrAvatar = filename;
                return db.SaveChanges();
            }        
        }
        #endregion

        #region Aood
        public List<st_GetUserInGroup_ById_Result> GetUserInGroupById(int UserId)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("USERID", UserId, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<st_GetUserInGroup_ById_Result>("st_GetUserInGroup_ById", ps);
                return result;
            }

        }

        public List<st_GET_UserRole_ById_Result> GetUserRolePermission(int UserId)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("USERID", UserId, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<st_GET_UserRole_ById_Result>("st_GET_UserRole_ById", ps);
                return result;
            }

        }
        #endregion
    }

    public class UserViewModel
    {
        public sp_getUserProfile_Result item_profile { get; set; }
        public List<sp_getUserProfileTimeline_Result> list_timeline { get; set; }
    }
}