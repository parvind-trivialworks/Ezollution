using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Services
{
    public class InvoiceService
    {
        private static InvoiceService instance = null;
        private InvoiceService()
        {
        }
        public static InvoiceService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvoiceService();
                }
                return instance;
            }
        }


        public List<InvoiceItemModel> GetInvoiceItems(int iInvoiceId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblInvoiceItems.Where(x => x.iInvoiceID == iInvoiceId && x.blsActive == true).ToList()
                    .Select((z, i) => new InvoiceItemModel
                    {
                        iInvoiceID = z.iInvoiceID,
                        iInvoiceItemID = z.iInvoiceItemID,
                        sHSN_SAC = z.sHSN_SAC,
                        sHSN_Desc = z.sHSN_Desc,
                        iQuantity = z.iQuantity.GetValueOrDefault(),
                        sUnit = z.sUnit,
                        sItemDescription = z.sItemDescription,
                        dAmountPerUnit = z.dAmountPerUnit.GetValueOrDefault(),
                        dCgstInPercent = Convert.ToInt32(z.dCgstInPercent.GetValueOrDefault()),
                        dSgstInPercent = Convert.ToInt32(z.dSgstInPercent.GetValueOrDefault()),
                        dIgstInPercent = Convert.ToInt32(z.dIgstInPercent.GetValueOrDefault()),
                        dCsesInPercent = Convert.ToInt32(z.dCsesInPercent.GetValueOrDefault()),
                        dTotalAmount = z.dTotalAmount.GetValueOrDefault()
                    }).ToList();

            }
        }
        public InvoiceItemModel AddUdpdateInvoiceItem(int iInvoiceItemID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblInvoiceItems.Where(x => x.iInvoiceItemID == iInvoiceItemID).ToList()
                .Select((z, i) => new InvoiceItemModel
                {
                    iInvoiceItemID=z.iInvoiceItemID,
                    iInvoiceID = z.iInvoiceID,
                    sHSN_SAC = z.sHSN_SAC,
                    sHSN_Desc = z.sHSN_Desc,
                    iQuantity = z.iQuantity.GetValueOrDefault(),
                    sUnit = z.sUnit,
                    sItemDescription = z.sItemDescription,
                    dAmountPerUnit =z.dAmountPerUnit.GetValueOrDefault(),
                    dCgstInPercent = Convert.ToInt32(z.dCgstInPercent.GetValueOrDefault()),
                    dSgstInPercent = Convert.ToInt32(z.dSgstInPercent.GetValueOrDefault()),
                    dIgstInPercent = Convert.ToInt32(z.dIgstInPercent.GetValueOrDefault()),
                    dCsesInPercent = Convert.ToInt32(z.dCsesInPercent.GetValueOrDefault()),
                    blsActive = z.blsActive.GetValueOrDefault()
                }).FirstOrDefault();
            }
        }

        public List<InvoiceItemModel> GetInvoiceItemsByClient(int iYear, int iMonth, int iClientId, Int16 clientType)
        {
            using (var db = new EzollutionProEntities())
            {
                var objInvoice = db.tblInvoices.Where(x => x.iClientId == iClientId && x.iYear == iYear && x.iMonth == iMonth && x.blsActive == true).FirstOrDefault();
                if (objInvoice != null)
                {
                    int iInvoiceId = objInvoice.iInvoiceID;
                    return db.tblInvoiceItems.Where(x => x.iInvoiceID == iInvoiceId && x.blsActive == true).ToList()
                        .Select((z, i) => new InvoiceItemModel
                        {
                            iInvoiceID = z.iInvoiceID,
                            sHSN_SAC = z.sHSN_SAC,
                            sHSN_Desc = z.sHSN_Desc,
                            iQuantity = z.iQuantity.GetValueOrDefault(),
                            sUnit = z.sUnit,
                            sItemDescription = z.sItemDescription,
                            dAmountPerUnit = z.dAmountPerUnit.GetValueOrDefault(),
                            dCgstInPercent = Convert.ToInt32(z.dCgstInPercent.GetValueOrDefault()),
                            dSgstInPercent = Convert.ToInt32(z.dSgstInPercent.GetValueOrDefault()),
                            dIgstInPercent = Convert.ToInt32(z.dIgstInPercent.GetValueOrDefault()),
                            dCsesInPercent = Convert.ToInt32(z.dCsesInPercent.GetValueOrDefault()),
                        }).ToList();
                }
                else return null;
            }
        }

        public int AddUpdateInvoice(InvoiceModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblInvoices.Where(z => z.iInvoiceID == model.iInvoiceID).FirstOrDefault();
                if (data != null)
                {
                    data.iCompanyId = model.iCompanyId;
                    data.iClientType = model.iClientType;
                    data.iClientId = model.iClientId;
                    data.blsPaymentStatus = model.blsPaymentStatus;
                    data.sPaymentMode = model.sPaymentMode;
                    data.FromInvoiceDate = model.FromInvoiceDate.ConvertDate();
                    data.ToInvoiceDate = model.ToInvoiceDate.ConvertDate();
                    data.dtPaymentDate = model.dtPaymentDate.ConvertDate();
                    data.dtReceivedDate = model.dtReceivedDate.ConvertDate();
                    data.sPOS = model.sPOS;
                    data.dtModifiedOn = DateTime.Now;
                    data.iModifiedBy = iUserId;
                    data.blsActive = model.blsActive;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.sp_AddBillingItem(model.iClientId, model.iClientType, model.FromInvoiceDate.ConvertDate(), model.ToInvoiceDate.ConvertDate(), data.iInvoiceID);
                    return data.iInvoiceID;
                }
                else
                {
                    var client = db.vw_ClientMaster.Where(x => x.iClientId == model.iClientId && x.iClientType==model.iClientType.ToString()).FirstOrDefault();
                    int InvoiceCount = db.tblInvoices.Where(x=>x.iClientId==model.iClientId && x.iClientType==model.iClientType).Count() + 1;
                    int CurrentYear = System.DateTime.Now.Year;
                    string InvoiceNo = "";
                    if (InvoiceCount < 10)
                    {
                        InvoiceNo = "0000" + InvoiceCount.ToString();
                    }
                    else if (InvoiceCount < 100)
                    {
                        InvoiceNo = "000" + InvoiceCount.ToString();
                    }
                    else if (InvoiceCount < 1000)
                    {
                        InvoiceNo = "00" + InvoiceCount.ToString();
                    }
                    else if (InvoiceCount < 10000)
                    {
                        InvoiceNo = "0" + InvoiceCount.ToString();
                    }
                    else
                    {
                        InvoiceNo = "" + InvoiceCount.ToString();
                    }
                    string year = System.DateTime.Now.Year.ToString() + "-" + (CurrentYear + 1).ToString();
                    if(System.DateTime.Now.Month<=3)
                    {
                        year = (System.DateTime.Now.Year-1).ToString() + "-" + (CurrentYear).ToString();
                    }
                    string clientCode =Convert.ToString(client.sClientCode);
                    if(string.IsNullOrEmpty(clientCode))
                    {
                        clientCode = client.iClientId.ToString();
                    }
                    InvoiceNo = "EZS/"+ clientCode + "/" + year + "/" + InvoiceNo;
                    data = new tblInvoice
                    {
                        dtAddedOn = DateTime.Now,
                        iAddedBy = iUserId,
                        sAddedFromIp = IP,
                        sInvoiceNo = InvoiceNo,
                        iClientId = model.iClientId,
                        iYear = DateTime.Now.Year,
                        iMonth = DateTime.Now.Month,
                        iClientType = model.iClientType,
                        iCompanyId = model.iCompanyId,
                        blsPaymentStatus = model.blsPaymentStatus,
                        sPaymentMode = model.sPaymentMode,
                        FromInvoiceDate = model.FromInvoiceDate.ConvertDate(),
                        ToInvoiceDate = model.ToInvoiceDate.ConvertDate(),
                        dtPaymentDate = model.dtPaymentDate.ConvertDate(),
                        dtReceivedDate = model.dtReceivedDate.ConvertDate(),
                        dTotalAmount=0,
                        dBalance=0,
                        dTotalTds=0,
                        dPaidAmount=0,
                        sPOS = model.sPOS,
                        blsActive = true,
                        InvoiceCreatedDate = System.DateTime.Now
                    };
                    db.tblInvoices.Add(data);
                    db.SaveChanges();
                    db.sp_AddBillingItem(model.iClientId, model.iClientType, model.FromInvoiceDate.ConvertDate(), model.ToInvoiceDate.ConvertDate(), data.iInvoiceID);
                    return data.iInvoiceID;
                }
            }
        }

        public ResponseStatus AddInvoiceItem(InvoiceItemModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {

                db.Usp_AddInvoiceItem(model.iInvoiceID, model.iInvoiceItemID, model.sHSN_SAC, model.sHSN_Desc, model.sItemDescription,
                    model.iQuantity, model.sUnit, model.dAmountPerUnit
                    , model.dCgstInPercent, model.dSgstInPercent, model.dIgstInPercent, model.dCsesInPercent, iUserId,
                    System.DateTime.Now, iUserId, System.DateTime.Now, model.blsActive);
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Invoice item added successfully!"
                };
            }
        }


        public InvoiceModel GetInvoice(int iInvoiceId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblInvoices.Where(x => x.iInvoiceID == iInvoiceId && x.blsActive == true).ToList()
                    .Select((z, i) => new InvoiceModel
                    {
                        iInvoiceID = z.iInvoiceID,
                        iClientId = z.iClientId.GetValueOrDefault(),
                        iClientType = z.iClientType.GetValueOrDefault(),
                        iYear = z.iYear,
                        iMonth = z.iMonth,
                        sInvoiceNo = z.sInvoiceNo,
                        FromInvoiceDate = z.FromInvoiceDate.FormatDate(),
                        ToInvoiceDate = z.ToInvoiceDate.FormatDate(),
                        blsPaymentStatus = z.blsPaymentStatus.GetValueOrDefault(),
                        sPaymentMode = z.sPaymentMode,
                        dtPaymentDate = z.dtPaymentDate.FormatDate(),
                        dtReceivedDate = z.dtReceivedDate.FormatDate(),
                        iCompanyId = z.iCompanyId,
                        sPOS = z.sPOS,
                        blsActive = z.blsActive,
                        dTotalAmount=z.dTotalAmount,
                        dPaidAmount=z.dPaidAmount,
                        dBalance=z.dBalance
                    }).FirstOrDefault();
            }
        }

        public static string IP
        {
            get
            {
                try
                {
                    return new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
                }
                catch (Exception)
                {

                    return "";
                }
            }
        }

        public List<InvoiceModel> GetInvoiceList(string minDate, int? iclientId, string maxDate, int iClientType,string PaymentStatus ,out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {

                DateTime dtMinDate, dtMaxDate;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = dtMaxDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                else
                {
                    var todaysDate = DateTime.Now;
                    dtMinDate = DateTime.Now.Date.AddMonths(-1);
                    dtMaxDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                var query = (from invoice in db.tblInvoices
                             join company in db.tblCompanies on invoice.iCompanyId equals company.iCompanyId
                             join client in db.vw_ClientMaster on invoice.iClientId equals client.iClientId
                             where ((iclientId > 0 ? invoice.iClientId == iclientId : 1 == 1) && (invoice.iClientType == iClientType)
                             && (client.iClientType == iClientType.ToString())
                             && (invoice.InvoiceCreatedDate >= dtMinDate && invoice.InvoiceCreatedDate <= dtMaxDate)
                             && invoice.blsActive == true)
                select new { invoice, company, client });

                if (PaymentStatus == "paid")
                {
                    query = query.Where(z => (z.invoice.dTotalAmount > 0 && z.invoice.dBalance == 0));
                }
                else if (PaymentStatus == "pending")
                {
                    query = query.Where(z => z.invoice.dTotalAmount >= 0
                    && !(z.invoice.dTotalAmount > 0 && z.invoice.dBalance == 0));
                }
                
                recordsTotal = query.Count();
                return query.OrderByDescending(z => z.invoice.iInvoiceID).ToList().Select((z, i) => new InvoiceModel
                {
                    sInvoiceNo = z.invoice.sInvoiceNo,
                    iInvoiceID = z.invoice.iInvoiceID,
                    StrCreateDate = z.invoice.InvoiceCreatedDate.FormatDate(),
                    StrCompanyName = z.company.sName,
                    StrClientType = z.invoice.iClientType == 0 ? "Sea" : "Air",
                    StrClientName = z.client.ClientName,
                    FromInvoiceDate = z.invoice.FromInvoiceDate.FormatDate(),
                    ToInvoiceDate = z.invoice.ToInvoiceDate.FormatDate(),
                    StrPaymentStatus = ConvertPaymentStatus(z.invoice.dTotalAmount.GetValueOrDefault(),z.invoice.dPaidAmount.GetValueOrDefault(), z.invoice.dBalance.GetValueOrDefault()),
                    sPaymentMode = z.invoice.sPaymentMode,
                    dtPaymentDate = z.invoice.dtPaymentDate.FormatDate(),
                    dtReceivedDate = z.invoice.dtReceivedDate.FormatDate(),
                    sPOS = z.invoice.sPOS,
                    sClientCode=z.client.sClientCode,
                    dTotalAmount=z.invoice.dTotalAmount,
                    dPaidAmount=z.invoice.dPaidAmount,
                    dBalance=z.invoice.dBalance,
                    dTotalTds=z.invoice.dTotalTds
                }).ToList();
            }
        }
        public string ConvertPaymentStatus(decimal InvoiceAmount,decimal ReceivedAmount,decimal Balance)
        {
            if (InvoiceAmount == 0)
            {
                return "Pending";
            }
            else
            {
                if (ReceivedAmount == 0)
                {
                    return "Pending";
                }
                else if (Balance == 0)
                {
                    return "Paid";
                }
                else
                {
                    return "Partial";
                }
            }
        }
        public bool IsStateCodeSame(int iInvoiceId)
        {
            using (var db = new EzollutionProEntities())
            {
                var Invoice = db.tblInvoices.Where(x => x.iInvoiceID == iInvoiceId).FirstOrDefault();
                if (Invoice != null)
                {
                    var company = db.tblCompanies.Where(x => x.iCompanyId == Invoice.iCompanyId).FirstOrDefault();
                    if (company != null)
                    {
                        string sClientType = Convert.ToString(Invoice.iClientType);
                        var client = db.vw_ClientMaster.Where(x => x.iClientId == Invoice.iClientId && x.iClientType == sClientType).FirstOrDefault();
                        if (client != null)
                        {
                            if (company.sStateCode == client.sStateCode)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public int GetItemCount(string sHSN_SAC, int iClientId, int ClienType, DateTime FromDate, DateTime ToDate)
        {
            using (var db = new EzollutionProEntities())
            {
                var result = db.sp_GetInvoiceItemCount(iClientId, ClienType, FromDate, ToDate).FirstOrDefault();
                if (result != null)
                    return result.GetValueOrDefault();
                else return 0;
            }
        }

        public List<InvoicePaymentModel> GetInvoicePayments(int iInvoiceId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblInvoicePayments.Where(x => x.iInvoiceId == iInvoiceId && x.blsActive == true).ToList()
                    .Select((z, i) => new InvoicePaymentModel
                    {
                        iInvoiceId = z.iInvoiceId,
                        iInvoicePaymentID = z.iInvoicePaymentID,
                        sCreateDate = z.dtCreateDate.FormatDate(),
                        dAmount = z.dAmount.GetValueOrDefault(),
                        dTds = z.dTds.GetValueOrDefault(),
                        dBalance = z.dBalance.GetValueOrDefault(),
                        blsPaymentStatus = z.blsPaymentStatus.GetValueOrDefault(),
                        sPaymentMode = z.sPaymentMode,
                        sCheckNeftNo = z.sCheckNeftNo,
                        dtCheckDate = z.dtCheckDate.FormatDate(),
                        dtReceivedDate = z.dtReceivedDate.FormatDate(),
                        blsActive = z.blsActive,
                        dtAddedOn = z.dtAddedOn.FormatDate(),
                        dtModifiedOn = z.dtModifiedOn.FormatDate(),
                        StrPaymentStatus = z.blsPaymentStatus.GetValueOrDefault() == false ? "Un-Paid" : "Paid",
                    }).ToList();

            }
        }

        public InvoicePaymentModel AddUdpdateInvoicePayment(int iInvoicePaymentID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblInvoicePayments.Where(x => x.iInvoicePaymentID == iInvoicePaymentID).ToList()
                .Select((z, i) => new InvoicePaymentModel
                {
                    iInvoiceId = z.iInvoiceId,
                    iInvoicePaymentID = z.iInvoicePaymentID,
                    sCreateDate = z.dtCreateDate.FormatDate(),
                    dAmount = z.dAmount.GetValueOrDefault(),
                    dTds = z.dTds.GetValueOrDefault(),
                    dBalance = z.dBalance.GetValueOrDefault(),
                    blsPaymentStatus = z.blsPaymentStatus.GetValueOrDefault(),
                    sPaymentMode = z.sPaymentMode,
                    sCheckNeftNo = z.sCheckNeftNo,
                    dtCheckDate = z.dtCheckDate.FormatDate(),
                    dtReceivedDate = z.dtReceivedDate.FormatDate(),
                    blsActive = z.blsActive,
                    dtAddedOn = z.dtAddedOn.FormatDate(),
                    dtModifiedOn = z.dtModifiedOn.FormatDate(),
                }).FirstOrDefault();
            }
        }

        public int SaveInvoicePayment(InvoicePaymentModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblInvoicePayments.Where(z => z.iInvoicePaymentID == model.iInvoicePaymentID).FirstOrDefault();
                if (data != null)
                {
                    data.iInvoiceId = model.iInvoiceId;
                    data.dAmount = model.dAmount;
                    data.dTds = model.dTds;
                    data.sPaymentMode = model.sPaymentMode;
                    data.dBalance = model.dAmount - model.dTds;
                    data.blsPaymentStatus = model.blsPaymentStatus;
                    data.sPaymentMode = model.sPaymentMode;
                    data.sCheckNeftNo = model.sCheckNeftNo;
                    data.dtCheckDate = model.dtCheckDate.ConvertDate();
                    data.dtReceivedDate = model.dtReceivedDate.ConvertDate();
                    data.blsActive = model.blsActive;
                    data.dtModifiedOn = DateTime.Now;
                    data.iModifiedBy = iUserId;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.Usp_UpdateInvoiceBalance(model.iInvoiceId);
                    return data.iInvoicePaymentID;
                }
                else
                {

                    data = new tblInvoicePayment
                    {
                        iInvoiceId = model.iInvoiceId,
                        dtCreateDate = System.DateTime.UtcNow,
                        dAmount = model.dAmount,
                        dTds = model.dTds,
                        sPaymentMode = model.sPaymentMode,
                        dBalance = model.dAmount - model.dTds,
                        blsPaymentStatus = model.blsPaymentStatus,
                        sCheckNeftNo = model.sCheckNeftNo,
                        dtCheckDate = model.dtCheckDate.ConvertDate(),
                        dtReceivedDate = model.dtReceivedDate.ConvertDate(),
                        blsActive = true,
                        dtAddedOn = DateTime.Now,
                        iAddedBy = iUserId,
                        sAddedFromIp = IP,
                    };
                    db.tblInvoicePayments.Add(data);
                    db.SaveChanges();
                    db.Usp_UpdateInvoiceBalance(model.iInvoiceId);
                    return data.iInvoicePaymentID;
                }
            }
        }

        public List<InvoicePaymentModel> GetPaymentsList(string minDate, int? iclientId, string maxDate, int iClientType, string sInvoiceNo, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {

                DateTime dtMinDate, dtMaxDate;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = dtMaxDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                else
                {
                    var todaysDate = DateTime.Now;
                    dtMinDate = DateTime.Now.Date.AddMonths(-1);
                    dtMaxDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                var query = (from payments in db.tblInvoicePayments
                             join invoice in db.tblInvoices on payments.iInvoiceId equals invoice.iInvoiceID
                             join company in db.tblCompanies on invoice.iCompanyId equals company.iCompanyId
                             join client in db.vw_ClientMaster on invoice.iClientId equals client.iClientId
                             where ((iclientId > 0 ? invoice.iClientId == iclientId : 1 == 1) && (invoice.iClientType == iClientType)
                             && (client.iClientType == iClientType.ToString())
                             //&& (payments.dtCreateDate >= dtMinDate && payments.dtCreateDate <= dtMaxDate)
                             && invoice.blsActive == true
                             && payments.blsActive==true)
                             select new { invoice, company, client, payments });

                if(dtMinDate!=null && dtMaxDate!=null)
                {
                    query = query.Where(x => x.payments.dtCreateDate >= dtMinDate && x.payments.dtCreateDate <= dtMaxDate);
                }
                if(!string.IsNullOrEmpty(sInvoiceNo))
                {
                    query= from m in query
                           where m.invoice.sInvoiceNo.Contains(sInvoiceNo.Trim())
                           select m;
                }
                recordsTotal = query.Count();
                return query.OrderBy(z => z.invoice.InvoiceCreatedDate).ToList().Select((z, i) => new InvoicePaymentModel
                {

                    iInvoiceId = z.invoice.iInvoiceID,
                    iInvoicePaymentID = z.payments.iInvoicePaymentID,
                    sCreateDate = z.payments.dtCreateDate.FormatDate(),
                    dAmount = z.payments.dAmount.GetValueOrDefault(),
                    dTds = z.payments.dTds.GetValueOrDefault(),
                    dBalance = z.payments.dBalance.GetValueOrDefault(),
                    blsPaymentStatus = z.payments.blsPaymentStatus.GetValueOrDefault(),
                    sPaymentMode = z.payments.sPaymentMode,
                    sCheckNeftNo = z.payments.sCheckNeftNo,
                    dtCheckDate = z.payments.dtCheckDate.FormatDate(),
                    dtReceivedDate = z.payments.dtReceivedDate.FormatDate(),
                    blsActive = z.payments.blsActive,
                    dtAddedOn = z.payments.dtAddedOn.FormatDate(),
                    dtModifiedOn = z.payments.dtModifiedOn.FormatDate(),
                    StrPaymentStatus = z.payments.blsPaymentStatus.GetValueOrDefault() == false ? "Un-Paid" : "Paid",
                    sClientCode = z.client.sClientCode,
                    StrClientType = z.invoice.iClientType == 0 ? "Sea" : "Air",
                    StrClientName = z.client.ClientName,
                    sInvoiceNo = z.invoice.sInvoiceNo,
                }).ToList();

            }
        }

    }

}       
