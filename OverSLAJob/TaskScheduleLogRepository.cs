using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverSLAJob
{
    class TaskScheduleLogRepository : RepositoryBase
    {
        public void AddByEntity(TaskScheduleLog data)
        {
            using (var db = GetDBContext())
            {
                db.TaskScheduleLog.Add(data);
                db.SaveChanges();
            }
        }
    }
}
