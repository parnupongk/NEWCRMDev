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
    
    public partial class District
    {
        public District()
        {
            this.District_Locale = new HashSet<District_Locale>();
            this.SubDistricts = new HashSet<SubDistrict>();
        }
    
        public int dstID { get; set; }
        public int pvnID { get; set; }
        public bool dstActive { get; set; }
        public System.DateTime dstModifiedOn { get; set; }
        public int dstModifiedBy { get; set; }
    
        public virtual ICollection<District_Locale> District_Locale { get; set; }
        public virtual ICollection<SubDistrict> SubDistricts { get; set; }
        public virtual Province Province { get; set; }
    }
}
