using EzollutionPro_DAL;
using EzollutionPro_BAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace EzollutionPro_BAL.Services
{
    public class SeaManifestedService
    {
        private static SeaManifestedService instance = null;
        private SeaManifestedService()
        {
        }
        public static SeaManifestedService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SeaManifestedService();
                }
                return instance;
            }
        }
        public List<SchedulingViewModel> GetScheduling(string minDate, string maxDate, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                DateTime dtMinDate, dtMaxDate;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtMinDate = DateTime.Now.Date;
                    dtMaxDate = DateTime.Now.Date.AddMonths(2);
                }
                var query = from scheduling in db.tblSeaSchedulings
                            where (scheduling.dtEstimatedDateOfArrival >= dtMinDate && scheduling.dtEstimatedDateOfArrival <= dtMaxDate) && scheduling.iSAction == 4
                            select scheduling;
                recordsTotal = query.Count();
                return query.OrderBy(z => z.dtEstimatedDateOfArrival).ThenBy(z => z.tblPODMaster.sPortCode).ThenBy(z => z.sVesselName).ThenBy(z => z.tblClientMaster.sClientName).ToList()
                .Select((z, i) => new SchedulingViewModel
                {
                    sCheckListApproved = (z.bCheckListApproved ?? false) == false ? "N" : "Y",
                    sCheckListSent = (z.bCheckListSent ?? false) == false ? "N" : "Y",
                    sEDA = z.dtEstimatedDateOfArrival.HasValue ? z.dtEstimatedDateOfArrival.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    iSchedulingId = z.iSchedulingId,
                    sClientName = z.tblClientMaster.sClientName,
                    sContainerNumber = z.sContainerNumber,
                    sFPOD = z.tblPOFDMaster.sPortCode,
                    sVesselName = z.sVesselName,
                    sMBLNumber = z.sMBLNumber,
                    sPOD = z.tblPODMaster.sPortCode,
                    sShippingLine = z.tblShippingLine.sShippingLineName,
                    iSAction = z.iSAction ?? 0,
                    sRecieveOn = z.dtReceivedOn.HasValue ? z.dtReceivedOn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                }).ToList();

            }
        }

        public List<SchedulingViewModel> GetFORMIIIScheduling(string minDate, string maxDate, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                DateTime dtMinDate, dtMaxDate;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtMinDate = DateTime.Now.Date;
                    dtMaxDate = DateTime.Now.Date.AddMonths(2);
                }
                var query = from scheduling in db.tblSeaSchedulings
                            where (scheduling.dtReceivedOn >= dtMinDate && scheduling.dtReceivedOn <= dtMaxDate) && scheduling.iSAction == 2
                            select scheduling;
                recordsTotal = query.Count();
                return query.OrderBy(z => z.tblClientMaster.sClientName).ToList().Select((z, i) => new SchedulingViewModel
                {
                    iSchedulingId = z.iSchedulingId,
                    sClientName = z.tblClientMaster.sClientName,
                    sMBLNumber = z.sMBLNumber,
                }).ToList();

            }
        }
        public FormIIIModel GenerateFormIIIData(int iSchedulingId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaMBLMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(z => new FormIIIModel
                    {
                        CARNNo = z.tblSeaScheduling.tblClientMaster.sCARN,
                        IGMDate = z.dtIGMDate.HasValue ? z.dtIGMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        IGMNo = Convert.ToString(z.nIGMNo ?? 0),
                        IMOCode = z.sIMOCode,
                        MBLDate = z.dtMBLDate.HasValue ? z.dtMBLDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        MBLNumber = z.tblSeaScheduling.sMBLNumber,
                        PortOfLoading = z.tblPOSMaster.sPortName + "(" + z.tblPOSMaster.sPortCode + ")",
                        PortOfFinalDestination = z.tblSeaScheduling.tblPOFDMaster.sPortName + "(" + z.tblSeaScheduling.tblPOFDMaster.sPortCode + ")",
                        PortOfDestination = z.tblSeaScheduling.tblPODMaster.sPortName + "(" + z.tblSeaScheduling.tblPODMaster.sPortCode + ")",
                        ShippingLine = z.tblSeaScheduling.tblShippingLine.sShippingLineName,
                        VoyageNo = z.sVoyageNo,
                        CallSign = z.sVesselCode,
                        AgentName = z.tblSeaScheduling.tblClientMaster.sCompanyName
                    }).FirstOrDefault();
                    if (data != null)
                    {
                        data.lstContainerFormIIIData = db.tblSeaHBLMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(z => new ContainerFormIIIData
                        {
                            ContainerDetails = string.Join(" ",z.tblSeaContainerMasters.ToList().Select(zx=> zx.sContainerNumber + "\n" + zx.sContainerSealNo + " " + zx.sContainerStatus ).ToList()),
                            DescriptionOfGoods = z.sGoodsDescription,
                            GrossWeight = z.dGrossWeight + " " + z.sUnitofWeight,
                            HBLDate = z.dtHouseBillofLadingDate.HasValue ? z.dtHouseBillofLadingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                            HBLNo = z.sHouseBillofLadingNo,
                            LineNo = Convert.ToInt32(z.tblSeaMBLMaster.nLineNo) + "/" + z.iSubLineNo,
                            CargoMovement = (z.tblSeaMBLMaster.sCargoMovement == "TI" ? "Trans shipment\n" : "Local Cargo\n"),
                            MarksAndNumber = z.sMarksandNumbers,
                            NameOfConsigneeAndAddress = z.sImporterName + " " + z.sImporterAddress1 + " " + z.sImporterAddress2 + z.sImporterAddress3,
                            NoofPackages = z.dTotalNumberofPackages + " " + z.sPackageCode,
                        }).ToList();
                    }
                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}