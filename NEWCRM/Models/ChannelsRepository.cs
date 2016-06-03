using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class ChannelsRepository : RepositoryBase
    {
        public List<Channel> getAll()
        {
            var db = this.GetDBContext();
            return db.Channels.Where(c => c.chnActive == true).OrderBy(c => c.chnName).ToList();
        }
    }
}