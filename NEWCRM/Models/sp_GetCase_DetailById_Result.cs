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
    
    public partial class sp_GetCase_DetailById_Result
    {
        public int CASID { get; set; }
        public int CONTACTID { get; set; }
        public string CONTACTCODE { get; set; }
        public string CASEIDNO { get; set; }
        public Nullable<int> ctaID { get; set; }
        public string CTAFULLNAME { get; set; }
        public Nullable<int> CTAEMO1 { get; set; }
        public Nullable<int> CTAEMO2 { get; set; }
        public Nullable<int> CTAEMO3 { get; set; }
        public string PHONES { get; set; }
        public string CTAEMAIL { get; set; }
        public string CTAFACEBOOK { get; set; }
        public string CTATWITTER { get; set; }
        public string ADDRESS { get; set; }
        public string CASSUMMARY { get; set; }
        public string CASESTATUS { get; set; }
        public string TALKTIME { get; set; }
        public string CALLBACKTYPE { get; set; }
        public string CALLBACKNUMBER { get; set; }
        public string CREATEBY { get; set; }
        public System.DateTime CREATEDATE { get; set; }
        public string MODIFIEDBY { get; set; }
        public System.DateTime MODIFIEDDATE { get; set; }
        public Nullable<System.DateTime> CLOSEDATE { get; set; }
        public Nullable<System.DateTime> DUEDATE { get; set; }
        public string REMARK { get; set; }
        public bool casFav { get; set; }
        public Nullable<int> casOwnerID { get; set; }
        public int casCreatedBy { get; set; }
        public Nullable<int> assignTo { get; set; }
        public Nullable<int> dispatchTo { get; set; }
        public Nullable<System.DateTime> casDueDate { get; set; }
        public string OWNERNAME { get; set; }
        public string chnName { get; set; }
        public Nullable<int> casIDLevel1 { get; set; }
        public string cascommerceType { get; set; }
        public string casproductCategory { get; set; }
        public string casserviceCategory { get; set; }
        public string casdeliveryType { get; set; }
        public string casvalueRange { get; set; }
        public string casconversationChannel { get; set; }
        public string casreferenceDetail { get; set; }
        public string casdetail { get; set; }
        public string caspaymentType { get; set; }
        public Nullable<System.DateTime> caseventDate { get; set; }
        public string casVendorID { get; set; }
        public string casAttachFile { get; set; }
        public string casLevel3 { get; set; }
        public string casLevel6 { get; set; }
    }
}
