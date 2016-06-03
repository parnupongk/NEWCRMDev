using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverSLAJob
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            CasesOverSLA();
        }

        private static void CasesOverSLA()
        {
            TaskScheduleLog task = new TaskScheduleLog();
            task.TaskName = "CaseOverSLA";
            task.TaskStart = DateTime.Now;
            string runtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            try
            {
                var rep = new CaseRepository();
                var overSLAlist = rep.getCasesOverSLA(runtime);
                int iOverSLA = overSLAlist.Count;
                if (iOverSLA > 0)
                {
                    foreach (var cas in overSLAlist)
                    {
                        rep.changedToPending(cas.casID);

                        CaseLogs log = new CaseLogs();
                        log.actID = 0;
                        log.casID = cas.casID;
                        log.cltID = 10;
                        log.cslNote = string.Format("Duedate: {0}", cas.casDueDate);
                        log.cslDesc = string.Format("Over SLA to Pending");
                        log.cslCreatedBy = 0;
                        log.cslCreatedOn = DateTime.Now;
                        log.cslModifiedOn = DateTime.Now;
                        log.cslModifiedBy = 0;
                        AddCaseLog(log);
                    }
                }

                task.TaskStatus = "S";
                task.TaskDesc = string.Format("Over SLA: {0}", iOverSLA);
            }
            catch (Exception ex)
            {
                task.TaskStatus = "F";
                task.TaskDesc = ex.Message;
            }
            finally
            {
                task.TaskEnd = DateTime.Now;
                new TaskScheduleLogRepository().AddByEntity(task);
            }
        }

        private static void AddCaseLog(CaseLogs data)
        {
            new CaseLogsRepository().AddByEntity(data);
        }
    }
}
