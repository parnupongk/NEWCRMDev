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
    
    public partial class District_Locale
    {
        public int dstID { get; set; }
        public string lclID { get; set; }
        public string dstName { get; set; }
        public System.DateTime dstModifiedOn { get; set; }
        public int dstModifiedBy { get; set; }
    
        public virtual District District { get; set; }
        public virtual Locale Locale { get; set; }
    }
}