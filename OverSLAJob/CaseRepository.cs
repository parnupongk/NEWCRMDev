using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverSLAJob;

namespace OverSLAJob
{
    class CaseRepository : RepositoryBase
    {
        public List<sp_getCasesOverSLA_Result> getCasesOverSLA(string _runtime)
        {
            using (var db = this.GetDBContext())
            {
                var ps = new[] {                    
                    this.GenObjectParam("p_Now",_runtime, ParamStyle.General),
                };

                var result = this.ExecStoredProcedure<sp_getCasesOverSLA_Result>("sp_getCasesOverSLA", ps);
                return result;
            }
        }

        public void changedToPending(int _casID)
        {
            using (var db = GetDBContext())
            {
                Cases cas = db.Cases.Single(c => c.casID == _casID);
                cas.cssID = 2;  // Pending
                db.SaveChanges();
            }
        }
    }
}
