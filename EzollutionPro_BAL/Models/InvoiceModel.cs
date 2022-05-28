using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EzollutionPro_BAL.Models
{
    public class InvoiceSearchModel   
    {
        public int iClientId { get; set; }
        public Int16 iClientType { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ToDate { get; set; }

        public string PaymentStatus { get; set; }

        public List<InvoiceModel> _List { get; set; }
    }
    public class InvoiceModel
    {
        public int iInvoiceID { get; set; }
        public string sInvoiceNo { get; set; }
        public int iClientId { get; set; }
        public int? iYear { get; set; }
        public int? iMonth { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public Int16 iClientType { get; set; }
        public string sPOS { get; set; }
        [Required(ErrorMessage ="This field is required")]
        public string FromInvoiceDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string ToInvoiceDate { get; set; }
        
        public bool blsPaymentStatus { get; set; }

        public string sPaymentMode { get; set; }
       
        public string dtPaymentDate { get; set; }
       
        public string dtReceivedDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public Nullable<int> iCompanyId { get; set; }

        public bool? blsActive { get; set; }

        public string StrClientName { get; set; }
        public string StrClientType { get; set; }

        public string StrCreateDate { get; set; }
        public string StrCompanyName { get; set; }

        public string StrPaymentStatus { get; set; }

        public string sClientCode { get; set; }
        public decimal? dTotalAmount { get; set; }
        public decimal? dPaidAmount { get; set; }
        public decimal? dBalance { get; set; }
        public decimal? dTotalTds { get; set; }
        //public InvoiceItemModel _InvoiceItemModel { get; set; }
        public List<InvoiceItemModel> _ItemList { get; set; }

    }


    public class InvoiceItemModel
    {
        public int? iClientId { get; set; }
        public int? iYear { get; set; }
        public int? iMonth { get; set; }

        public int iInvoiceItemID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int iInvoiceID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sHSN_SAC { get; set; }
        
        public string sItemDescription { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sHSN_Desc { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int iQuantity { get; set; }

       // [Required(ErrorMessage = "This field is required")]
        public string sUnit { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal? dAmountPerUnit { get; set; }
        public int? dCgstInPercent { get; set; }
        public int? dSgstInPercent { get; set; }
        public int? dIgstInPercent { get; set; }      
        public int? dCsesInPercent { get; set; }

        public bool blsActive { get; set; }

        public bool? IsStateCodeSame { get; set; }

        public decimal? dTotalAmount { get; set; }

    }


    public class InvoicePaymentContainer
    {
        public InvoiceModel _InvoiceModel { get; set; }

    }

    public partial class InvoicePaymentModel
    {
        public int iInvoicePaymentID { get; set; }
        public int iInvoiceId { get; set; }
        public string sCreateDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public Nullable<decimal> dAmount { get; set; }
        public Nullable<decimal> dTds { get; set; }
        public Nullable<decimal> dBalance { get; set; }
        public bool blsPaymentStatus { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string sPaymentMode { get; set; }
        public string sCheckNeftNo { get; set; }
        public string dtCheckDate { get; set; }
        public string dtReceivedDate { get; set; }
        public Nullable<bool> blsActive { get; set; }
        public Nullable<int> iAddedBy { get; set; }
        public string dtAddedOn { get; set; }
        public string sAddedFromIp { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public string dtModifiedOn { get; set; }
        public string sModifiedFromIp { get; set; }

        public string StrPaymentStatus { get; set; }

        public string sClientCode { get; set; }
        public string StrClientType { get; set; }
        public string StrClientName { get; set; }
        public string sInvoiceNo { get; set; }
    }

    public class InvoicePaymentSearchModel
    {
        public int iClientId { get; set; }
        public Int16 iClientType { get; set; }

        
        public string FromDate { get; set; }

        
        public string ToDate { get; set; }

        public string sInvoiceNo { get; set; }

        public List<InvoicePaymentModel> _List { get; set; }
    }
}
