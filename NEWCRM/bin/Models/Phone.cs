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
    
    public partial class Phone
    {
        public int phnID { get; set; }
        public int ctaID { get; set; }
        public int phtID { get; set; }
        public string phnNumber { get; set; }
        public string phnExt { get; set; }
        public string phnNote { get; set; }
        public bool phnIsMain { get; set; }
        public bool phnActive { get; set; }
        public System.DateTime phnCreatedOn { get; set; }
        public int phnCreatedBy { get; set; }
        public System.DateTime phnModifiedOn { get; set; }
        public int phnModifiedBy { get; set; }
    }
}