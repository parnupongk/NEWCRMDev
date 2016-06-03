using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverSLAJob
{
    class CaseLogsRepository : RepositoryBase
    {
        public int AddByEntity(CaseLogs data)
        {
            using (var db = GetDBContext())
            {
                db.CaseLogs.Add(data);
                return db.SaveChanges();
            }
        }
    }
}
