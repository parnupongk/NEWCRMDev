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
    
    public partial class Announcement
    {
        public int annID { get; set; }
        public string annText { get; set; }
        public bool annActive { get; set; }
        public System.DateTime annCreatedOn { get; set; }
        public int annCreatedBy { get; set; }
        public Nullable<System.DateTime> annModifiedOn { get; set; }
        public Nullable<int> annModifiedBy { get; set; }
    }
}
