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
    
    public partial class Province
    {
        public Province()
        {
            this.Districts = new HashSet<District>();
            this.Province_Locale = new HashSet<Province_Locale>();
        }
    
        public int pvnID { get; set; }
        public bool pvnActive { get; set; }
        public System.DateTime pvnModifiedOn { get; set; }
        public int pvnModifiedBy { get; set; }
    
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Province_Locale> Province_Locale { get; set; }
    }
}