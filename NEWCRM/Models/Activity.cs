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
    
    public partial class Activity
    {
        public int actID { get; set; }
        public Nullable<int> ctaID { get; set; }
        public string chnID { get; set; }
        public int atsID { get; set; }
        public string actChannelData { get; set; }
        public string actIVRMenu { get; set; }
        public Nullable<int> actDuration { get; set; }
        public Nullable<System.DateTime> actStart { get; set; }
        public Nullable<System.DateTime> actStop { get; set; }
        public Nullable<int> actEmo { get; set; }
        public string conID { get; set; }
        public System.DateTime actCreatedOn { get; set; }
        public int actCreatedBy { get; set; }
        public System.DateTime actModifiedOn { get; set; }
        public int actModifiedBy { get; set; }
    }
}
