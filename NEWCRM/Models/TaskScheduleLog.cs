//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NEWCRM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskScheduleLog
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public Nullable<System.DateTime> TaskStart { get; set; }
        public Nullable<System.DateTime> TaskEnd { get; set; }
        public string TaskStatus { get; set; }
        public string TaskDesc { get; set; }
    }
}
