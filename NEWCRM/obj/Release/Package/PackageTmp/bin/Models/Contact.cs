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
    
    public partial class Contact
    {
        public int ctaID { get; set; }
        public int ctyID { get; set; }
        public string ctaFullName { get; set; }
        public string ctaFirstName { get; set; }
        public string ctaLastName { get; set; }
        public string ctaAvatar { get; set; }
        public Nullable<int> ttlID { get; set; }
        public Nullable<System.DateTime> ctaDOB { get; set; }
        public Nullable<int> gndID { get; set; }
        public Nullable<int> marID { get; set; }
        public string ctaNote { get; set; }
        public Nullable<short> ctaRating { get; set; }
        public string ctaEmail { get; set; }
        public string ctaWebSite { get; set; }
        public string ctaFacebook { get; set; }
        public string ctaTwitter { get; set; }
        public bool ctaDoNotEmail { get; set; }
        public bool ctaDoNotPhone { get; set; }
        public bool ctaDoNotFax { get; set; }
        public bool ctaDoNotPostalMail { get; set; }
        public string ctaCareer { get; set; }
        public Nullable<int> ctaEmo1 { get; set; }
        public Nullable<int> ctaEmo2 { get; set; }
        public Nullable<int> ctaEmo3 { get; set; }
        public System.DateTime ctaCreatedOn { get; set; }
        public int ctaCreatedBy { get; set; }
        public System.DateTime ctaModifiedOn { get; set; }
        public int ctaModifiedBy { get; set; }
    }
}