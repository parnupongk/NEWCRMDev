using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using NEWCRM.Models;

namespace NEWCRM.Common
{
    
    public class MailUtils
    {
        public static string _mailTo = string.Empty;
        public static string _mailCC = string.Empty;
        public static string _Subject = string.Empty;
        public static string _Body = string.Empty;

//        public static MailContent createdMail(int casID)
//        {
//            MailContent mail = new MailContent();
//            mail.MailSubject = string.Empty;
//            Cases cas = new CaseRepository().GetByID(casID);

//            // Check for case followup
//            if (cas.casIDLevel1 == 1 && cas.casIDLevel2 == 10)
//            {
//                var cf = new CasesFollowupRepository().getEntityByID(cas.casID);
//                if (cf != null)
//                {
//                    mail.MailSubject = "ติดตามเคส ";
//                    cas = new CaseRepository().GetByID(cf.casFollowupID);
//                }
//            }

//            return CreateTemplate(cas, mail);
//        }

//        private static MailContent CreateTemplate(Cases cas, MailContent mail)
//        {
//            if (cas != null)
//            {
//                if (cas.casIDLevel1 == 1)   // MEM
//                {
//                    if (cas.casIDLevel2 == 11)   // Complaint
//                    {
//                        mail.MailTo = MAIL_TO_MEM_COMPLAINT;
//                        mail.MailCc = MAIL_CC_MEM_COMPLAINT;
//                        mail.MailSubject += "Complain " + cas.casIDName;
//                        mail.MailBody = cas.casNote;
//                    }
//                    else if (cas.casIDLevel2 == 10) // Case Followup
//                    {

//                    }
//                    else if (cas.casIDLevel2 == 8 || cas.casIDLevel2 == 9 || cas.casIDLevel2 == 12)     // ยกเว้น Complaint
//                    {
//                        // If Case status=Opend changed to Dispatch
//                        //if (cas.cssID ==  4)
//                        //{
//                        //    new CaseRepository().DoSaveStatus(cas.casID, 5, string.Empty, 10, AppUtils.Session.User.usrID, AppUtils.Session.User.usrID);
//                        //    // Reload Case
//                        //    cas = null;
//                        //    cas = new CaseRepository().GetByID(casID);
//                        //}                        

//                        string contactName = string.Empty;
//                        string contactPhone = string.Empty;
//                        var cus = new ContactRepository().GetById(cas.ctaID);
//                        if (cus != null)
//                        {
//                            contactName = cus.ctaFullName;

//                            var phones = new PhoneRepository().getByContact(cus.ctaID);
//                            if (phones != null && phones.Count > 0)
//                            {
//                                contactPhone = phones[0].phnNumber;
//                            }
//                        }

//                        DataTable dt = new CaseFormUIRepository().getFormDatas("MEM_CaseFormData", cas.casID);
//                        if (dt.Rows.Count > 0)
//                        {
//                            DataRow dr = dt.Rows[0];

//                            if (dr["memProvince"] != DBNull.Value)
//                            {
//                                if (Convert.ToInt32(dr["memProvince"]) > 0)
//                                {
//                                    Province_Locale pv = new ProvinceRepository().getByID(Convert.ToInt32(dr["memProvince"]));
//                                    // Get Mail from Service Area
//                                    ServiceArea sa = new ServiceAreaRepository().getByState(pv.pvnName);
//                                    if (sa != null)
//                                    {
//                                        mail.MailTo = sa.MailTo;
//                                        mail.MailCc = sa.MailCC;
//                                    }
//                                }
//                            }

//                            string EquipName = string.Empty;
//                            string SerialNo = string.Empty;
//                            string CustomerCode = string.Empty;
//                            string CustomerName = "N/A";
//                            string DMSAddress = "N/A";
//                            string DMSPhone = "N/A";

//                            if (dr["memEquipmentNo"] != DBNull.Value)
//                            {
//                                SerialNo = dr["memEquipmentNo"].ToString();
//                                var asset = new AssetRepository().getEntityBySerialNo(SerialNo);
//                                if (asset != null)
//                                {
//                                    EquipName = asset.ModelMake;
//                                    CustomerCode = asset.CustomerCode;
//                                    CustomerName = asset.CustomerName;
//                                    DMSAddress = string.Format("{0} {1} {2} {3} {4} {5}", asset.Street, asset.Area, asset.City, asset.District, asset.State, asset.PinCode);
//                                }
//                            }

//                            string CaseStatus = string.Empty;
//                            if (cas.cssID == 4)
//                            {
//                                CaseStatus = "Dispatch";
//                            }
//                            else
//                            {
//                                var css = new CaseStatusRepository().getEntityByID(cas.cssID);
//                                if (css != null)
//                                {
//                                    CaseStatus = css.cssName;
//                                }
//                            }

//                            DateTime StartTime = cas.casCreatedOn;
//                            if (dr["memAppointDate"] != DBNull.Value)
//                            {
//                                if (dr["memAppointTime"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(dr["memAppointTime"])))
//                                {
//                                    StartTime = Convert.ToDateTime(dr["memAppointDate"] + " " + dr["memAppointTime"]);
//                                }
//                                else
//                                {
//                                    StartTime = Convert.ToDateTime(dr["memAppointDate"] + " 08:30");
//                                }
//                            }
//                            string phoneNo = string.Empty;
//                            if (dr["memPhoneNo"] != DBNull.Value)
//                            {
//                                phoneNo = dr["memPhoneNo"].ToString();
//                            }

//                            string ShopName = string.Empty;
//                            if (dr["memShopName"] != DBNull.Value)
//                            {
//                                ShopName = dr["memShopName"].ToString();
//                            }

//                            string memAddress = string.Empty;
//                            if (dr["memAddress"] != DBNull.Value)
//                            {
//                                memAddress = dr["memAddress"].ToString();
//                            }

//                            string memWorkTime = string.Empty;
//                            if (dr["memWorkTime"] != DBNull.Value)
//                            {
//                                memWorkTime = dr["memWorkTime"].ToString();
//                            }

//                            string memModel = string.Empty;
//                            if (dr["memModel"] != DBNull.Value)
//                            {
//                                memModel = dr["memModel"].ToString();
//                            }

//                            string memRemark = string.Empty;
//                            if (dr["memRemark"] != DBNull.Value)
//                            {
//                                memRemark = dr["memRemark"].ToString();
//                            }

//                            mail.MailSubject += string.Format("{0} {1} {2} {3}", cas.casIDName, EquipName, ShopName, CaseStatus);

//                            string bodyMEM = string.Format(@"<table border='1' style='width:60%'> 
//                                                                <tr>
//                                                                    <td colspan='2' style='background-color:pink'><b>ส่วนที่ 1(Agent บันทึก)</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue; width:30%'>Job ID:</td>
//                                                                    <td><b>{0}</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>Status:</td>
//                                                                    <td><b>{1}</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>Call Date/Time:</td>
//                                                                    <td>{2}</td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>Start date/Time:</td>
//                                                                    <td>{3}</td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>เข้าบริการภายในวันที่:</td>
//                                                                    <td style='color:red'>{4}</td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>ชื่อผู้แจ้ง/Tel:</td>
//                                                                    <td>{5}/{6}</td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>ชื่อร้าน/Tel:</td>
//                                                                    <td><b>{7}/{14}</b></td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>จุดสังเกตุ/การเดินทาง:</td>
//                                                                    <td>{8}</td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>เวลาเปิด-ปิดร้าน:</td>
//                                                                    <td>{9}</td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>ประเภทเครื่อง:</td>
//                                                                    <td><b>{10}</b></td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>Model:</td>
//                                                                    <td>{11}</td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>Equipment:</td>
//                                                                    <td><b>{12}</b></td>
//                                                                </tr>
//                                                                <tr>    
//                                                                    <td style='color:blue'>อาการเสีย:</td>
//                                                                    <td style='color:red'>{13}</td>
//                                                                </tr>
//                                                            </table>", cas.casIDName, CaseStatus, cas.casCreatedOn, StartTime, cas.casDueDate, contactName, contactPhone, ShopName, memAddress, memWorkTime, EquipName, memModel, SerialNo, memRemark, phoneNo);

//                            string bodyDMS = string.Format(@"<table border='1' style='width:60%'> 
//                                                                <tr>
//                                                                    <td colspan='2' style='background-color:yellow'><b>ส่วนที่ 2 DMS</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue; width: 30%'>รหัสร้านค้า:</td>
//                                                                    <td><b>{0}</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>ชื่อร้านค้าในระบบ:</td>
//                                                                    <td><b>{1}</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>ที่อยู่ในระบบ:</td>
//                                                                    <td><b>{2}</b></td>
//                                                                </tr>
//                                                                <tr>
//                                                                    <td style='color:blue'>เบอร์โทรในระบบ:</td>
//                                                                    <td><b>{3}</b></td>
//                                                                </tr>
//                                                            </table>", CustomerCode, CustomerName, DMSAddress, DMSPhone);

//                            //string bodyMEM = string.Format("Job ID: {0}<br/>Status: {1}<br/>Call Date/Time: {2}<br/>Start date/Time: {3}<br/>เข้าบริการภายในวันที่: {4}<br/>ชื่อผู้แจ้ง/Tel: {5}/{6}<br/>ชื่อร้าน: {7}<br/>จุดสังเกตุ/การเดินทาง: {8}<br/>เวลาเปิด-ปิดร้าน: {9}<br/>ประเภทเครื่อง: {10}<br/>Model: {11}<br/>Equipment: {12}<br/>อาการเสีย: {13}<br/>", cas.casIDName, CaseStatus, cas.casCreatedOn, StartTime, cas.casDueDate, cusName, phoneNo, ShopName, memAddress, memWorkTime, EquipName, memModel, SerialNo, memRemark);
//                            //string bodyDMS = string.Format("รหัสร้านค้า: {0}<br/>ชื่อร้านค้าในระบบ: {1}<br/>ที่อยู่ในระบบ: {2}<br/>เบอร์โทรในระบบ: {3}<br/>", CustomerCode, CustomerName, DMSAddress, DMSPhone);

//                            mail.MailBody = string.Format("{0}<br/><br/>{1}", bodyMEM, bodyDMS);
//                        }
//                        //mail.MailSubject = "เคสใหม่ " + cas.casIDName + " " + cusName;
//                        //mail.MailBody = string.Format("ชื่อร้านค้า: {0}<br/>ที่อยู่: {1}<br/>Cooler number: {2}<br/>", dr["memShopName"], dr["memAddress"], dr["memEquipmentNo"]);
//                    }

//                }
//                else if (cas.casIDLevel1 == 2)  // Sales-Beverage
//                {
//                    mail.MailTo = string.Empty;
//                    mail.MailCc = string.Empty;
//                    DataTable dt = new CaseFormUIRepository().getFormDatas("SalesBev_CaseFormData", cas.casID);
//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        DataRow dr = dt.Rows[0];
//                        string province = dr["sbProvince"].ToString();
//                        string district = dr["sbDistrict"].ToString();
//                        SalesBevServiceArea sb = null;
//                        if (!string.IsNullOrEmpty(province) && !string.IsNullOrEmpty(district))
//                        {
//                            sb = new SalesBevServiceAreaRepository().getEntity(province, district);
//                        }
//                        else
//                        {
//                            sb = new SalesBevServiceAreaRepository().getEntity(province);
//                        }

//                        if (sb != null)
//                        {
//                            mail.MailTo = sb.MailTO;
//                            mail.MailCc = sb.MailCC;
//                        }

//                    }
//                    mail.MailSubject += string.Format("{0} {1}", cas.casLevel3, cas.casIDName);
//                    var mailContent = new CaseRepository().GetMailContentInfo(cas.casID);
//                    mail.MailBody = string.Format("ลูกค้า: {0}<br/>ช่องทางการติดต่อ: {1}<br/>ชื่อร้านค้า: {2}<br/>โทรศัพท์: {3}<br/>ที่อยู่: {4}<br/>ประเภทร้าน: {5}<br/>เรื่องที่แจ้ง: {6}<br/>", mailContent.CTAFULLNAME, string.Empty, string.Empty, mailContent.PHNNUMBER, mailContent.adrSummary, string.Empty, cas.casNote);
//                }
//                else if (cas.casIDLevel1 == 3)  // Sales- Snack
//                {
//                    if (cas.casIDLevel3 == 109)  // Modern Trade
//                    {
//                        mail.MailTo = MAIL_TO_SALESSNACK_MODERNTRADE;
//                        mail.MailCc = MAIL_CC_SALESSNACK_MODERNTRADE;
//                    }
//                    else
//                    {
//                        mail.MailTo = MAIL_TO_SALESSNACK_NOT_MODERNTRADE;
//                        mail.MailCc = MAIL_CC_SALESSNACK_NOT_MODERNTRADE;
//                    }

//                    mail.MailSubject += string.Format("{0} {1}", cas.casLevel3, cas.casIDName);
//                    var mailContent = new CaseRepository().GetMailContentInfo(cas.casID);
//                    mail.MailBody = string.Format("ลูกค้า: {0}<br/>ชื่อร้านค้า: {1}<br/>โทรศัพท์: {2}<br/>ที่อยู่: {3}<br/>ประเภทร้าน: {4}<br/>เรื่องที่แจ้ง: {5}", mailContent.CTAFULLNAME, string.Empty, mailContent.PHNNUMBER, mailContent.adrSummary, string.Empty, cas.casNote);
//                }
//                else if (cas.casIDLevel1 == 4 || cas.casIDLevel1 == 5 || cas.casIDLevel1 == 6)   // CA
//                {
//                    if (cas.casIDLevel2 == 17 || cas.casIDLevel2 == 19 || cas.casIDLevel2 == 21)    // General inquiry
//                    {
//                        mail.MailTo = MAIL_TO_CA_GENERAL_INQUIRY;
//                        mail.MailCc = MAIL_CC_CA_GENERAL_INQUIRY;
//                        string subcate = string.Empty;
//                        if (cas.casIDLevel1 == 4)
//                        {
//                            subcate = "Bev";
//                        }
//                        if (cas.casIDLevel1 == 5)
//                        {
//                            subcate = "Food";
//                        }
//                        if (cas.casIDLevel1 == 6)
//                        {
//                            subcate = "Import";
//                        }

//                        mail.MailSubject += "Inquiry " + subcate + " " + cas.casIDName;

//                        string cusName = string.Empty;
//                        var cus = new ContactRepository().GetById(cas.ctaID);
//                        if (cus != null)
//                        {
//                            cusName = cus.ctaFullName;
//                        }
//                        var mailContent = new CaseRepository().GetMailContentInfo(cas.casID);
//                        mail.MailBody = string.Format("ชื่อลูกค้า : {0}<br/>หมายเลขติดต่อ : {1}<br/>ที่อยู่ : {2}<br/>category : {3}<br/>sub-category : {4}เรื่องที่แจ้ง : {5}", cusName, mailContent.PHNNUMBER, mailContent.adrSummary, cas.casLevel1, cas.casLevel2, cas.casNote);
//                    }
//                    else if (cas.casIDLevel2 == 18 || cas.casIDLevel2 == 20 || cas.casIDLevel2 == 22)    // Quality Complaint
//                    {
//                        mail.MailTo = MAIL_TO_CA_QUALITY_COMPLAINT;
//                        mail.MailCc = MAIL_CC_CA_QUALITY_COMPLAINT;
//                        string subcate = string.Empty;
//                        if (cas.casIDLevel1 == 4)
//                        {
//                            subcate = "Bev";
//                        }
//                        if (cas.casIDLevel1 == 5)
//                        {
//                            subcate = "Food";
//                        }
//                        if (cas.casIDLevel1 == 6)
//                        {
//                            subcate = "Import";
//                        }

//                        mail.MailSubject += "Complaint " + subcate + " " + cas.casIDName;
//                        var mailContent = new CaseRepository().GetMailContentInfo(cas.casID);

//                        var list_ui = new CaseFormUIRepository().getUIbyCatID(Convert.ToInt32(cas.casIDLevel1));
//                        if (list_ui.Count() > 0)
//                        {
//                            var ui = list_ui[0];
//                            var frm = new CaseFormUIRepository().getFormUIByID(ui.uiID);
//                            DataTable dt = new CaseFormUIRepository().getFormDatas(frm.tblDataName, cas.casID);
//                            DataRow dr = dt.Rows[0];

//                            if (ui.uiID == 2)   // CA Snack Import
//                            {
//                                string recSample = "No";
//                                if (dr["casiSample"] != DBNull.Value && Convert.ToBoolean(dr["casiSample"]))
//                                {
//                                    recSample = "Yes";
//                                }

//                                mail.MailBody = string.Format("ชื่อลูกค้า : {0}<br/>หมายเลขติดต่อ : {1}<br/>ที่อยู่ : {2}<br/>วันที่ซื้อสินค้า : {3}<br/>สถานที่ซื้อ : {4}<br/>จำนวนที่ซื้อ : {5}<br/>ขนาดบรรจุ : {6}<br/>วันผลิต : {7} {8}<br/>วันหมดอายุ : {9} {10}<br/>เลขอย. : {11}<br/>category : {12}<br/>sub-category : {13}<br/>เรื่องที่แจ้ง : {14}<br/>Received Simple : {15}<br/>รายละเอียดเพิ่มเติม : {16}", mailContent.CTAFULLNAME, mailContent.PHNNUMBER, mailContent.adrSummary, string.Empty, dr["casiPurchased"], string.Empty, dr["casiSize"], dr["casiMFG"], dr["casiTime"], dr["casiExpire"], dr["casiLine"], dr["casiFDA"], cas.casLevel1, cas.casLevel2, dr["casiIssue"], recSample, cas.casNote);
//                            }
//                            else if (ui.uiID == 3)  // CA Beverage
//                            {
//                                mail.MailBody = string.Format("ชื่อลูกค้า : {0}<br/>หมายเลขติดต่อ : {1}<br/>ที่อยู่ : {2}<br/>วันที่ซื้อสินค้า : {3}<br/>สถานที่ซื้อ : {4}<br/>จำนวนที่ซื้อ : {5}<br/>ขนาดบรรจุ : {6}<br/>เลขอย. : {7}<br/>category : {8}<br/>sub-category : {9}<br/>เรื่องที่แจ้ง : {10}<br/>รายละเอียดเพิ่มเติม : {11}", mailContent.CTAFULLNAME, mailContent.PHNNUMBER, mailContent.adrSummary, string.Empty, dr["cabPurchaseForm"], string.Empty, dr["cabSize"], dr["cabFDA"], cas.casLevel1, cas.casLevel2, dr["cabIssue"], cas.casNote);
//                            }
//                        }
//                    }
//                }
//            }

//            return mail;
//        }
    }
}