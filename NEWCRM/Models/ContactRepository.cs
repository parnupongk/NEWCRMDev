using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ContactRepository : RepositoryBase
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public int AddByEntity(Contact data)
        {
            var db = this.GetDBContext();
            db.Contacts.Add(data);
            return db.SaveChanges();                 
        }

        public Contact GetById(int id)
        {
            var db = this.GetDBContext();
            return db.Contacts.SingleOrDefault(c => c.ctaID == id);
        }

        public List<sp_GetInboundContact_Result> getInboundContact(string p_phone = null, string p_name = null)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_Phone", p_phone, ParamStyle.General),
                    this.GenObjectParam("p_Name", p_name, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetInboundContact_Result>("sp_GetInboundContact", ps);
                return result;
            }
        }

        public List<sp_ContactAutoComplete_Result> getContactAutoComplete(string p_Search, int p_Row)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_Search", p_Search, ParamStyle.General),
                    this.GenObjectParam("p_Row", p_Row, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_ContactAutoComplete_Result>("sp_ContactAutoComplete", ps);
                return result;
            }
        }

        public sp_getContactCommonDetail_Result getContactCommonDetail(int p_ctaID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_ctaID", p_ctaID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getContactCommonDetail_Result>("sp_getContactCommonDetail", ps);
                if (result.Count() > 0)
                {
                    return result[0];
                }
                return new sp_getContactCommonDetail_Result();
            }
        }

        public int updateEmoticon(int newEmoticon, int ctaID)
        {
            var db = this.GetDBContext();
            Contact cta = db.Contacts.Single(c => c.ctaID == ctaID);
            cta.ctaEmo1 = cta.ctaEmo2;
            cta.ctaEmo2 = cta.ctaEmo3;
            cta.ctaEmo3 = newEmoticon;
            return db.SaveChanges();
        }

        public int updateAvatar(int ctaID, string avatar)
        {
            var db = this.GetDBContext();
            Contact cta = db.Contacts.Single(c => c.ctaID == ctaID);
            cta.ctaAvatar = avatar;
            return db.SaveChanges();            
        }

        public int updateName(int ctaID, string firstname, string lastname)
        {
            var db = this.GetDBContext();
            Contact cta = db.Contacts.Single(c => c.ctaID == ctaID);
            cta.ctaFirstName = firstname;
            cta.ctaLastName = lastname;
            cta.ctaFullName = string.Format("{0} {1}", firstname, lastname);
            return db.SaveChanges();
        }

        public List<sp_getContactSearch_Result> getList(ContactSearchCriteria _search)
        {
            using (var db = this.GetDBContext())
            {

                var ps = new[] {                    
                    this.GenObjectParam("p_Name", _search.p_name, ParamStyle.General),
                    this.GenObjectParam("p_Cate", _search.p_cate, ParamStyle.General),
                    this.GenObjectParam("p_Order", _search.p_order, ParamStyle.General),
                    this.GenObjectParam("p_Top", _search.p_top, ParamStyle.General),
                    this.GenObjectParam("p_Created", _search.p_top, ParamStyle.General),
                };

                var result = this.ExecStoredProcedure<sp_getContactSearch_Result>("sp_getContactSearch", ps);
                return result;
            }
        }

        public List<sp_getContactLogTimeline_Result> getTimeline(int p_ctaID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_ctaID", p_ctaID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getContactLogTimeline_Result>("sp_getContactLogTimeline", ps);
                return result;
            }
        }

        // this method for SignalR
        //public List<sp_getContactLogTimeline_Result> getTimeline(int p_ctaID)
        //{
        //    var list = new List<sp_getContactLogTimeline_Result>();
        //    using (var connection = new SqlConnection(_connString))
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand(string.Format("sp_getContactLogTimeline {0}", p_ctaID), connection))
        //        {
        //            command.Notification = null;

        //            var dependency = new SqlDependency(command);
        //            dependency.OnChange += new OnChangeEventHandler(dependency_OnSubScriptTimeline);

        //            if (connection.State == ConnectionState.Closed)
        //                connection.Open();

        //            var reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                list.Add(item: new sp_getContactLogTimeline_Result {
        //                    casID = (reader["casID"] != DBNull.Value) ? (int)reader["casID"] : 0,
        //                    casIDName = (reader["casIDName"] != DBNull.Value) ? (string)reader["casIDName"] : string.Empty,
        //                    cssID = (reader["cssID"] != DBNull.Value) ? (int)reader["cssID"] : 0,
        //                    cbtID = (reader["cbtID"] != DBNull.Value) ? (int)reader["cbtID"] : 0,
        //                    Lognote = (reader["Lognote"] != DBNull.Value) ? (string)reader["Lognote"] : string.Empty,
        //                    cltName = (reader["cltName"] != DBNull.Value) ? (string)reader["cltName"] : string.Empty,
        //                    Created = Convert.ToDateTime(reader["Created"]),
        //                    Owner = (reader["Owner"] != DBNull.Value) ? (string)reader["Owner"] : string.Empty,
        //                    LogType = (reader["LogType"] != DBNull.Value) ? (string)reader["LogType"] : string.Empty,
        //                });
        //            }

        //        }
        //    }
        //    return list;
        //}


        // this method for SignalR
        //public IEnumerable<sp_ActivityChannelSummary_Result> getActivityChannel()
        //{
        //    var list = new List<sp_ActivityChannelSummary_Result>();
        //    using (var connection = new SqlConnection(_connString))
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand(@"sp_ActivityChannelSummary", connection))
        //        {
        //            command.Notification = null;

        //            var dependency = new SqlDependency(command);
        //            dependency.OnChange += new OnChangeEventHandler(dependency_OnSubScript);

        //            if (connection.State == ConnectionState.Closed)
        //                connection.Open();

        //            var reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                list.Add(item: new sp_ActivityChannelSummary_Result { chnName = (string)reader["chnName"], Total = (int)reader["Total"], chnIcon = (string)reader["chnIcon"] });
        //            }

        //        }
        //    }
        //    return list;
        //}

        public IEnumerable<sp_ActivityChannelSummary_Result> getActivityChannel()
        {
            using (var db = this.GetDBContext())
            {                
                var result = this.ExecStoredProcedure<sp_ActivityChannelSummary_Result>("sp_ActivityChannelSummary");
                return result;
            }           
        }

        public List<sp_getLastActivityContact_Result> getLastActivityContact(int usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_usrID", usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getLastActivityContact_Result>("sp_getLastActivityContact", ps);
                return result;
            }
        }                
    }

    public class ContactViewModel
    {
        public int ctaID { get; set; }
        public PagingModel paging { get; set; }
        public Contact contact { get; set; }
        public sp_getContactCommonDetail_Result commondetail { get; set; }
        public List<Phone> list_phones { get; set; }
        public List<Address> list_address { get; set; }
        public ContactSearchCriteria search { get; set; }
        public List<sp_getContactLogTimeline_Result> list_timeline { get; set; }
        public int sum_timeline_case { get; set; }
        public List<sp_GetContactViewLog_Result> list_lastview { get; set; }
        public List<sp_GetLastCases_Result> list_lastcases { get; set; }
    }

    public class ContactCommonData
    {
        public int ctaID { get; set; }
        public string datafield { get; set; }
        public string datatable { get; set; }
        public string dataid { get; set; }
        public string datavalue { get; set; }
        public string dataismain { get; set; }
    }

    public class ContactSearchCriteria
    {
        public string p_name { get; set; }
        public int p_cate { get; set; }
        public string p_order { get; set; }
        public int? page { get; set; }
        public int? p_top { get; set; }
        public int? p_created { get; set; }
    }

    public class ContactAddNewModel
    {
        public bool isSave { get; set; }

        // Contact Fields      
        public string ctaFirstName { get; set; }
        public string ctaLastName { get; set; }
        public int gndID { get; set; }
        public string ctaDOB { get; set; }
        public string ctaEmail { get; set; }
        public string ctaCareer { get; set; }
        // Main Phones Fields
        public string phnMNumber { get; set; }
        
        // Other Phones Fields
        public string phnNumber { get; set; }

        // Address Fields
        public string adrHomeNo { get; set; }
        public string adrMoo { get; set; }
        public string adrBuilding { get; set; }
        public string adrRoom { get; set; }
        public string adrMooban { get; set; }
        public string adrSoi { get; set; }
        public string adrRoad { get; set; }
        public string adrSubDistrict { get; set; }
        public string adrDistrict { get; set; }
        public string adrProvince { get; set; }
        public string adrZip { get; set; }    
    }
}