using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace NEWCRM.Models
{
    public class CaseRepository : RepositoryBase
    {

        #region bom
        public DataTable GetCaseSummaryReport(string startDate, string endDate,int catParrentID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), "uspReportCaseSummaryByDate"
                            , new SqlParameter[] {
                            new SqlParameter("@STARTDATE",startDate)
                            ,new SqlParameter("@ENDDATE",endDate)
                            ,new SqlParameter("@catParrentID",catParrentID)});
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetCaseReport_CALLPERDAY(string mount, string year)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IVRConnection"].ToString(), "SP_CALLPERDAY_ETDA"
                            , new SqlParameter[] { new SqlParameter("@smount", mount), new SqlParameter("@syear", year) });
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetCaseReport_EndCALL(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["EndCallConnection"].ToString(), "SP_ENDCALL_SURVEY"
                            , new SqlParameter[] {new SqlParameter("@startdate", startDate)
                            ,new SqlParameter("@enddate", endDate)
                            ,new SqlParameter("@HOTLINEID", ConfigurationManager.AppSettings["ENDCALL_SURVEY"])
                            });
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetCaseReport_CALLPERHOUR(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IVRConnection"].ToString(), "SP_CALLPERHOUR_ETDA"
                            , new SqlParameter[] {new SqlParameter("@startdate", startDate)
                            ,new SqlParameter("@enddate", endDate)
                            });
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetCaseReport(int? casIDLevel1, int? casIDLevel2, int? casIDLevel3, int? casIDLevel4, string startDate,string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), "uspReportCaseLevel"
                            , new SqlParameter[] {new SqlParameter("@casIDLevel1",casIDLevel1)
                            ,new SqlParameter("@casIDLevel2",casIDLevel2)
                            ,new SqlParameter("@casIDLevel3",casIDLevel3)
                            ,new SqlParameter("@casIDLevel4",casIDLevel4)
                            ,new SqlParameter("@STARTDATE",startDate)
                            ,new SqlParameter("@ENDDATE",endDate)});
                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Case> GetGroupURLAccount()
        {
            using (var db = this.GetDBContext())
            {
                var cas = db.Cases.Where(c => !c.casLevel6.Equals(null) && c.casParentID.Equals(null) );
                return cas.ToList();
            }
        }
        #endregion

        #region Aeh
        public int AddByEntity(Case data)
        {
            var db = this.GetDBContext();
            db.Cases.Add(data);
            return db.SaveChanges();
        }

        public void UpdatedCaseIDName(string casIDName, int id)
        {
            using (var db = this.GetDBContext())
            {
                var cas = db.Cases.Single(c => c.casID == id);
                cas.casIDName = casIDName;
                db.SaveChanges();
            }
        }

        public void updateFav(int id, bool isFav)
        {
            using (var db = this.GetDBContext())
            {
                var cas = db.Cases.Single(c => c.casID == id);
                cas.casFav = isFav;
                db.SaveChanges();
            }
        }

        public Case GetByCaseNo(string caseno)
        { 
            using (var db = this.GetDBContext()){
                var cas = db.Cases.Where(c => c.casIDName.Equals(caseno)).FirstOrDefault();
                return cas;            
            }
        }

        public Case GetByID(int casID)
        {
            using (var db = this.GetDBContext())
            {
                var cas = db.Cases.Single(c => c.casID == casID);
                return cas;
            }
        }

        public List<sp_GetLastCases_Result> GetLastCases(int p_ctaID, int p_top = 3)
        { 
            using (var db = this.GetDBContext()){

                var ps = new[] {                    
                    this.GenObjectParam("p_ctaID", p_ctaID, ParamStyle.General),
                    this.GenObjectParam("p_top", p_top, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetLastCases_Result>("sp_GetLastCases", ps);
                return result;
   
            }        
        }

        public List<sp_getCaseQuery_Result> getQuery(string p_query)
        {
            using (var db = this.GetDBContext())
            {

                var ps = new[] {                    
                    this.GenObjectParam("p_query", p_query, ParamStyle.General)                   
                };

                var result = this.ExecStoredProcedure<sp_getCaseQuery_Result>("sp_getCaseQuery", ps);
                return result;
            }
        }

        public List<sp_GetCaseDetailTimeline_Result> GetDetailTimeline(int casID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("casID", casID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetCaseDetailTimeline_Result>("sp_GetCaseDetailTimeline", ps);
                return result;

            }
        }

        public List<sp_GetCaseFavourite_Result> getFavourite(int usrID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("usrID", usrID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_GetCaseFavourite_Result>("sp_GetCaseFavourite", ps);
                return result;
            }
        }

        public List<sp_getNewCaseList_Result> getNewCaseList(int actID)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_actID", actID, ParamStyle.General)
                };

                var result = this.ExecStoredProcedure<sp_getNewCaseList_Result>("sp_getNewCaseList", ps);
                return result;
            }
        }       

        public int DetailUpdate(NewCaseModel _nc)
        {
            using (var db = GetDBContext())
            {
                var cas = db.Cases.Single(c => c.casID == _nc.casID);
                cas.casIDLevel1 = _nc.casIDLevel1;
                cas.casIDLevel2 = _nc.casIDLevel2;
                cas.casIDLevel3 = _nc.casIDLevel3;
                cas.casIDLevel4 = _nc.casIDLevel4;
                cas.casIDLevel5 = _nc.casIDLevel5;
                cas.casLevel1 = _nc.casLevel1;
                cas.casLevel2 = _nc.casLevel2;
                cas.casLevel3 = _nc.casLevel3;
                cas.casLevel4 = _nc.casLevel4;
                cas.casLevel5 = _nc.casLevel5;                
                cas.casIDSummary = Convert.ToInt32(_nc.casIDSummary);
                cas.casSummary = _nc.casSummary;
                cas.casNote = _nc.casNote;
                cas.casModifiedOn = Convert.ToDateTime(_nc.casModifiedOn);
                cas.casModifiedBy = Convert.ToInt32(_nc.casModifiedBy);
                //cas.casSLA = _nc.casSLA;
                //cas.casDueDate = _nc.casDueDate;

                return db.SaveChanges();
            }        
        }

        public void UpdatedRemindFlow(int id, int cafID, string status)
        {
            using (var db = this.GetDBContext())
            {
                var cas = db.Cases.Single(c => c.casID == id);
                cas.casRemindID = cafID;
                cas.casRemindStatus = status;
                db.SaveChanges();
            }
        }

        public void RemoveByID(int id)
        {
            using (var db = GetDBContext())
            {
                var data = db.Cases.SingleOrDefault(c => c.casID == id);
                if (data != null)
                {
                    db.Cases.Remove(data);
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Aood

        public sp_CaseDetailById_Result GetCaseDetailById(decimal Id, string Lang)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("CASEID", Id, ParamStyle.General),
                    this.GenObjectParam("LANG",Lang, ParamStyle.General),

                };

                var result = this.ExecStoredProcedure<sp_CaseDetailById_Result>("sp_CaseDetailById", ps);
                if (result.Count > 0)
                    return result[0];
                return new sp_CaseDetailById_Result();
            }
        }

        public List<sp_GetGroupUser_Result> GetGroupUsers()
        {
            using (var db = this.GetDBContext())
            {

                var result = this.ExecStoredProcedure<sp_GetGroupUser_Result>("sp_GetGroupUser");
                return result;
            }
        }

        public List<sp_GetStatus_Result> GetStatus(string Lang)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("Lang",Lang, ParamStyle.General),
                };

                var result = this.ExecStoredProcedure<sp_GetStatus_Result>("sp_GetStatus", ps);
                return result;
            }
        }

        public sp_GetMailContentInfo_Result GetMailContentInfo(int? CaseId)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("CASEID",CaseId, ParamStyle.General),
                };

                var result = this.ExecStoredProcedure<sp_GetMailContentInfo_Result>("sp_GetMailContentInfo", ps);
                if (result.Count > 0)
                    return result[0];
                return new sp_GetMailContentInfo_Result();
            }
        }

        public List<sp_GetUser_Result> GetStatus()
        {
            using (var db = this.GetDBContext())
            {
                var result = this.ExecStoredProcedure<sp_GetUser_Result>("sp_GetUser");
                return result;
            }
        }

        public string GetPendingCount()
        {
            using (var db = this.GetDBContext())
            {
                var count = (from p in db.Cases
                             where (new int[] { 2, 4 }).Contains(p.cssID)
                             select p).Count();
                return count.ToString();
            }


        }

        private void dependency_OnChange(object sender, System.Data.SqlClient.SqlNotificationEventArgs e)
        {
            if (e.Type == System.Data.SqlClient.SqlNotificationType.Change)
            {
                GetPendingCount();
            }
        }

        public bool DoSaveNote(int CaseId, string Remark, int CreateBy, int ModifiedBy)
        {
            new CaseLogsRepository().AddByEntity(new CaseLog
            {
                actID = 0,
                casID = CaseId,
                cslNote = Remark,
                cltID = EnumType.LogType.AddNote,
                cslCreatedBy = CreateBy,
                cslCreatedOn = DateTime.Now,
                cslModifiedOn = DateTime.Now,
                cslModifiedBy = ModifiedBy
            });
            return true;
        }

        public bool DoSaveAccept(int CaseId, int OwnerId, int GroupId)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();
                
                var usr = db.Users.Single(u => u.usrID == OwnerId);

                if (c != null)
                {
                    if (c.assignTo == OwnerId || c.casGroupID == GroupId)
                    {
                        c.assignTo = null;
                        c.casGroupID = null;
                        c.casOwnerID = OwnerId;
                        c.casModifiedBy = OwnerId;
                        c.casModifiedOn = DateTime.Now;
                        db.SaveChanges();

                        new CaseLogsRepository().AddByEntity(new CaseLog
                        {
                            actID = 0,
                            cltID = EnumType.LogType.AcceptCase,
                            casID = CaseId,
                            cslNote = string.Empty,
                            cslDesc = string.Format("by {0} {1}", usr.usrFirstName, usr.usrLastName),
                            cslCreatedBy = OwnerId,
                            cslCreatedOn = DateTime.Now,
                            cslModifiedOn = DateTime.Now,
                            cslModifiedBy = OwnerId
                        });
                    }
                }
                return true;
            }
        }

        public bool DoSaveReject(int CaseId, int OwnerId, int GroupId)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();

                var usr = db.Users.Single(u => u.usrID == OwnerId);

                if (c != null)
                {                    
                    c.assignTo = null;
                    c.casGroupID = null;
                    //c.casOwnerID = OwnerId;
                    c.casModifiedBy = OwnerId;
                    c.casModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        actID = 0,
                        cltID = EnumType.LogType.RejectCase,
                        casID = CaseId,
                        cslNote = string.Empty,
                        cslDesc = string.Format("by {0} {1}", usr.usrFirstName, usr.usrLastName),
                        cslCreatedBy = OwnerId,
                        cslCreatedOn = DateTime.Now,
                        cslModifiedOn = DateTime.Now,
                        cslModifiedBy = OwnerId
                    });                   
                }
                return true;
            }
        }

        public bool DoSaveStatus(int CaseId, int StatusId, string Remark, int cltId, int CreateBy, int ModifiedBy)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();

                var snew = db.CaseStatus_Locale.Single(cs => cs.cssID == StatusId);
                var sold = db.CaseStatus_Locale.Single(cs => cs.cssID == c.cssID);

                if (c != null)
                {
                    c.cssID = StatusId;
                    c.casModifiedBy = ModifiedBy;
                    c.casModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        actID = 0,
                        casID = CaseId,
                        cltID = EnumType.LogType.ChangeStatus,
                        cslNote = Remark,
                        cslDesc = string.Format("{0} to {1}", sold.cssName, snew.cssName),
                        cslCreatedBy = CreateBy,
                        cslCreatedOn = DateTime.Now,
                        cslModifiedOn = DateTime.Now,
                        cslModifiedBy = ModifiedBy
                    });
                }

            }
            return true;
        }

        public bool DoSaveClosed(int CaseId, string Status, string Remark, int cltId, int CreateBy, int ModifiedBy)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();
                if (c != null)
                {
                    c.cssID = 4;
                    c.casstatusReason = Status;
                    c.casModifiedBy = ModifiedBy;
                    c.casModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        actID = 0,
                        casID = CaseId,
                        cltID = EnumType.LogType.CloseCase,
                        cslNote = Remark,
                        cslCreatedBy = CreateBy,
                        cslCreatedOn = DateTime.Now,
                        cslModifiedOn = DateTime.Now,
                        cslModifiedBy = ModifiedBy
                    });
                }

            }
            return true;
        }

        public bool DoSaveDispatch(int CaseId, int GroupId, string Remark, int CreateBy, int ModifiedBy)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();

                var g = db.Groups.Single(gr => gr.grpID == GroupId);

                if (c != null)
                {
                    c.casGroupID = GroupId;
                    c.casModifiedBy = ModifiedBy;
                    c.casModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        actID = 0,
                        casID = CaseId,
                        cltID = EnumType.LogType.DispatchCase,
                        cslNote = Remark,
                        cslDesc = string.Format("to group {0}", g.grpName),
                        cslCreatedBy = CreateBy,
                        cslCreatedOn = DateTime.Now,
                        cslModifiedOn = DateTime.Now,
                        cslModifiedBy = ModifiedBy
                    });
                }

            }
            return true;


        }

        public bool DoSaveAssign(int CaseId, int UserId, string Remark, int CreateBy, int ModifiedBy)
        {
            using (var db = this.GetDBContext())
            {
                var c = (from ca in db.Cases
                         where ca.casID == CaseId
                         select ca).FirstOrDefault();

                var usr = db.Users.Single(u => u.usrID == UserId);

                if (c != null)
                {
                    // c.casGroupID = GroupId;
                    c.assignTo = UserId;
                    c.casModifiedBy = ModifiedBy;
                    c.casModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    new CaseLogsRepository().AddByEntity(new CaseLog
                    {
                        casID = CaseId,
                        actID = 0,
                        cltID = EnumType.LogType.AssignCase,
                        cslNote = Remark,
                        cslDesc = string.Format("to {0} {1}", usr.usrFirstName, usr.usrLastName),
                        cslCreatedBy = CreateBy,
                        cslCreatedOn = DateTime.Now,
                        cslModifiedOn = DateTime.Now,
                        cslModifiedBy = ModifiedBy
                    });
                }

            }
            return true;


        }

        #endregion
    }

    public class ENDCALL
    {
        public string createDate { get; set; }
        public decimal total_answer { get; set; }
        public decimal score_wrong { get; set; }
        public decimal not_answer { get; set; }
        public decimal score_1 { get; set; }
        public decimal per_score_1 { get; set; }
        public decimal score_2 { get; set; }
        public decimal per_score_2 { get; set; }
        public decimal score_3 { get; set; }
        public decimal per_score_3 { get; set; }
    }
    public class CALLPERHOUR
    {
        public string period { get; set; }
        public int entered { get; set; }
        public int transfer { get; set; }
        public int accepted_agent { get; set; }
        public int abandoned { get; set; }
        public decimal per_abandoned { get; set; }
        public string avg_engage_time { get; set; }
        public string engage_time { get; set; }
    }

    public class NewCaseModel
    {
        public int? casID { get; set; }
        public long? id { get; set; }
        public int? casParentID { get; set; }
        public int? casIDLevel1 { get; set; }
        public int? casIDLevel2 { get; set; }
        public int? casIDLevel3 { get; set; }
        public int? casIDLevel4 { get; set; }
        public int? casIDLevel5 { get; set; }
        public string casLevel1 { get; set; }
        public string casLevel2 { get; set; }
        public string casLevel3 { get; set; }
        public string casLevel4 { get; set; }
        public string casLevel5 { get; set; }
        public int? casIDSummary { get; set; }
        public string casSummary { get; set; }
        public string casURLAccount { get; set; }
        public string casNote { get; set; }
        public int? priID { get; set; }
        public int? cssID { get; set; }        
        public DateTime? casCreatedOn { get; set; }
        public DateTime? casDueDate { get; set; }
        public int? casCreatedBy { get; set; }
        public string casCreatedByName { get; set; }
        public int? casOwnerID { get; set; }
        public int? slaID { get; set; }
        public int? pvnID { get; set; }
        public string casFollowID { get; set; }
        public string casFollowDesc { get; set; }
        public DateTime? casModifiedOn { get; set; }
        public int? casModifiedBy { get; set; }
        // Call Back Fields
        public int? cbtID { get; set; }        
        public int? cbcID { get; set; }
        public string cbcName { get; set; }
        public string CallbackInfo { get; set; }
        public int? casSLA { get; set; }
        public bool casFav { get; set; }
        public string commerceType { get; set; }
        public string productCategory { get; set; }
        public string serviceCategory { get; set; }
        public string deliveryType { get; set; }
        public string deliveryTypeOther { get; set; }
        public string valueRange { get; set; }
        public string conversationChannel { get; set; }
        public string txtRefDetail { get; set; }
        public string txtDetail { get; set; }
        public string cssStatusReason { get; set; }
        public string paymentType { get; set; }
        public string paymentTypeOther { get; set; }
        public string txtVendorID { get; set; }
        public DateTime? eventDate { get; set; }
        public string casPoNo { get; set; }
        public string casPrice { get; set; }
    }

    public class CaseViewModelLocalization
    {
        public string new_case { get; set; }
        public string new_contact { get; set; }
        public List<Priority_Locale> list_priority { get; set; }
        public List<Channel> list_channels { get; set; }
        public List<sp_GetStatus_Result> list_status { get; set; }
        public List<sp_getCaseQuery_Result> list_query { get; set; }
        public List<sp_GetCaseDetailTimeline_Result> list_timeline { get; set; }
        public List<string> list_status_reason { get; set; }

        // group case 1
        public List<string> commerceType { get; set; }
        public List<string> productCategory { get; set; }
        public List<string> serviceCategory { get; set; }
        public List<string> deliveryType { get; set; }
        public List<string> paymentType { get; set; }
        public List<string> valueRange { get; set; }
        public List<string> conversationChannel { get; set; }
    }

    public class ListRepCaseModel
    {
        public DateTime currDate { get; set; }
        public List<RepCaseModel> list_repcase { get; set; }
        public List<RepCaseSummary> list_repcasesum { get; set; }
        public List<CALLPERHOUR> list_call { get; set; }
        public List<ENDCALL> list_endcall { get; set; }
    }

    public class RepCaseSummary
    {
        public string catID { get; set; }
        public string catName { get; set; }
        public string catParrentID { get; set; }
        public int counts { get; set; }
        public decimal Percents { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

        public class RepCaseModel
    {
        public string chnID { get; set; }
        public string cssName { get; set; }
        public string casCreatedByName { get; set; }
        public string casOwnerByName { get; set; }
        public string casIDName { get; set; }
        public int? casID { get; set; }
        public long? id { get; set; }
        public int? casIDLevel1 { get; set; }
        public int? casIDLevel2 { get; set; }
        public int? casIDLevel3 { get; set; }
        public int? casIDLevel4 { get; set; }
        public int? casIDLevel5 { get; set; }
        public string casLevel1 { get; set; }
        public string casLevel2 { get; set; }
        public string casLevel3 { get; set; }
        public string casLevel4 { get; set; }
        public string casLevel5 { get; set; }
        public string casLevel6 { get; set; }
        public int? casIDSummary { get; set; }
        public string casSummary { get; set; }
        public string casURLAccount { get; set; }
        public string casNote { get; set; }
        public int? priID { get; set; }
        public int? cssID { get; set; }
        public DateTime? casCreatedOn { get; set; }
        public DateTime? casDueDate { get; set; }
        public int? casCreatedBy { get; set; }
        public int? casOwnerID { get; set; }
        public int? slaID { get; set; }
        public int? pvnID { get; set; }
        public string casFollowID { get; set; }
        public string casFollowDesc { get; set; }
        public DateTime? casModifiedOn { get; set; }
        public int? casModifiedBy { get; set; }
        public string cascommerceType { get; set; }
        public string casproductCategory { get; set; }
        public string casserviceCategory { get; set; }
        public string casdeliveryType { get; set; }
        public string casvalueRange { get; set; }
        public string casconversationChannel { get; set; }
        public string casreferenceDetail { get; set; }
        public string casdetail { get; set; }
        public string casstatusReason { get; set; }
        public string ctaEmail { get; set; }
        public string ctaNumber { get; set; }
        public string caspaymentType { get; set; }
        public string casvendorID { get; set; }
        public string caseventDate { get; set; }
        public string casAttachFile { get; set; }
        public string ctaFullName { get; set; }
        public string chnName { get; set; }
        public string ctaCareer { get; set; }
    }
}