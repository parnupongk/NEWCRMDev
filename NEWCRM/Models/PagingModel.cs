using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class PagingModel
    {
        public int RecordsPerPage { get; set; }
        public int TotalRecords { get; set; }        
    }
}