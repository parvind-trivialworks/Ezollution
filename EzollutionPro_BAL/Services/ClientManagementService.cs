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
    public class ClientManagementService
    {
        private static ClientManagementService instance = null;
        private ClientManagementService()
        {
        }
        public static ClientManagementService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientManagementService();
                }
                return instance;
            }
        }

        public List<ClientManagementModel> GetClientManagementList(string minDate, string maxDate, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {

                DateTime? dtMinDate = null, dtMaxDate = null;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = Convert.ToDateTime(dtMaxDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                //else
                //{
                //    var todaysDate = DateTime.Now;
                //    dtMinDate = DateTime.Now.Date.AddMonths(-1);
                //    dtMaxDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                //}
                var query = (from invoice in db.tblClientManagements
                             where invoice.blsActive == true
                             select invoice);

                if (dtMinDate != null)
                {
                    query = query.Where(x => x.dtAddedOn >= dtMinDate);
                }
                if (dtMaxDate != null)
                {
                    query = query.Where(x => x.dtAddedOn <= dtMaxDate);
                }
                recordsTotal = query.Count();
                return query.OrderBy(z => z.iClientManagementId).ToList().Select((z, i) => new ClientManagementModel
                {
                    iClientManagementId = z.iClientManagementId,
                    sClientCode = z.sClientCode,
                    dtDate = z.dtDate.FormatDate(),
                    sNewClientName = z.sNewClientName,
                    sExistingClientName = z.sExistingClientName,
                    sContactPersonName = z.sContactPersonName,
                    sContactPersonContactNo = z.sContactPersonContactNo,
                    sDecisionMakerName = z.sDecisionMakerName,
                    sDecisionMakerContactNo = z.sDecisionMakerContactNo,
                    sReference = z.sReference,
                    sBranch = z.sBranch,
                    sAddress = z.sAddress,
                    dtNextFollowUpDate = z.dtNextFollowUpDate.FormatDate(),
                    sEmailId = z.sEmailId,
                    sServiceType = z.sServiceType,
                    dRateOffered = z.dRateOffered,
                    dFinalRate = z.dFinalRate,
                    dRevenueExpected = z.dRevenueExpected,
                    dActualRevenue = z.dActualRevenue,
                    dtBusinessStartDate = z.dtBusinessStartDate.FormatDate(),
                    sAgreementImageName = z.sAgreementImageName,
                    blsActive = z.blsActive,
                    dtAddedOn = z.dtAddedOn.FormatDate()
                }).ToList();

            }
        }

        public ClientManagementModel GetClientManagement(int iClientManagementId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientManagements.Where(x => x.iClientManagementId == iClientManagementId && x.blsActive == true).ToList()
                    .Select((model, i) => new ClientManagementModel
                    {
                        iClientManagementId = model.iClientManagementId,
                        sClientCode = model.sClientCode,
                        dtDate = model.dtDate.FormatDate(),
                        sNewClientName = model.sNewClientName,
                        sExistingClientName = model.sExistingClientName,
                        sContactPersonName = model.sContactPersonName,
                        sContactPersonContactNo = model.sContactPersonContactNo,
                        sDecisionMakerName = model.sDecisionMakerName,
                        sDecisionMakerContactNo = model.sDecisionMakerContactNo,
                        sReference = model.sReference,
                        sBranch = model.sBranch,
                        sAddress = model.sAddress,
                        dtNextFollowUpDate = model.dtNextFollowUpDate.FormatDate(),
                        sEmailId = model.sEmailId,
                        sServiceType = model.sServiceType,
                        dRateOffered = model.dRateOffered,
                        dFinalRate = model.dFinalRate,
                        dRevenueExpected = model.dRevenueExpected,
                        dActualRevenue = model.dActualRevenue,
                        dtBusinessStartDate = model.dtBusinessStartDate.FormatDate(),
                        sAgreementImageName = model.sAgreementImageName,
                        blsActive = true
                    }).FirstOrDefault();
            }
        }

        public int AddUpdateClientManagement(ClientManagementModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblClientManagements.Where(z => z.iClientManagementId == model.iClientManagementId).FirstOrDefault();
                if (data != null)
                {
                    data.dtDate = model.dtDate.ConvertDate();
                    data.sNewClientName = model.sNewClientName;
                    data.sExistingClientName = model.sExistingClientName;
                    data.sContactPersonName = model.sContactPersonName;
                    data.sContactPersonContactNo = model.sContactPersonContactNo;
                    data.sDecisionMakerName = model.sDecisionMakerName;
                    data.sDecisionMakerContactNo = model.sDecisionMakerContactNo;
                    data.sReference = model.sReference;
                    data.sBranch = model.sBranch;
                    data.sAddress = model.sAddress;
                    data.dtNextFollowUpDate = model.dtNextFollowUpDate.ConvertDate();
                    data.sEmailId = model.sEmailId;
                    data.sServiceType = model.sServiceType;
                    data.dRateOffered = model.dRateOffered;
                    data.dFinalRate = model.dFinalRate;
                    data.dRevenueExpected = model.dRevenueExpected;
                    data.dActualRevenue = model.dActualRevenue;
                    data.dtBusinessStartDate = model.dtBusinessStartDate.ConvertDate();
                    data.sAgreementImageName = model.sAgreementImageName;
                    data.dtModifiedOn = DateTime.Now;
                    data.iModifiedBy = iUserId;
                    data.blsActive = model.blsActive;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return data.iClientManagementId;
                }
                else
                {
                    int ClientCount = db.tblClientManagements.Count() + 1;
                    int CurrentYear = System.DateTime.Now.Year;
                    string ClientCode = "";
                    if (ClientCount < 10)
                    {
                        ClientCode = "0000" + ClientCount.ToString();
                    }
                    else if (ClientCount < 100)
                    {
                        ClientCode = "000" + ClientCount.ToString();
                    }
                    else if (ClientCount < 1000)
                    {
                        ClientCode = "00" + ClientCount.ToString();
                    }
                    else if (ClientCount < 10000)
                    {
                        ClientCode = "0" + ClientCount.ToString();
                    }
                    else
                    {
                        ClientCode = "" + ClientCount.ToString();
                    }

                    ClientCode = "EZS" + ClientCode;
                    data = new tblClientManagement
                    {
                        dtAddedOn = DateTime.Now,
                        iAddedBy = iUserId,
                        sAddedFromIp = IP,
                        sClientCode = ClientCode,
                        dtDate = model.dtDate.ConvertDate(),
                        sNewClientName = model.sNewClientName,
                        sExistingClientName = model.sExistingClientName,
                        sContactPersonName = model.sContactPersonName,
                        sContactPersonContactNo = model.sContactPersonContactNo,
                        sDecisionMakerName = model.sDecisionMakerName,
                        sDecisionMakerContactNo = model.sDecisionMakerContactNo,
                        sReference = model.sReference,
                        sBranch = model.sBranch,
                        sAddress = model.sAddress,
                        dtNextFollowUpDate = model.dtNextFollowUpDate.ConvertDate(),
                        sEmailId = model.sEmailId,
                        sServiceType = model.sServiceType,
                        dRateOffered = model.dRateOffered,
                        dFinalRate = model.dFinalRate,
                        dRevenueExpected = model.dRevenueExpected,
                        dActualRevenue = model.dActualRevenue,
                        dtBusinessStartDate = model.dtBusinessStartDate.ConvertDate(),
                        sAgreementImageName = model.sAgreementImageName,
                        blsActive = true,
                    };
                    db.tblClientManagements.Add(data);
                    db.SaveChanges();
                    return data.iClientManagementId;
                }
            }
        }

        public List<ClientManagementFollowupModel> GetClientManagementFollowupsList(int iClientManagementId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientManagementFollowups.Where(x => x.iClientManagementId == iClientManagementId && x.blsActive == true).ToList()
                    .Select((z, i) => new ClientManagementFollowupModel
                    {
                        iClientManagementFollowupId=z.iClientManagementFollowupId,
                        iClientManagementId = z.iClientManagementId.GetValueOrDefault(),
                        sFollowUpType = z.sFollowUpType,
                        sDifference = z.sDifference,
                        sRemark = z.sRemark,
                        sManagementRemark = z.sManagementRemark,
                        dRateOffered = z.dRateOffered,
                        dFinalRate = z.dFinalRate,
                        dRevenueExpected = z.dRevenueExpected,
                        dActualRevenue = z.dActualRevenue,
                        dtNextFollowUpDate = z.dtNextFollowUpDate.FormatDate(),
                        blsActive=z.blsActive,
                        dtAddedOn=z.dtAddedOn.FormatDate()

                    }).ToList();

            }
        }
        public ClientManagementFollowupModel GetClientManagementFollowup(int iClientManagementFollowupId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientManagementFollowups.Where(x => x.iClientManagementFollowupId == iClientManagementFollowupId).ToList()
                .Select((z, i) => new ClientManagementFollowupModel
                {
                    iClientManagementFollowupId=z.iClientManagementFollowupId,
                    iClientManagementId = z.iClientManagementId.GetValueOrDefault(),
                    sFollowUpType = z.sFollowUpType,
                    sDifference = z.sDifference,
                    sRemark = z.sRemark,
                    sManagementRemark = z.sManagementRemark,
                    dRateOffered = z.dRateOffered,
                    dFinalRate = z.dFinalRate,
                    dRevenueExpected = z.dRevenueExpected,
                    dActualRevenue = z.dActualRevenue,
                    dtNextFollowUpDate = z.dtNextFollowUpDate.FormatDate(),
                    blsActive = z.blsActive.GetValueOrDefault()
                }).FirstOrDefault();
            }
        }
        public ResponseStatus AddUdpdateClientManagementFollowup(ClientManagementFollowupModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {

                var data = db.tblClientManagementFollowups.Where(z => z.iClientManagementFollowupId == model.iClientManagementFollowupId).FirstOrDefault();
                if (data != null)
                {
                    data.iClientManagementId = model.iClientManagementId;
                    data.sFollowUpType = model.sFollowUpType;
                    data.sDifference = model.sDifference;
                    data.sRemark = model.sRemark;
                    data.sManagementRemark = model.sManagementRemark;
                    data.dRateOffered = model.dRateOffered;
                    data.dFinalRate = model.dFinalRate;
                    data.dRevenueExpected = model.dRevenueExpected;
                    data.dActualRevenue = model.dActualRevenue;
                    data.dtNextFollowUpDate = model.dtNextFollowUpDate.ConvertDate();
                    data.blsActive = model.blsActive;
                    data.dtModifiedOn = DateTime.Now;
                    data.iModifiedBy = iUserId;
                    data.blsActive = model.blsActive;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Followup details updated successfully!"
                    };
                }
                else
                {
                   
                    data = new tblClientManagementFollowup
                    {
                        iClientManagementId = model.iClientManagementId,
                        sFollowUpType = model.sFollowUpType,
                        sDifference = model.sDifference,
                        sRemark = model.sRemark,
                        sManagementRemark = model.sManagementRemark,
                        dRateOffered = model.dRateOffered,
                        dFinalRate = model.dFinalRate,
                        dRevenueExpected = model.dRevenueExpected,
                        dActualRevenue = model.dActualRevenue,
                        dtNextFollowUpDate = model.dtNextFollowUpDate.ConvertDate(),
                        blsActive = true,
                        dtAddedOn = DateTime.Now,
                        iAddedBy = iUserId,
                        sAddedFromIp = IP,
                    };
                    db.tblClientManagementFollowups.Add(data);
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Followup details added successfully!"
                    };
                }
                
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

    }
            
}       
