using EzollutionPro_DAL;
using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.SqlClient;

namespace EzollutionPro_BAL.Services
{
    public class SeaSchedulingService
    {
        private static SeaSchedulingService instance = null;
        private SeaSchedulingService()
        {
        }
        public static SeaSchedulingService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SeaSchedulingService();
                }
                return instance;
            }
        }
        public List<SchedulingViewModel> GetScheduling(string minDate, int? iclientId, string maxDate, out int recordsTotal)
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
                    var todaysDate = DateTime.Now;
                    dtMinDate = new DateTime(todaysDate.Year, todaysDate.Month, 1);
                    dtMaxDate = DateTime.Now.Date.AddMonths(2);
                }
                var query = from scheduling in db.tblSeaSchedulings
                            where ((scheduling.dtEstimatedDateOfArrival >= dtMinDate && scheduling.dtEstimatedDateOfArrival <= dtMaxDate) && (scheduling.iSAction != 4) && (iclientId != null ? scheduling.iClientId == iclientId : 1 == 1))
                            select scheduling;
                recordsTotal = query.Count();
                return query.OrderBy(z => z.dtEstimatedDateOfArrival).ThenBy(z => z.tblPODMaster.sPortCode).ThenBy(z => z.sVesselName).ThenBy(z => z.tblClientMaster.sClientName).ToList().Select((z, i) => new SchedulingViewModel
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
                    sRemarks = z.sRemarks
                }).ToList();

            }
        }

        public CheckListPDFModel GeneratePDFData(int iSchedulingId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblSeaMBLMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList()
                           .Select(MBL => new CheckListPDFModel
                           {
                               EDA = MBL.tblSeaScheduling.dtEstimatedDateOfArrival.HasValue ? MBL.tblSeaScheduling.dtEstimatedDateOfArrival.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                               MBLDate = MBL.dtMBLDate.HasValue ? MBL.dtMBLDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                               MBLNumber = MBL.tblSeaScheduling.sMBLNumber,
                               PortOfDestination = MBL.tblSeaScheduling.tblPODMaster.sPortName + "(" + MBL.tblSeaScheduling.tblPODMaster.sPortCode + ")",
                               PortOfFinalDestination = MBL.tblSeaScheduling.tblPOFDMaster.sPortName + "(" + MBL.tblSeaScheduling.tblPOFDMaster.sPortCode + ")",
                               PortOfLoading = MBL.tblPOSMaster.sPortName + "(" + MBL.tblPOSMaster.sPortCode + ")",
                               ShippingLine = MBL.tblSeaScheduling.tblShippingLine.sShippingLineName,
                               VesselName = MBL.tblSeaScheduling.sVesselName,
                               lstHBLData = MBL.tblSeaHBLMasters.Select((HBL, i) => new HBLPDFData
                               {
                                   CargoMovement = MBL.sCargoMovement == "TI" ? "Trans shipment" : (MBL.sCargoMovement == "LC" ? "Local Cargo" : ""),
                                   GoodsDescription = HBL.sGoodsDescription,
                                   GrossWeight = HBL.dGrossWeight + " KGS",
                                   HBLDate = HBL.dtHouseBillofLadingDate.HasValue ? HBL.dtHouseBillofLadingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                                   HBLNumber = HBL.sHouseBillofLadingNo,
                                   ImporterAddress = HBL.sImporterAddress1 + " " + HBL.sImporterAddress2 + " " + HBL.sImporterAddress3,
                                   ImporterName = HBL.sImporterName,
                                   MarksAndNumbers = HBL.sMarksandNumbers,
                                   NoOfPackages = (HBL.dTotalNumberofPackages.HasValue ? HBL.dTotalNumberofPackages.Value.ToString() : "") + " " + HBL.sPackageCode,
                                   SublineNo = (i + 1).ToString(),
                                   lstContainerData = HBL.tblSeaContainerMasters.Select((Container, k) => new ContainerPDFData
                                   {
                                       ContainerNumber = Container.sContainerNumber,
                                       ContainerStatus = Container.sContainerStatus,
                                       ContainerType = Container.sISOCode == "2200" ? "20 ft" : (Container.sISOCode == "4200" ? "40 ft" : ""),
                                       ContainerWeight = Math.Round(((Container.nContainerWeight ?? 0) / 1000), 2) + " TONS",
                                       SealNumber = Container.sContainerSealNo,
                                       TotalPackages = Container.nTotalPackages.ToString()
                                   }).ToList()
                               }).ToList()
                           }).SingleOrDefault();

            }
        }

        public ResponseStatus SaveMBL(MBLData model, int iUserId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaMBLMasters.Where(z => z.iMBLId == model.iMBLId).FirstOrDefault();
                    if (data == null)
                    {
                        //if (db.tblSeaSchedulings.Any(z => z.sMBLNumber == model.sMBLNumber.Trim()))
                        //{
                        //    return new ResponseStatus
                        //    {
                        //        Status = false,
                        //        Message = "MBL Number already exists"
                        //    };
                        //}
                        data = new tblSeaMBLMaster
                        {
                            dtAddedOn = DateTime.Now,
                            dtIGMDate = string.IsNullOrEmpty(model.sIGMDate) ? (DateTime?)null : DateTime.ParseExact(model.sIGMDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            dtMBLDate = DateTime.ParseExact(model.sMBLDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            iAddedBy = iUserId,
                            iPOSId = model.iPOSId,
                            iSchedulingId = model.iSchedulingId,
                            sCargoMovement = model.sCargoMovement,
                            sCFSCode = model.sCFSCode,
                            nIGMNo = string.IsNullOrEmpty(model.sIGMNo) ? (decimal?)null : Convert.ToDecimal(model.sIGMNo),
                            nLineNo = string.IsNullOrEmpty(model.sLineNo) ? (decimal?)null : Convert.ToDecimal(model.sLineNo),
                            sIMOCode = model.sIMOCode,
                            sAddedFromIp = IP,
                            sVoyageNo = model.sVoyageNo,
                            sVesselCode = model.sVesselCode,
                        };
                        db.tblSeaMBLMasters.Add(data);
                    }
                    else
                    {

                        data.dtIGMDate = string.IsNullOrEmpty(model.sIGMDate) ? (DateTime?)null : DateTime.ParseExact(model.sIGMDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        data.dtMBLDate = DateTime.ParseExact(model.sMBLDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        data.dtModifiedOn = DateTime.Now;
                        data.iModifiedBy = iUserId;
                        data.iPOSId = model.iPOSId;
                        data.iSchedulingId = model.iSchedulingId;
                        data.sCargoMovement = model.sCargoMovement;
                        data.sCFSCode = model.sCFSCode;
                        data.sIMOCode = model.sIMOCode;
                        data.nIGMNo = string.IsNullOrEmpty(model.sIGMNo) ? (decimal?)null : Convert.ToDecimal(model.sIGMNo);
                        data.nLineNo = string.IsNullOrEmpty(model.sLineNo) ? (decimal?)null : Convert.ToDecimal(model.sLineNo);
                        data.sModifiedFromIp = IP;
                        data.sVoyageNo = model.sVoyageNo;
                        data.sVesselCode = model.sVesselCode;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "MBL saved successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public bool VerifyMBLNumber(string sMBLNumber)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblSeaSchedulings.Any(z => z.sMBLNumber == sMBLNumber);
            }
        }

        private ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }


        public DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < currentRow.Cells.Count; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        ? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResponseStatus BulkUploadHBL(DataTable dt, string sMBLNumber, int iUserId)
        {
            if (dt != null)
            {
                var db = new EzollutionProEntities();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@tblHBL", dt);
                param[1] = new SqlParameter("@sMBLNumber", sMBLNumber);
                param[2] = new SqlParameter("@iUserId", iUserId);
                var message = (string)SqlHelper.ExecuteDataset(new SqlConnection(db.Database.Connection.ConnectionString), CommandType.StoredProcedure, "uspBulkUploadHBL", param).Tables[0].Rows[0][0];
                if (message.Contains("Fail"))
                {
                    return new ResponseStatus { Status = false, Message = message.Split('\\')[1] };
                }
                return new ResponseStatus { Status = true, Message = "HBL Uploaded successfully!" };
            }
            else
            {
                return new ResponseStatus { Status = false, Message = "Excel does not contain any rows" };
            }

        }
        public ResponseStatus SaveContainer(ContainerData model, int iUserId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaContainerMasters.Where(z => z.iContainerId == model.iContainerId).FirstOrDefault();
                    if (data == null)
                    {
                        data = new tblSeaContainerMaster
                        {
                            dtAddedOn = DateTime.Now,
                            iAddedBy = iUserId,
                            sAddedFromIp = IP,
                            bStatus = true,
                            iHBLID = model.iHBLId,
                            iSchedulingId = model.iSchedulingId,
                            nContainerWeight = model.nContainerWeight,
                            nTotalPackages = model.nTotalPackages,
                            sContainerNumber = model.sContainerNumber,
                            sContainerSealNo = model.sContainerSealNumber,
                            sContainerStatus = model.sContainerStatus,
                            sISOCode = model.sISOCode,
                        };
                        db.tblSeaContainerMasters.Add(data);
                    }
                    else
                    {
                        data.bStatus = true;
                        data.iHBLID = model.iHBLId;
                        data.iSchedulingId = model.iSchedulingId;
                        data.nContainerWeight = model.nContainerWeight;
                        data.nTotalPackages = model.nTotalPackages;
                        data.sContainerNumber = model.sContainerNumber;
                        data.sContainerSealNo = model.sContainerSealNumber;
                        data.sContainerStatus = model.sContainerStatus;
                        data.sISOCode = model.sISOCode;
                        data.dtModifiedOn = DateTime.Now;
                        data.iModifiedBy = iUserId;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Container saved successfully!"
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = "Something went wrong"
                };
            }
        }

        public bool DeleteBondDetail(int iSchedulingId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaBondMasters.Where(z => z.iSchedulingId == iSchedulingId).FirstOrDefault();
                    if (data != null)
                    {
                        db.tblSeaBondMasters.Remove(data);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            { }
            return true;
        }

        public ResponseStatus SaveBondDetails(BondData model, int iUserId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaBondMasters.Where(z => z.iSchedulingId == model.iSchedulingId).FirstOrDefault();
                    if (data == null)
                    {
                        data = new tblSeaBondMaster
                        {
                            dtAddedOn = DateTime.Now,
                            iAddedBy = iUserId,
                            sAddedFromIp = IP,
                            iSchedulingId = model.iSchedulingId,
                            dBondNumber = model.dBondNumber,
                            sCarrierCode = model.sCarrierCode,
                            sMovementType = model.sMovementType
                        };
                        db.tblSeaBondMasters.Add(data);
                    }
                    else
                    {
                        data.iModifiedBy = iUserId;
                        data.sModifiedFromIp = IP;
                        data.dtModifiedOn = DateTime.Now;
                        data.dBondNumber = model.dBondNumber;
                        data.sCarrierCode = model.sCarrierCode;
                        data.sMovementType = model.sMovementType;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Bond updated successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public BondData AddUpdateBondDetails(int schedulingId, int? iBondId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var query = db.tblSeaBondMasters.Where(z => z.iBondID == iBondId);
                    if (query.Count() == 0)
                    {
                        var data = db.tblSeaSchedulings.Where(z => z.iSchedulingId == schedulingId).FirstOrDefault();
                        return new BondData
                        {
                            sMBLNumber = data.sMBLNumber,
                            iSchedulingId = schedulingId,
                            sMLOCode = data.tblShippingLine.sMLOCode,
                        };
                    }
                    else
                    {
                        return query.ToList().Select(z => new BondData
                        {
                            sMBLNumber = z.tblSeaScheduling.sMBLNumber,
                            iSchedulingId = z.iSchedulingId,
                            dBondNumber = Convert.ToInt64(z.dBondNumber ?? 0),
                            iBondID = z.iBondID,
                            sCarrierCode = z.sCarrierCode,
                            sMLOCode = z.tblSeaScheduling.tblShippingLine.sMLOCode,
                            sMovementType = z.sMovementType
                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ContainerData AddUpdateContainer(int schedulingId, int hBLId, int? iContainerId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var query = db.tblSeaContainerMasters.Where(z => z.iContainerId == iContainerId);
                    if (query.Count() == 0)
                    {
                        var data = db.tblSeaHBLMasters.Where(z => z.iHBLID == hBLId).FirstOrDefault();
                        return new ContainerData
                        {
                            sMBLNumber = data.tblSeaScheduling.sMBLNumber,
                            iHBLId = data.iHBLID,
                            iSchedulingId = schedulingId
                        };
                    }
                    else
                    {
                        return query.ToList().Select(z => new ContainerData
                        {
                            iHBLId = z.iHBLID ?? 0,
                            sMBLNumber = z.tblSeaScheduling.sMBLNumber,
                            iContainerId = z.iContainerId,
                            nContainerWeight = z.nContainerWeight,
                            nTotalPackages = z.nTotalPackages,
                            sContainerStatus = z.sContainerStatus,
                            sContainerNumber = z.sContainerNumber,
                            sContainerSealNumber = z.sContainerSealNo,
                            sISOCode = z.sISOCode,
                            iSchedulingId = z.iSchedulingId ?? 0
                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ResponseStatus DeleteHBL(int iHBLId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    if (db.tblSeaContainerMasters.Any(z => z.iHBLID == iHBLId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Container is already linked with HBL. Please delete HBL first associated with MBL."
                        };
                    }
                    else
                    {
                        var HBL = db.tblSeaHBLMasters.Where(z => z.iHBLID == iHBLId);
                        db.tblSeaHBLMasters.RemoveRange(HBL);
                        db.SaveChanges();
                        return new ResponseStatus
                        {
                            Status = true,
                            Message = "HBL has been successfully deleted."
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = "Something went wrong!"
                };
            }
        }

        public ResponseStatus DeleteContainer(int iContainerId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var Container = db.tblSeaContainerMasters.Where(z => z.iContainerId == iContainerId);
                    db.tblSeaContainerMasters.RemoveRange(Container);
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Container has been successfully deleted."
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = "Something went wrong!"
                };
            }
        }


        public ResponseStatus DeleteMBL(int iMBLId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    if (db.tblSeaHBLMasters.Any(z => z.iMBLId == iMBLId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "MBL is already linked with HBL. Please delete HBL first associated with MBL."
                        };
                    }
                    else
                    {
                        var MBL = db.tblSeaMBLMasters.Where(z => z.iMBLId == iMBLId);
                        db.tblSeaMBLMasters.RemoveRange(MBL);
                        db.SaveChanges();
                        return new ResponseStatus
                        {
                            Status = true,
                            Message = "MBL has been successfully deleted."
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = "Something went wrong!"
                };
            }
        }

        public ResponseStatus SaveHBL(HBLData model, int iUserId)
        {
            try
            {
                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblSeaHBLMasters.Where(z => z.iHBLID == model.iHBLId).FirstOrDefault();
                    var iSublineNos = db.tblSeaHBLMasters.Where(zx => zx.iSchedulingId == model.iSchedulingId).Select(z => (z.iSubLineNo ?? 0)).ToList();
                    if (data == null)
                    {
                        if (db.tblSeaHBLMasters.Any(z => z.iMBLId == model.iMBLId && z.sHouseBillofLadingNo == model.sHouseBillofLadingNo))
                        {
                            return new ResponseStatus { Status = false, Message = "HBL already exists" };
                        }
                        data = new tblSeaHBLMaster
                        {
                            dtAddedOn = DateTime.Now,
                            iSchedulingId = model.iSchedulingId,
                            iAddedBy = iUserId,
                            sAddedFromIp = IP,
                            dtHouseBillofLadingDate = DateTime.ParseExact(model.sHouseBillofLadingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            sHouseBillofLadingNo = model.sHouseBillofLadingNo,
                            iMBLId = model.iMBLId,
                            dTotalNumberofPackages = model.dTotalNumberofPackages,
                            sGoodsDescription = model.sGoodsDescription,
                            dGrossWeight = Convert.ToDecimal(model.dGrossWeight),
                            sImporterAddress1 = model.sImporterAddress1,
                            sImporterAddress2 = model.sImporterAddress2,
                            sImporterAddress3 = model.sImporterAddress3,
                            sImporterName = model.sImporterName,
                            sMarksandNumbers = model.sMarksandNumbers,
                            sPackageCode = model.sPackageCode,
                            sUnitofWeight = model.sUnitofWeight,
                            iSubLineNo = Convert.ToByte((iSublineNos.Count() == 0 ? 0 : iSublineNos.Max()) + 1)
                        };
                        db.tblSeaHBLMasters.Add(data);
                    }
                    else
                    {
                        if (db.tblSeaHBLMasters.Any(z => z.iMBLId == model.iMBLId && z.sHouseBillofLadingNo == model.sHouseBillofLadingNo && z.iHBLID != model.iHBLId))
                        {
                            return new ResponseStatus { Status = false, Message = "HBL already exists" };
                        }
                        data.dtHouseBillofLadingDate = DateTime.ParseExact(model.sHouseBillofLadingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        data.sHouseBillofLadingNo = model.sHouseBillofLadingNo;
                        data.iMBLId = model.iMBLId;
                        data.dTotalNumberofPackages = model.dTotalNumberofPackages;
                        data.sGoodsDescription = model.sGoodsDescription;
                        data.dGrossWeight = model.dGrossWeight;
                        data.sImporterAddress1 = model.sImporterAddress1;
                        data.sImporterAddress2 = model.sImporterAddress2;
                        data.sImporterAddress3 = model.sImporterAddress3;
                        data.sImporterName = model.sImporterName;
                        data.sMarksandNumbers = model.sMarksandNumbers;
                        data.iSchedulingId = model.iSchedulingId;
                        data.sPackageCode = model.sPackageCode;
                        data.sUnitofWeight = model.sUnitofWeight;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "HBL saved successfully!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public HBLData AddUpdateHBL(int iSchedulingId, int iMBLId, int? iHBLId)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblSeaHBLMasters.Where(z => z.iHBLID == iHBLId);
                if (query.Count() == 0)
                {
                    var data = db.tblSeaSchedulings.Where(z => z.iSchedulingId == iSchedulingId).FirstOrDefault();
                    return new HBLData
                    {
                        sMBLNumber = data.sMBLNumber,
                        sUnitofWeight = "KGS",
                        iMBLId = iMBLId,
                        iSchedulingId = iSchedulingId
                    };
                }
                else
                {
                    return query.ToList().Select(z => new HBLData
                    {
                        iMBLId = z.iMBLId,
                        iHBLId = z.iHBLID,
                        sMBLNumber = z.tblSeaMBLMaster.tblSeaScheduling.sMBLNumber,
                        sHouseBillofLadingDate = z.dtHouseBillofLadingDate.HasValue ? z.dtHouseBillofLadingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        dTotalNumberofPackages = z.dTotalNumberofPackages ?? 0,
                        sPackageCode = z.sPackageCode,
                        sGoodsDescription = z.sGoodsDescription,
                        dGrossWeight = z.dGrossWeight ?? 0,
                        sHouseBillofLadingNo = z.sHouseBillofLadingNo,
                        sImporterAddress1 = z.sImporterAddress1,
                        sImporterAddress2 = z.sImporterAddress2,
                        sImporterAddress3 = z.sImporterAddress3,
                        sImporterName = z.sImporterName,
                        sMarksandNumbers = z.sMarksandNumbers,
                        sUnitofWeight = z.sUnitofWeight,
                        iSchedulingId = iSchedulingId
                    }).FirstOrDefault();
                }
            }
        }

        public List<SelectListItem> GetPOSs()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPOSMasters.Where(z => z.bStatus ?? false).ToList().Select(z => new SelectListItem
                {
                    Text = z.sPortName,
                    Value = z.iPortID.ToString()
                }).ToList();
            }
        }


        public MBLData AddUpdateMBL(int iSchedulingId, int? iMBLId)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblSeaMBLMasters.Where(z => z.iMBLId == iMBLId);
                if (query.Count() == 0)
                {
                    var data = db.tblSeaSchedulings.Where(z => z.iSchedulingId == iSchedulingId).FirstOrDefault();
                    return new MBLData
                    {
                        sMBLNumber = data.sMBLNumber,
                        sPODCode = data.tblPODMaster.sPortName,
                        sPOFDCode = data.tblPOFDMaster.sPortName,
                        iSchedulingId = data.iSchedulingId
                    };
                }
                else
                {
                    return query.ToList().Select(z => new MBLData
                    {
                        sMBLNumber = z.tblSeaScheduling.sMBLNumber,
                        sMBLDate = z.dtMBLDate.HasValue ? z.dtMBLDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        iPOSId = z.iPOSId ?? 0,
                        sPODCode = z.tblSeaScheduling.tblPODMaster.sPortName,
                        sCargoMovement = z.sCargoMovement,
                        sCFSCode = z.sCFSCode,
                        sPOFDCode = z.tblSeaScheduling.tblPOFDMaster.sPortName,
                        sIGMNo = Convert.ToString(z.nIGMNo),
                        sIGMDate = z.dtIGMDate.HasValue ? z.dtIGMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        sIMOCode = z.sIMOCode,
                        sVesselCode = z.sVesselCode,
                        sVoyageNo = z.sVoyageNo,
                        sLineNo = Convert.ToString(z.nLineNo),
                        iMBLId = z.iMBLId,
                        iSchedulingId = z.iSchedulingId ?? 0
                    }).FirstOrDefault();
                }
            }
        }

        public ChecklistData GetChecklistData(int iSchedulingId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    return new ChecklistData
                    {
                        iSchedulingId = iSchedulingId,
                        lstMBLData = db.tblSeaMBLMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(z => new MBLData
                        {
                            iMBLId = z.iMBLId,
                            iSchedulingId = z.iSchedulingId ?? 0,
                            sMBLNumber = z.tblSeaScheduling.sMBLNumber,
                            sMBLDate = z.dtMBLDate.HasValue ? z.dtMBLDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                            sPOSCode = z.tblPOSMaster.sPortName,
                            sPODCode = z.tblSeaScheduling.tblPODMaster.sPortCode,
                            sPOFDCode = z.tblSeaScheduling.tblPOFDMaster.sPortCode,
                            sShippingLineMLOCode = z.tblSeaScheduling.tblShippingLine.sShippingLineName,
                            sPODName = z.tblSeaScheduling.tblPODMaster.sPortName,
                            sPOFDNames = z.tblSeaScheduling.tblPOFDMaster.sPortName
                        }).ToList(),
                        lstHBLData = db.tblSeaHBLMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(zx => new HBLData
                        {
                            iMBLId = zx.iMBLId,
                            iSchedulingId = zx.iSchedulingId ?? 0,
                            sHouseBillofLadingNo = zx.sHouseBillofLadingNo,
                            sHouseBillofLadingDate = zx.dtHouseBillofLadingDate.HasValue ? zx.dtHouseBillofLadingDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                            sImporterName = zx.sImporterName,
                            sPackageCode = zx.sPackageCode,
                            dTotalNumberofPackages = zx.dTotalNumberofPackages ?? 0,
                            dGrossWeight = Math.Round((zx.dGrossWeight ?? 0), 3),
                            sGoodsDescription = zx.sGoodsDescription,
                            iHBLId = zx.iHBLID,
                            iSublineNo = zx.iSubLineNo ?? 0
                        }).ToList(),
                        lstContainerData = db.tblSeaContainerMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(zx => new ContainerData
                        {
                            iHBLId = zx.iHBLID ?? 0,
                            iSchedulingId = zx.iSchedulingId ?? 0,
                            nContainerWeight = (zx.nContainerWeight ?? 0) / 1000,
                            nTotalPackages = zx.nTotalPackages,
                            sContainerSealNumber = zx.sContainerSealNo,
                            sContainerStatus = zx.sContainerStatus,
                            sISOCode = zx.sISOCode,
                            sMBLNumber = zx.tblSeaHBLMaster.tblSeaMBLMaster.tblSeaScheduling.sMBLNumber,
                            sContainerNumber = zx.sContainerNumber,
                            iContainerId = zx.iContainerId,
                            iSubLineNo = zx.tblSeaHBLMaster.iSubLineNo ?? 0
                        }).ToList(),
                        lstBondData = db.tblSeaBondMasters.Where(z => z.iSchedulingId == iSchedulingId).ToList().Select(zx => new BondData
                        {
                            iSchedulingId = zx.iSchedulingId,
                            sMBLNumber = zx.tblSeaScheduling.sMBLNumber,
                            dBondNumber = (long)(zx.dBondNumber ?? 0),
                            iBondID = zx.iBondID,
                            sCarrierCode = zx.sCarrierCode,
                            sMLOCode = zx.tblSeaScheduling.tblShippingLine.sMLOCode,
                            sMovementType = zx.sMovementType,
                        }).ToList(),
                    };
                }
                catch (Exception)
                {
                    throw;
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

        public ResponseStatus SaveSchedule(SchedulingViewModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                //  bool isNewRecord = false;
                var data = db.tblSeaSchedulings.Where(z => z.iSchedulingId == model.iSchedulingId).SingleOrDefault();
                if (data == null)
                {
                    var isExist = db.tblSeaSchedulings.Any(z => z.sMBLNumber == model.sMBLNumber.Trim());
                    if (isExist)
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "MBL Number already exists"
                        };
                    }
                    if (!isExist)
                    {
                        data = new tblSeaScheduling
                        {
                            iClientId = model.iClientId,
                            sMBLNumber = model.sMBLNumber,
                            iShippingID = model.iShippingId,
                            sContainerNumber = model.sContainerNumber,
                            iPortOfDestinationID = model.iPODId,
                            iPortOfFinalDestinationID = model.iFPODId,
                            sVesselName = model.sVesselName,
                            dtEstimatedDateOfArrival = model.sEDA == null ? (DateTime?)null : DateTime.ParseExact(model.sEDA, "dd/MM/yyyy", null),
                            bCheckListSent = model.sCheckListSent == "Yes",
                            bCheckListApproved = model.sCheckListApproved == "Yes",
                            iSAction = model.iSAction,
                            dtReceivedOn = model.sRecieveOn == null ? (DateTime?)null : DateTime.ParseExact(model.sRecieveOn, "dd/MM/yyyy", null),
                            sRemarks = model.sRemarks,
                            sInvoiceRemarks = model.sInvoiceRemarks,
                            iCreatedBy = iUserId,
                            dtCreatedOn = DateTime.Now,
                            sAddedFromIp = IP,
                        };
                        db.tblSeaSchedulings.Add(data);
                    }
                }
                else
                {
                    if (db.tblSeaSchedulings.Any(z => z.sMBLNumber == model.sMBLNumber && z.iSchedulingId != model.iSchedulingId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "MBL Number already exists"
                        };
                    }
                    var objCurrentValues = db.tblSeaSchedulings.FirstOrDefault(x => x.iSchedulingId == model.iSchedulingId);
                    objCurrentValues.iClientId = model.iClientId;
                    objCurrentValues.iShippingID = model.iShippingId;
                    if (iUserId == 7)
                        objCurrentValues.sMBLNumber = model.sMBLNumber;

                    objCurrentValues.sContainerNumber = model.sContainerNumber;
                    objCurrentValues.iPortOfDestinationID = model.iPODId;
                    objCurrentValues.iPortOfFinalDestinationID = model.iFPODId;
                    objCurrentValues.sVesselName = model.sVesselName;
                    objCurrentValues.dtEstimatedDateOfArrival = model.sEDA == null ? (DateTime?)null : DateTime.ParseExact(model.sEDA, "dd/MM/yyyy", null);
                    objCurrentValues.bCheckListSent = model.sCheckListSent == "Yes";
                    objCurrentValues.bCheckListApproved = model.sCheckListApproved == "Yes";
                    objCurrentValues.iSAction = model.iSAction;
                    objCurrentValues.dtReceivedOn = model.sRecieveOn == null ? (DateTime?)null : DateTime.ParseExact(model.sRecieveOn, "dd/MM/yyyy", null);
                    objCurrentValues.sRemarks = model.sRemarks;
                    objCurrentValues.sInvoiceRemarks = model.sInvoiceRemarks;
                    objCurrentValues.iModifiedBy = iUserId;
                    objCurrentValues.dtModifiedOn = DateTime.Now;
                    objCurrentValues.sModifiedFromIp = IP;
                    db.Entry(objCurrentValues).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                //if(isNewRecord==true)
                //{
                if (data.iSchedulingId > 0)
                {
                    var bondMasterObj = db.tblBondMasterMs.Where(x => x.iPODId == data.iPortOfDestinationID && x.iFPODId == data.iPortOfFinalDestinationID).FirstOrDefault();
                    if (bondMasterObj != null)
                    {
                        var bondDataObj = new BondData
                        {
                            iSchedulingId = data.iSchedulingId,
                            dBondNumber = bondMasterObj.nBondNo != null ? Convert.ToInt64(bondMasterObj.nBondNo) : 0,
                            sCarrierCode = bondMasterObj.sCarrierCode,
                            sMovementType = bondMasterObj.sModeOfTransport
                        };

                        this.SaveBondDetails(bondDataObj, iUserId);
                    }
                    var refreshdata = db.tblSeaSchedulings.Where(z => z.iSchedulingId == data.iSchedulingId).SingleOrDefault();
                    if (refreshdata.tblPODMaster != null && refreshdata.tblPOFDMaster != null)
                    {
                        if (refreshdata.tblPODMaster.sPortCode == refreshdata.tblPOFDMaster.sPortCode)
                            DeleteBondDetail(data.iSchedulingId);
                    }

                }
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Scheduling saved successfully!"
                };
            }

        }

        public SchedulingViewModel AddUdpdateSchedule(int iScheduleId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblSeaSchedulings.Where(z => z.iSchedulingId == iScheduleId).ToList().Select(z => new SchedulingViewModel
                {
                    iSchedulingId = z.iSchedulingId,
                    sEDA = z.dtEstimatedDateOfArrival.HasValue ? z.dtEstimatedDateOfArrival.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    iSAction = z.iSAction ?? 0,
                    sCheckListApproved = z.bCheckListApproved == false ? "No" : "Yes",
                    sCheckListSent = z.bCheckListSent == false ? "No" : "Yes",
                    iClientId = z.iClientId ?? 0,
                    iPODId = z.iPortOfDestinationID ?? 0,
                    iFPODId = z.iPortOfFinalDestinationID ?? 0,
                    sMBLNumber = z.sMBLNumber,
                    sContainerNumber = z.sContainerNumber,
                    sVesselName = z.sVesselName,
                    sRecieveOn = z.dtReceivedOn.HasValue ? z.dtReceivedOn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    sRemarks = z.sRemarks,
                    iShippingId = z.iShippingID ?? 0,
                    sInvoiceRemarks = z.sInvoiceRemarks,
                    iModifiedBy = z.iModifiedBy,
                    dtModifiedOn = DateTime.Now,
                    sModifiedFromIp = IP
                }).SingleOrDefault();
            }
        }

        public List<SelectListItem> GetClients(int? iClientID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientMasters.Where(z => (z.bIsActive ?? false) && (iClientID != null ? z.iClientID == iClientID.Value : 1 == 1)).ToList().Select(z => new SelectListItem
                {
                    Text = z.sClientName,
                    Value = z.iClientID.ToString()
                }).ToList();
            }
        }

        public List<SelectListItem> GetShippingLines()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblShippingLines.Where(z => z.bStatus ?? false).ToList().Select(z => new SelectListItem
                {
                    Text = z.sShippingLineName,
                    Value = z.iShippingID.ToString()
                }).ToList();
            }
        }

        public List<SelectListItem> GetPODs()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPODMasters.Where(z => z.bStatus ?? false).ToList().Select(z => new SelectListItem
                {
                    Text = z.sPortName,
                    Value = z.iPortID.ToString()
                }).ToList();
            }
        }
        public List<SelectListItem> GetFPODs()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPOFDMasters.Where(z => z.bStatus ?? false).ToList().Select(z => new SelectListItem
                {
                    Text = z.sPortName,
                    Value = z.iPortID.ToString()
                }).ToList();
            }
        }
    }
}