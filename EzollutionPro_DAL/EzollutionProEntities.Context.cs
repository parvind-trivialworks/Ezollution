﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EzollutionProEntities : DbContext
    {
        public EzollutionProEntities()
            : base("name=EzollutionProEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAirClientMaster> tblAirClientMasters { get; set; }
        public virtual DbSet<tblAirEGMFlightDetailsM> tblAirEGMFlightDetailsMs { get; set; }
        public virtual DbSet<tblAirEGMMAWBM> tblAirEGMMAWBMs { get; set; }
        public virtual DbSet<tblAirHAWBM> tblAirHAWBMs { get; set; }
        public virtual DbSet<tblAirIGMFlightDetailsM> tblAirIGMFlightDetailsMs { get; set; }
        public virtual DbSet<tblAirIGMMAWBM> tblAirIGMMAWBMs { get; set; }
        public virtual DbSet<tblAirLocationM> tblAirLocationMs { get; set; }
        public virtual DbSet<tblBillingItem> tblBillingItems { get; set; }
        public virtual DbSet<tblBondMasterM> tblBondMasterMs { get; set; }
        public virtual DbSet<tblCityM> tblCityMs { get; set; }
        public virtual DbSet<tblClientManagement> tblClientManagements { get; set; }
        public virtual DbSet<tblClientManagementFollowup> tblClientManagementFollowups { get; set; }
        public virtual DbSet<tblClientMaster> tblClientMasters { get; set; }
        public virtual DbSet<tblClientMultipleEmail> tblClientMultipleEmails { get; set; }
        public virtual DbSet<tblCompany> tblCompanies { get; set; }
        public virtual DbSet<tblCountryM> tblCountryMs { get; set; }
        public virtual DbSet<tblInvoice> tblInvoices { get; set; }
        public virtual DbSet<tblInvoiceItem> tblInvoiceItems { get; set; }
        public virtual DbSet<tblInvoicePayment> tblInvoicePayments { get; set; }
        public virtual DbSet<tblPermissionsM> tblPermissionsMs { get; set; }
        public virtual DbSet<tblPODMaster> tblPODMasters { get; set; }
        public virtual DbSet<tblPOFDMaster> tblPOFDMasters { get; set; }
        public virtual DbSet<tblPOSMaster> tblPOSMasters { get; set; }
        public virtual DbSet<tblRoleM> tblRoleMs { get; set; }
        public virtual DbSet<tblRolePermissionMap> tblRolePermissionMaps { get; set; }
        public virtual DbSet<tblSeaBondMaster> tblSeaBondMasters { get; set; }
        public virtual DbSet<tblSeaContainerMaster> tblSeaContainerMasters { get; set; }
        public virtual DbSet<tblSeaHBLMaster> tblSeaHBLMasters { get; set; }
        public virtual DbSet<tblSeaMBLMaster> tblSeaMBLMasters { get; set; }
        public virtual DbSet<tblSeaScheduling> tblSeaSchedulings { get; set; }
        public virtual DbSet<tblShippingLine> tblShippingLines { get; set; }
        public virtual DbSet<tblStateM> tblStateMs { get; set; }
        public virtual DbSet<tblUserM> tblUserMs { get; set; }
        public virtual DbSet<vw_AirSea_MAW_MBL> vw_AirSea_MAW_MBL { get; set; }
        public virtual DbSet<vw_ClientMaster> vw_ClientMaster { get; set; }
        public virtual DbSet<tblAirMAWBM> tblAirMAWBMs { get; set; }
    
        public virtual int sp_AddBillingItem(Nullable<int> clientId, Nullable<int> clienType, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> invoiceId)
        {
            var clientIdParameter = clientId.HasValue ?
                new ObjectParameter("ClientId", clientId) :
                new ObjectParameter("ClientId", typeof(int));
    
            var clienTypeParameter = clienType.HasValue ?
                new ObjectParameter("ClienType", clienType) :
                new ObjectParameter("ClienType", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var invoiceIdParameter = invoiceId.HasValue ?
                new ObjectParameter("InvoiceId", invoiceId) :
                new ObjectParameter("InvoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_AddBillingItem", clientIdParameter, clienTypeParameter, fromDateParameter, toDateParameter, invoiceIdParameter);
        }
    
        public virtual int sp_GetInvoice(Nullable<int> clientId, Nullable<int> clienType, Nullable<int> invoiceId)
        {
            var clientIdParameter = clientId.HasValue ?
                new ObjectParameter("ClientId", clientId) :
                new ObjectParameter("ClientId", typeof(int));
    
            var clienTypeParameter = clienType.HasValue ?
                new ObjectParameter("ClienType", clienType) :
                new ObjectParameter("ClienType", typeof(int));
    
            var invoiceIdParameter = invoiceId.HasValue ?
                new ObjectParameter("InvoiceId", invoiceId) :
                new ObjectParameter("InvoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GetInvoice", clientIdParameter, clienTypeParameter, invoiceIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_GetInvoiceItemCount(Nullable<int> clientId, Nullable<int> clienType, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var clientIdParameter = clientId.HasValue ?
                new ObjectParameter("ClientId", clientId) :
                new ObjectParameter("ClientId", typeof(int));
    
            var clienTypeParameter = clienType.HasValue ?
                new ObjectParameter("ClienType", clienType) :
                new ObjectParameter("ClienType", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_GetInvoiceItemCount", clientIdParameter, clienTypeParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual int Usp_AddInvoiceItem(Nullable<int> iInvoiceID, Nullable<int> iInvoiceItemID, string sHSN_SAC, string sHSN_Desc, string sItemDescription, Nullable<int> iQuantity, string sUnit, Nullable<decimal> dAmountPerUnit, Nullable<decimal> dCgstInPercent, Nullable<decimal> dSgstInPercent, Nullable<decimal> dIgstInPercent, Nullable<decimal> dCsesInPercent, Nullable<int> iAddedBy, Nullable<System.DateTime> dtAddedOn, Nullable<int> iModifiedBy, Nullable<System.DateTime> dtModifiedOn, Nullable<bool> blsActive)
        {
            var iInvoiceIDParameter = iInvoiceID.HasValue ?
                new ObjectParameter("iInvoiceID", iInvoiceID) :
                new ObjectParameter("iInvoiceID", typeof(int));
    
            var iInvoiceItemIDParameter = iInvoiceItemID.HasValue ?
                new ObjectParameter("iInvoiceItemID", iInvoiceItemID) :
                new ObjectParameter("iInvoiceItemID", typeof(int));
    
            var sHSN_SACParameter = sHSN_SAC != null ?
                new ObjectParameter("sHSN_SAC", sHSN_SAC) :
                new ObjectParameter("sHSN_SAC", typeof(string));
    
            var sHSN_DescParameter = sHSN_Desc != null ?
                new ObjectParameter("sHSN_Desc", sHSN_Desc) :
                new ObjectParameter("sHSN_Desc", typeof(string));
    
            var sItemDescriptionParameter = sItemDescription != null ?
                new ObjectParameter("sItemDescription", sItemDescription) :
                new ObjectParameter("sItemDescription", typeof(string));
    
            var iQuantityParameter = iQuantity.HasValue ?
                new ObjectParameter("iQuantity", iQuantity) :
                new ObjectParameter("iQuantity", typeof(int));
    
            var sUnitParameter = sUnit != null ?
                new ObjectParameter("sUnit", sUnit) :
                new ObjectParameter("sUnit", typeof(string));
    
            var dAmountPerUnitParameter = dAmountPerUnit.HasValue ?
                new ObjectParameter("dAmountPerUnit", dAmountPerUnit) :
                new ObjectParameter("dAmountPerUnit", typeof(decimal));
    
            var dCgstInPercentParameter = dCgstInPercent.HasValue ?
                new ObjectParameter("dCgstInPercent", dCgstInPercent) :
                new ObjectParameter("dCgstInPercent", typeof(decimal));
    
            var dSgstInPercentParameter = dSgstInPercent.HasValue ?
                new ObjectParameter("dSgstInPercent", dSgstInPercent) :
                new ObjectParameter("dSgstInPercent", typeof(decimal));
    
            var dIgstInPercentParameter = dIgstInPercent.HasValue ?
                new ObjectParameter("dIgstInPercent", dIgstInPercent) :
                new ObjectParameter("dIgstInPercent", typeof(decimal));
    
            var dCsesInPercentParameter = dCsesInPercent.HasValue ?
                new ObjectParameter("dCsesInPercent", dCsesInPercent) :
                new ObjectParameter("dCsesInPercent", typeof(decimal));
    
            var iAddedByParameter = iAddedBy.HasValue ?
                new ObjectParameter("iAddedBy", iAddedBy) :
                new ObjectParameter("iAddedBy", typeof(int));
    
            var dtAddedOnParameter = dtAddedOn.HasValue ?
                new ObjectParameter("dtAddedOn", dtAddedOn) :
                new ObjectParameter("dtAddedOn", typeof(System.DateTime));
    
            var iModifiedByParameter = iModifiedBy.HasValue ?
                new ObjectParameter("iModifiedBy", iModifiedBy) :
                new ObjectParameter("iModifiedBy", typeof(int));
    
            var dtModifiedOnParameter = dtModifiedOn.HasValue ?
                new ObjectParameter("dtModifiedOn", dtModifiedOn) :
                new ObjectParameter("dtModifiedOn", typeof(System.DateTime));
    
            var blsActiveParameter = blsActive.HasValue ?
                new ObjectParameter("blsActive", blsActive) :
                new ObjectParameter("blsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_AddInvoiceItem", iInvoiceIDParameter, iInvoiceItemIDParameter, sHSN_SACParameter, sHSN_DescParameter, sItemDescriptionParameter, iQuantityParameter, sUnitParameter, dAmountPerUnitParameter, dCgstInPercentParameter, dSgstInPercentParameter, dIgstInPercentParameter, dCsesInPercentParameter, iAddedByParameter, dtAddedOnParameter, iModifiedByParameter, dtModifiedOnParameter, blsActiveParameter);
        }
    
        public virtual int Usp_UpdateInvoiceTotal(Nullable<int> iInvoiceId)
        {
            var iInvoiceIdParameter = iInvoiceId.HasValue ?
                new ObjectParameter("iInvoiceId", iInvoiceId) :
                new ObjectParameter("iInvoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_UpdateInvoiceTotal", iInvoiceIdParameter);
        }
    
        public virtual int uspBulkUploadHBL(string sMBLNumber, Nullable<int> iUserId)
        {
            var sMBLNumberParameter = sMBLNumber != null ?
                new ObjectParameter("sMBLNumber", sMBLNumber) :
                new ObjectParameter("sMBLNumber", typeof(string));
    
            var iUserIdParameter = iUserId.HasValue ?
                new ObjectParameter("iUserId", iUserId) :
                new ObjectParameter("iUserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspBulkUploadHBL", sMBLNumberParameter, iUserIdParameter);
        }
    
        public virtual int Usp_UpdateIsDefaultEmail(Nullable<int> iMailId, Nullable<int> iClientType, Nullable<int> iClientId)
        {
            var iMailIdParameter = iMailId.HasValue ?
                new ObjectParameter("iMailId", iMailId) :
                new ObjectParameter("iMailId", typeof(int));
    
            var iClientTypeParameter = iClientType.HasValue ?
                new ObjectParameter("iClientType", iClientType) :
                new ObjectParameter("iClientType", typeof(int));
    
            var iClientIdParameter = iClientId.HasValue ?
                new ObjectParameter("iClientId", iClientId) :
                new ObjectParameter("iClientId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_UpdateIsDefaultEmail", iMailIdParameter, iClientTypeParameter, iClientIdParameter);
        }
    
        public virtual int Usp_UpdateInvoiceBalance(Nullable<int> iInvoiceId)
        {
            var iInvoiceIdParameter = iInvoiceId.HasValue ?
                new ObjectParameter("iInvoiceId", iInvoiceId) :
                new ObjectParameter("iInvoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_UpdateInvoiceBalance", iInvoiceIdParameter);
        }
    
        public virtual ObjectResult<sp_rpt_ALLDateWiseReport_Result> sp_rpt_ALLDateWiseReport(string fromDate, string toDate)
        {
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_rpt_ALLDateWiseReport_Result>("sp_rpt_ALLDateWiseReport", fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<sp_rpt_DateWiseReport_Result> sp_rpt_DateWiseReport(Nullable<int> clientId, Nullable<int> clienType, string fromDate, string toDate)
        {
            var clientIdParameter = clientId.HasValue ?
                new ObjectParameter("ClientId", clientId) :
                new ObjectParameter("ClientId", typeof(int));
    
            var clienTypeParameter = clienType.HasValue ?
                new ObjectParameter("ClienType", clienType) :
                new ObjectParameter("ClienType", typeof(int));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_rpt_DateWiseReport_Result>("sp_rpt_DateWiseReport", clientIdParameter, clienTypeParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<sp_rpt_IGMAIR_Report_Result> sp_rpt_IGMAIR_Report(string clientId, string fromDate, string toDate, string filterBy)
        {
            var clientIdParameter = clientId != null ?
                new ObjectParameter("clientId", clientId) :
                new ObjectParameter("clientId", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            var filterByParameter = filterBy != null ?
                new ObjectParameter("filterBy", filterBy) :
                new ObjectParameter("filterBy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_rpt_IGMAIR_Report_Result>("sp_rpt_IGMAIR_Report", clientIdParameter, fromDateParameter, toDateParameter, filterByParameter);
        }
    
        public virtual ObjectResult<sp_rpt_MBLReport_Result> sp_rpt_MBLReport(string clientId, string fromDate, string toDate, string filterBy)
        {
            var clientIdParameter = clientId != null ?
                new ObjectParameter("clientId", clientId) :
                new ObjectParameter("clientId", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            var filterByParameter = filterBy != null ?
                new ObjectParameter("filterBy", filterBy) :
                new ObjectParameter("filterBy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_rpt_MBLReport_Result>("sp_rpt_MBLReport", clientIdParameter, fromDateParameter, toDateParameter, filterByParameter);
        }
    
        public virtual ObjectResult<uspGetMsgResult_Result> uspGetMsgResult(Nullable<int> iSchedulingId)
        {
            var iSchedulingIdParameter = iSchedulingId.HasValue ?
                new ObjectParameter("iSchedulingId", iSchedulingId) :
                new ObjectParameter("iSchedulingId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<uspGetMsgResult_Result>("uspGetMsgResult", iSchedulingIdParameter);
        }
    
        public virtual int Usp_AddInvoiceItem_Old(Nullable<int> iInvoiceId, string sHSN_SAC, string sHSN_Desc, Nullable<int> iQuantity, string sUnit, Nullable<decimal> dAmountPerUnit, Nullable<decimal> dCgstInPercent, Nullable<decimal> dSgstInPercent, Nullable<decimal> dIgstInPercent, Nullable<decimal> dCsesInPercent, Nullable<int> iAddedBy, Nullable<System.DateTime> dtAddedOn, Nullable<int> iModifiedBy, Nullable<System.DateTime> dtModifiedOn)
        {
            var iInvoiceIdParameter = iInvoiceId.HasValue ?
                new ObjectParameter("iInvoiceId", iInvoiceId) :
                new ObjectParameter("iInvoiceId", typeof(int));
    
            var sHSN_SACParameter = sHSN_SAC != null ?
                new ObjectParameter("sHSN_SAC", sHSN_SAC) :
                new ObjectParameter("sHSN_SAC", typeof(string));
    
            var sHSN_DescParameter = sHSN_Desc != null ?
                new ObjectParameter("sHSN_Desc", sHSN_Desc) :
                new ObjectParameter("sHSN_Desc", typeof(string));
    
            var iQuantityParameter = iQuantity.HasValue ?
                new ObjectParameter("iQuantity", iQuantity) :
                new ObjectParameter("iQuantity", typeof(int));
    
            var sUnitParameter = sUnit != null ?
                new ObjectParameter("sUnit", sUnit) :
                new ObjectParameter("sUnit", typeof(string));
    
            var dAmountPerUnitParameter = dAmountPerUnit.HasValue ?
                new ObjectParameter("dAmountPerUnit", dAmountPerUnit) :
                new ObjectParameter("dAmountPerUnit", typeof(decimal));
    
            var dCgstInPercentParameter = dCgstInPercent.HasValue ?
                new ObjectParameter("dCgstInPercent", dCgstInPercent) :
                new ObjectParameter("dCgstInPercent", typeof(decimal));
    
            var dSgstInPercentParameter = dSgstInPercent.HasValue ?
                new ObjectParameter("dSgstInPercent", dSgstInPercent) :
                new ObjectParameter("dSgstInPercent", typeof(decimal));
    
            var dIgstInPercentParameter = dIgstInPercent.HasValue ?
                new ObjectParameter("dIgstInPercent", dIgstInPercent) :
                new ObjectParameter("dIgstInPercent", typeof(decimal));
    
            var dCsesInPercentParameter = dCsesInPercent.HasValue ?
                new ObjectParameter("dCsesInPercent", dCsesInPercent) :
                new ObjectParameter("dCsesInPercent", typeof(decimal));
    
            var iAddedByParameter = iAddedBy.HasValue ?
                new ObjectParameter("iAddedBy", iAddedBy) :
                new ObjectParameter("iAddedBy", typeof(int));
    
            var dtAddedOnParameter = dtAddedOn.HasValue ?
                new ObjectParameter("dtAddedOn", dtAddedOn) :
                new ObjectParameter("dtAddedOn", typeof(System.DateTime));
    
            var iModifiedByParameter = iModifiedBy.HasValue ?
                new ObjectParameter("iModifiedBy", iModifiedBy) :
                new ObjectParameter("iModifiedBy", typeof(int));
    
            var dtModifiedOnParameter = dtModifiedOn.HasValue ?
                new ObjectParameter("dtModifiedOn", dtModifiedOn) :
                new ObjectParameter("dtModifiedOn", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_AddInvoiceItem_Old", iInvoiceIdParameter, sHSN_SACParameter, sHSN_DescParameter, iQuantityParameter, sUnitParameter, dAmountPerUnitParameter, dCgstInPercentParameter, dSgstInPercentParameter, dIgstInPercentParameter, dCsesInPercentParameter, iAddedByParameter, dtAddedOnParameter, iModifiedByParameter, dtModifiedOnParameter);
        }
    
        public virtual int Usp_AddInvoiceItem2(Nullable<int> iClientId, Nullable<int> iYear, Nullable<int> iMonth, string sHSN_SAC, string sHSN_Desc, Nullable<int> iQuantity, string sUnit, Nullable<decimal> dAmountPerUnit, Nullable<decimal> dCgstInPercent, Nullable<decimal> dSgstInPercent, Nullable<decimal> dIgstInPercent, Nullable<decimal> dCsesInPercent, Nullable<int> iAddedBy, Nullable<System.DateTime> dtAddedOn, Nullable<int> iModifiedBy, Nullable<System.DateTime> dtModifiedOn)
        {
            var iClientIdParameter = iClientId.HasValue ?
                new ObjectParameter("iClientId", iClientId) :
                new ObjectParameter("iClientId", typeof(int));
    
            var iYearParameter = iYear.HasValue ?
                new ObjectParameter("iYear", iYear) :
                new ObjectParameter("iYear", typeof(int));
    
            var iMonthParameter = iMonth.HasValue ?
                new ObjectParameter("iMonth", iMonth) :
                new ObjectParameter("iMonth", typeof(int));
    
            var sHSN_SACParameter = sHSN_SAC != null ?
                new ObjectParameter("sHSN_SAC", sHSN_SAC) :
                new ObjectParameter("sHSN_SAC", typeof(string));
    
            var sHSN_DescParameter = sHSN_Desc != null ?
                new ObjectParameter("sHSN_Desc", sHSN_Desc) :
                new ObjectParameter("sHSN_Desc", typeof(string));
    
            var iQuantityParameter = iQuantity.HasValue ?
                new ObjectParameter("iQuantity", iQuantity) :
                new ObjectParameter("iQuantity", typeof(int));
    
            var sUnitParameter = sUnit != null ?
                new ObjectParameter("sUnit", sUnit) :
                new ObjectParameter("sUnit", typeof(string));
    
            var dAmountPerUnitParameter = dAmountPerUnit.HasValue ?
                new ObjectParameter("dAmountPerUnit", dAmountPerUnit) :
                new ObjectParameter("dAmountPerUnit", typeof(decimal));
    
            var dCgstInPercentParameter = dCgstInPercent.HasValue ?
                new ObjectParameter("dCgstInPercent", dCgstInPercent) :
                new ObjectParameter("dCgstInPercent", typeof(decimal));
    
            var dSgstInPercentParameter = dSgstInPercent.HasValue ?
                new ObjectParameter("dSgstInPercent", dSgstInPercent) :
                new ObjectParameter("dSgstInPercent", typeof(decimal));
    
            var dIgstInPercentParameter = dIgstInPercent.HasValue ?
                new ObjectParameter("dIgstInPercent", dIgstInPercent) :
                new ObjectParameter("dIgstInPercent", typeof(decimal));
    
            var dCsesInPercentParameter = dCsesInPercent.HasValue ?
                new ObjectParameter("dCsesInPercent", dCsesInPercent) :
                new ObjectParameter("dCsesInPercent", typeof(decimal));
    
            var iAddedByParameter = iAddedBy.HasValue ?
                new ObjectParameter("iAddedBy", iAddedBy) :
                new ObjectParameter("iAddedBy", typeof(int));
    
            var dtAddedOnParameter = dtAddedOn.HasValue ?
                new ObjectParameter("dtAddedOn", dtAddedOn) :
                new ObjectParameter("dtAddedOn", typeof(System.DateTime));
    
            var iModifiedByParameter = iModifiedBy.HasValue ?
                new ObjectParameter("iModifiedBy", iModifiedBy) :
                new ObjectParameter("iModifiedBy", typeof(int));
    
            var dtModifiedOnParameter = dtModifiedOn.HasValue ?
                new ObjectParameter("dtModifiedOn", dtModifiedOn) :
                new ObjectParameter("dtModifiedOn", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_AddInvoiceItem2", iClientIdParameter, iYearParameter, iMonthParameter, sHSN_SACParameter, sHSN_DescParameter, iQuantityParameter, sUnitParameter, dAmountPerUnitParameter, dCgstInPercentParameter, dSgstInPercentParameter, dIgstInPercentParameter, dCsesInPercentParameter, iAddedByParameter, dtAddedOnParameter, iModifiedByParameter, dtModifiedOnParameter);
        }
    
        public virtual int Usp_UpdateInvoiceBalance1(Nullable<int> iInvoiceId)
        {
            var iInvoiceIdParameter = iInvoiceId.HasValue ?
                new ObjectParameter("iInvoiceId", iInvoiceId) :
                new ObjectParameter("iInvoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Usp_UpdateInvoiceBalance1", iInvoiceIdParameter);
        }
    }
}
