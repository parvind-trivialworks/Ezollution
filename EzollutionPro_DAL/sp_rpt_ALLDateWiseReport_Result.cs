//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EzollutionPro_DAL
{
    using System;
    
    public partial class sp_rpt_ALLDateWiseReport_Result
    {
        public string clientName { get; set; }
        public int iInvoiceID { get; set; }
        public string sInvoiceNo { get; set; }
        public Nullable<int> iYear { get; set; }
        public string iMonth { get; set; }
        public Nullable<System.DateTime> InvoiceCreatedDate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
    }
}
