using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CaseRemindFlowRepository : RepositoryBase
    {
        public CaseRemindFlow getByID(int _id)
        {
            using (var db = GetDBContext())
            {
                return db.CaseRemindFlows.SingleOrDefault(cr => cr.crfID == _id);
            }
        }

        public List<CaseRemindFlow> getList()
        {
            using (var db = GetDBContext())
            {
                return db.CaseRemindFlows.OrderBy(cr => cr.crfID).ToList();
            }
        }
    }
}