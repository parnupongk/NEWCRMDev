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
    
    public partial class sp_GetUser_Result
    {
        public int usrID { get; set; }
        public Nullable<int> grpID { get; set; }
        public string usrUsername { get; set; }
        public string usrPassword { get; set; }
        public string usrFirstName { get; set; }
        public string usrLastName { get; set; }
        public string usrDepartment { get; set; }
        public string usrAvatar { get; set; }
        public bool usrIsActive { get; set; }
        public System.DateTime usrCreatedOn { get; set; }
        public int usrCreatedBy { get; set; }
        public System.DateTime usrModifiedOn { get; set; }
        public int usrModifiedBy { get; set; }
    }
}