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
    
    public partial class sp_GetNotification_Result
    {
        public int notID { get; set; }
        public int nttID { get; set; }
        public Nullable<int> refID { get; set; }
        public Nullable<int> Sender { get; set; }
        public Nullable<int> Receiver { get; set; }
        public string notNote { get; set; }
        public string notDesc { get; set; }
        public Nullable<bool> notRead { get; set; }
        public Nullable<bool> notActive { get; set; }
        public System.DateTime notCreatedOn { get; set; }
        public int notCreatedBy { get; set; }
        public System.DateTime notModifiedOn { get; set; }
        public int notModifiedBy { get; set; }
        public string nttName { get; set; }
        public Nullable<System.DateTime> casDueDate { get; set; }
    }
}
