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
    
    public partial class Gender
    {
        public Gender()
        {
            this.Genders_Locale = new HashSet<Genders_Locale>();
        }
    
        public int gndID { get; set; }
        public bool gndActive { get; set; }
        public System.DateTime gndModifiedOn { get; set; }
        public int gndModifiedBy { get; set; }
    
        public virtual ICollection<Genders_Locale> Genders_Locale { get; set; }
    }
}
