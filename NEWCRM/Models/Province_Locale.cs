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
    
    public partial class Province_Locale
    {
        public int pvnID { get; set; }
        public string lclID { get; set; }
        public Nullable<int> zneID { get; set; }
        public string pvnName { get; set; }
        public System.DateTime pvnModifiedOn { get; set; }
        public int pvnModifiedBy { get; set; }
    
        public virtual Locale Locale { get; set; }
        public virtual Province Province { get; set; }
    }
}
