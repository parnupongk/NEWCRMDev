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
    
    public partial class Group
    {
        public Group()
        {
            this.Group_Role = new HashSet<Group_Role>();
            this.User_Group = new HashSet<User_Group>();
        }
    
        public int grpID { get; set; }
        public string grpName { get; set; }
        public bool grpActive { get; set; }
        public System.DateTime grpModifiedOn { get; set; }
        public int grpModifiedBy { get; set; }
    
        public virtual ICollection<Group_Role> Group_Role { get; set; }
        public virtual ICollection<User_Group> User_Group { get; set; }
    }
}
