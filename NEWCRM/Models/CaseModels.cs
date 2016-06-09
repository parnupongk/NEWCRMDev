using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CaseModels
    {
        #region CaseDetail Aood

            public sp_CaseDetailById_Result CaseDetails { get; set; }

            public List<sp_GetStatus_Result> CaseStatus { get; set; }     

            public List<sp_GetGroupUser_Result> UserGroups { get; set; }

            public List<sp_GetUser_Result> Users { get; set; }
        
            public MailContent MailContents { get; set; }

            public CaseRemindFlow RemindFlow { get; set; }
        #endregion
    }

    // Begin Aood
    public class MailContent {

        public int casID { get; set; }
        public string MailTo { get; set; }
        public string MailCc { get; set; }
        public string MailBcc { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string attachFiles { get; set; }
        public List<CaseAttachFile> MailAttachments { get; set; }        
    }    
    // End Aood


}