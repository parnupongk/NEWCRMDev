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
    
    public partial class SubDistrict_Locale
    {
        public int sdtID { get; set; }
        public string lclID { get; set; }
        public string sdtName { get; set; }
        public int dstID { get; set; }
        public int pvnID { get; set; }
        public System.DateTime sdtModifiedOn { get; set; }
        public int sdtModifiedBy { get; set; }
    
        public virtual Locale Locale { get; set; }
        public virtual SubDistrict SubDistrict { get; set; }
    }
}
