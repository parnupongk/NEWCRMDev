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
    
    public partial class ContactLog
    {
        public int ctlID { get; set; }
        public int ctaID { get; set; }
        public Nullable<int> actID { get; set; }
        public Nullable<int> cltID { get; set; }
        public string ctlNote { get; set; }
        public System.DateTime ctlCreatedOn { get; set; }
        public int ctlCreatedBy { get; set; }
        public System.DateTime ctlModifiedOn { get; set; }
        public int ctlModifiedBy { get; set; }
    }
}
