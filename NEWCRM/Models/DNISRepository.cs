using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWCRM.Models
{
    class DNISRepository : RepositoryBase
    {
        
        public DNIS GetById(int dnis)  
        {
            var db = this.GetDBContext();
            return db.DNIS.SingleOrDefault(c => c.objID == dnis && c.Is_Active == 1);
        }

    }
}
