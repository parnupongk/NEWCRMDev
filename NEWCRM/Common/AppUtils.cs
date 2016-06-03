using NEWCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

namespace NEWCRM.Common
{
    public class AppUtils
    {
        public class Session
        {
            private static HttpSessionState CurrentSession
            {
                get
                {
                    return HttpContext.Current.Session;
                }
            }

            private static T GetSessionValue<T>(string name)
            {
                return (T)CurrentSession[name];
            }

            private static List<T> GetSessionList<T>(string name)
            {
                return (List<T>)CurrentSession[name];
            }

            private static string GetSessionValueString(string name)
            {
                return GetSessionValue<string>(name);
            }

            private static void SetSessionValue(string name, object value)
            {
                CurrentSession[name] = value;
            }

            public static void ClearSession()
            {
                CurrentSession.Clear();
            }

            public static List<CaseType> CaseTypes
            {
                get
                {
                    var obj = GetSessionList<CaseType>("CASETYPE");
                    if (obj == null)
                    {
                        try
                        {
                            var rep = new CasetypeRepository();
                            var result = rep.getAll();
                            AppUtils.Session.CaseTypes = result;
                            obj = result;
                        }
                        catch
                        {

                        }
                    }
                    return obj;
                }
                set
                {
                    SetSessionValue("CASETYPE", value);
                }
            }

            public static Activities Activity
            {
                get
                {
                    var obj = GetSessionValue<Activities>("ACTIVITY");
                    return obj;
                }
                set
                {
                    SetSessionValue("ACTIVITY", value);
                }
            
            }

            public static List<NewCaseModel> NewCase
            {
                get
                {
                    var obj = GetSessionList<NewCaseModel>("NEWCASE");
                    return obj;
                }
                set
                {
                    SetSessionValue("NEWCASE", value);
                }
            
            }

            public static User User
            {
                get
                {
                    var obj = GetSessionValue<User>("USER");                    
                    //Try to Reload
                    //if (obj == null)
                    //{
                    //    try
                    //    {
                    //        if (HttpContext.Current.Request.IsAuthenticated)
                    //        {
                    //            var rep = new UserRepository();
                    //            var result = rep.Search(new UserSearchParam() { OBJID = decimal.Parse(HttpContext.Current.User.Identity.Name) });

                    //            if (result.Count > 0)
                    //            {
                    //                obj = result[0];
                    //                AppUtils.Session.User = obj;
                    //            }

                    //        }
                    //    }
                    //    catch
                    //    {
                    //    }
                    //}

                    // Custom Session for Develop
                    //var obj = new Users()
                    //{
                    //    usrID = 1,
                    //    usrUsername = "Dev",
                    //    usrPassword = "Admin",
                    //    usrFirstName = "Dev",
                    //    usrLastName = "OTO",
                    //    usrDepartment = "Administrator",
                    //    usrAvatar = "1_10382881_896728570344293_5282455743426919356_n.jpg"
                    //};
                    return obj;
                }
                set
                {
                    SetSessionValue("USER", value);
                }
            }

            public class Userrolepermission
            {

                public static bool HasAction(string Name)
                {
                    var p = UserRole;
                    if (p == null)
                        return false;
                    return p.Exists(o => o.fncName == Name);
                }

                public static List<st_GET_UserRole_ById_Result> UserRole
                {

                    get
                    {
                        var obj = GetSessionValue<List<st_GET_UserRole_ById_Result>>("UserRole");
                        if (obj == null)
                        {
                            try
                            {
                                //obj = new UserRepository().GetUserRolePermission(int.Parse(HttpContext.Current.User.Identity.Name));
                                obj = new UserRepository().GetUserRolePermission(Session.User.usrID);
                                if (obj.Any())
                                    UserRole = obj;
                            }
                            catch { }
                        }
                        return obj;
                    }
                    set { SetSessionValue("UserRole", value); }

                }
            }
        }

        public class CaseLogType
        {
            public static int CreatedCase = 1;
            public static int CreatedContact = 2;
            public static int UpdatedContact = 3;
        }

        public class NotificationType
        {
            public static int AssignCase = 1;
            public static int RemindCase = 2;
        }

        public class Channels
        { 
            public static string Email = "em";
            public static string Facebook = "fb";
            public static string Fax = "fx";
            public static string Inbound = "ib";
            public static string Outbound = "ob";
            public static string Pantip = "pt";
            public static string SMS = "sm";
            public static string Twitter = "tw";
            public static string WebChat = "wc";
            public static string WalkIn = "wi";
        }

        public class AppConfig
        {
            private static string GetAppConfigString(string name)
            {
                return ConfigurationManager.AppSettings[name];
            }

            public static string SMTPSERVER
            {
                get
                {
                    return GetAppConfigString("SMTPSERVER");
                }
            }

            public static string SMTPUSER
            {
                get
                {
                    return GetAppConfigString("SMTPUSER");
                }
            }

            public static string SMTPPORT
            {
                get
                {
                    return GetAppConfigString("SMTPPORT");
                }
            }

            public static string SMTPPASS
            {
                get
                {
                    return GetAppConfigString("SMTPPASS");
                }
            }

            public static string SMTPMAILFROM
            {
                get
                {
                    return GetAppConfigString("SMTPMAILFROM");
                }
            }

            public static string CASE_MAIL_TO
            {
                get
                {
                    return GetAppConfigString("CASE_MAIL_TO");
                }
            }
            public static string CASE_MAIL_CC
            {
                get
                {
                    return GetAppConfigString("CASE_MAIL_CC");
                }
            }

            public static string FAX_OUT_HOST
            {
                get
                {
                    return GetAppConfigString("FAX_OUT_HOST");
                }
            }
            public static string FAX_OUT_USERNAME
            {
                get
                {
                    return GetAppConfigString("FAX_OUT_USERNAME");
                }
            }
            public static string FAX_OUT_PASSWORD
            {
                get
                {
                    return GetAppConfigString("FAX_OUT_PASSWORD");
                }
            }
            public static string FAX_OUT_EXT
            {
                get
                {
                    return GetAppConfigString("FAX_OUT_EXT");
                }
            }

            public static string STATION
            {
                get
                {
                    return GetAppConfigString("STATION");
                }
            }
            public static string PORTAL
            {
                get
                {
                    return GetAppConfigString("PORTAL");
                }
            }
        }

        public class Province
        {
            public static string getAll4SelectOption()
            {
                string str = string.Empty;

                var pvnList = new ProvinceRepository().getAll();
                foreach (var pvn in pvnList)
                {
                    str += string.Format("<option value='{0}'>{1}</option>/r/n", pvn.pvnID, pvn.pvnName);
                }

                return str;
            }
        }
    }
}