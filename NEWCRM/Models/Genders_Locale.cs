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
    
    public partial class Genders_Locale
    {
        public int gndID { get; set; }
        public string lclID { get; set; }
        public string gndName { get; set; }
        public System.DateTime gndModifiedOn { get; set; }
        public int gndModifiedBy { get; set; }
    
        public virtual Gender Gender { get; set; }
        public virtual Locale Locale { get; set; }
    }
}
