using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public static class EnumType
    {
        #region Aood
        public static class LogType
        {
            public const int CreateCases = 1;
            public const int CreatedContact = 2;
            public const int UpdatedContact = 3;
            public const int AddNote = 8;
            public const int ChangeStatus = 10;
            public const int CloseCase = 11;
            public const int AcceptCase = 12;
            public const int DispatchCase = 13;
            public const int AssignCase = 14;
            public const int RejectCase = 15;
            public const int Feedback = 16;
            public const int Sendmail = 17;
            public const int RemindMail = 18;
            public const int EditCase = 19;
            public const int AttachFile = 20;
            public const int RemindCase = 21;
        }

        public static class ActionName {
            public const string ADD_NOTE = "ADD_NOTE";
            public const string CHANGE_STATUS = "CHANGE_STATUS";
            public const string CLOSED_CASE = "CLOSED_CASE";
            public const string DISPATCH = "DISPATCH";
            public const string ASSIGN = "ASSIGN";
            public const string ACCEPT = "ACCEPT";
            public const string REJECT = "REJECT";
            public const string SEND_EMAIL = "SEND_EMAIL";
        }

        #endregion

    }

  
}