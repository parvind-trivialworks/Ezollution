using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Services
{
    public class MAWBService
    {
        private static MAWBService instance = null;

        private MAWBService()
        {
        }

        public static MAWBService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MAWBService();
                }
                return instance;
            }
        }

        public object GetScheduling(string minDate, string maxDate, int displayStart, int displayLength, string search, UserModel userModel, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                DateTime dtMinDate, dtMaxDate;
                var transmited = (byte)AirSchedulingEnum.Transmited;
                var proceedToTransmit = (byte)AirSchedulingEnum.ProceedToTransmit;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtMinDate = DateTime.Now.Date.AddMonths(-2);
                    dtMaxDate = DateTime.Now.Date;
                }
                dtMaxDate = dtMaxDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                var query = from scheduling in db.tblAirMAWBMs
                            where (scheduling.dtActionDate >= dtMinDate && scheduling.dtActionDate <= dtMaxDate)
                            && (scheduling.tblAirClientMaster.sClientName.Contains(search) || scheduling.sMAWBNo.Contains(search))
                            && (scheduling.iStatus != transmited && scheduling.iStatus != proceedToTransmit)
                            && (userModel.bIsClient == false || scheduling.iActionBy == userModel.iUserId)
                            select scheduling;
                recordsTotal = query.Count();
                return query.OrderByDescending(z => z.dtActionDate).Skip(displayStart).Take(displayLength).ToList().Select((z, i) => new
                {
                    SNo = i + 1,
                    z.sMAWBNo,
                    z.tblAirClientMaster.sClientName,
                    z.iMAWBId,
                    bIsScheduled = (z.nPackages == z.tblAirHAWBMs.Sum(c => c.nPackages) && z.nWeight == z.tblAirHAWBMs.Sum(c => c.nWeight))
                }).ToList();

            }
        }



        public List<DropDownData> GetCustomLocations()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirLocationMs.Select(z => new DropDownData
                {
                    Text = z.sCustomLocation,
                    Value = z.sThreeLetterCode,
                    Id = z.iLocationId
                }).ToList();
            }
        }

        public List<DropDownData> GetAirClients()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirClientMasters.Select(z => new DropDownData
                {
                    Text = z.sClientName,
                    Value = z.sCARNNo,
                    Id = z.iAirClientId
                }).ToList();
            }
        }

        public ResponseStatus SaveMAWB(MAWBModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    var data = db.tblAirMAWBMs.Where(z => z.iMAWBId == model.iMAWBId).SingleOrDefault();
                    if (db.tblAirClientMasters.Any(z => z.iAirClientId == model.iAirClientId && z.sCARNNo != model.sCARNNo))
                    {
                        var airClient = db.tblAirClientMasters.Where(z => z.iAirClientId == model.iAirClientId).SingleOrDefault();
                        airClient.sCARNNo = model.sCARNNo;
                        db.Entry(airClient).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (data == null)
                    {
                        //if (db.tblAirMAWBMs.Any(z => z.sMAWBNo == model.sMAWBNo))
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        data = new tblAirMAWBM
                        {
                            dtFlightDate = string.IsNullOrEmpty(model.sFlightDate) ? (DateTime?)null : DateTime.ParseExact(model.sFlightDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            dtActionDate = DateTime.Now,
                            dtIGMDate = string.IsNullOrEmpty(model.sIGMDate) ? (DateTime?)null : DateTime.ParseExact(model.sIGMDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            iActionBy = iUserId,
                            sDestination = model.sDestination,
                            sFlightNumber = model.sFlightNumber,
                            sIGMNumber = model.sIGMNumber,
                            sMAWBNo = model.sMAWBNo,
                            sOrigin = model.sOrigin,
                            nPackages = Convert.ToDecimal(model.sPackages),
                            nWeight = Convert.ToDecimal(model.sWeight),
                            iAirClientId = model.iAirClientId,
                            iLocationId = model.iLocationId,
                            iStatus = (byte)AirSchedulingEnum.Scheduled
                        };
                        db.tblAirMAWBMs.Add(data);
                    }
                    else
                    {
                        //if (db.tblAirMAWBMs.Any(z => z.iMAWBId != model.iMAWBId && z.sMAWBNo == model.sMAWBNo))
                        //{
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        //}
                        data.iLocationId = model.iLocationId;
                        data.dtFlightDate = string.IsNullOrEmpty(model.sFlightDate) ? (DateTime?)null : DateTime.ParseExact(model.sFlightDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        data.dtUpdateDate = DateTime.Now;
                        data.dtIGMDate = string.IsNullOrEmpty(model.sIGMDate) ? (DateTime?)null : DateTime.ParseExact(model.sIGMDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        data.iUpdatedBy = iUserId;
                        data.sDestination = model.sDestination;
                        data.sFlightNumber = model.sFlightNumber;
                        data.sIGMNumber = model.sIGMNumber;
                        data.sMAWBNo = model.sMAWBNo;
                        data.iAirClientId = model.iAirClientId;
                        data.sOrigin = model.sOrigin;
                        data.nPackages = Convert.ToDecimal(model.sPackages);
                        data.nWeight = Convert.ToDecimal(model.sWeight);
                        data.iStatus = (byte)AirSchedulingEnum.Scheduled;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    model.iMAWBId = data.iMAWBId;
                    return new ResponseStatus { Status = true, Message = "MAWB saved successfully" };
                }
                catch (Exception ex)
                {
                    return new ResponseStatus { Status = false, Message = ex.Message };
                }
            }
        }

        public object ProceedToTransmit(int iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                var mawb = db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId).SingleOrDefault();
                if (mawb == null)
                    return new { Status = false, Message = "MAWB does not exists" };
                else
                {
                    mawb.iStatus = (byte)AirSchedulingEnum.ProceedToTransmit;
                    mawb.dtActionDate = DateTime.Now;
                    db.Entry(mawb).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return new { Status = true };
                }
            }
        }

        public string GetTransmitFileData(long iMAWBId, int iUserId, out string sMAWBNumber)
        {
            using (var db = new EzollutionProEntities())
            {
                string mysf = "ZZ";
                string GroupSeperator = Encoding.ASCII.GetString(new byte[] { 29 });
                var MAWBData = db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId).FirstOrDefault();
                string finalString = "";
                if (MAWBData != null)
                {
                    sMAWBNumber = MAWBData.sMAWBNo;
                    if (MAWBData.sIGMNumber != null)
                    {
                        finalString += "HREC" + GroupSeperator + mysf + GroupSeperator;
                        finalString += MAWBData.tblAirClientMaster.sICEGateId + GroupSeperator + mysf + GroupSeperator;
                        finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += "ICES1_5" + GroupSeperator + "P" + GroupSeperator + GroupSeperator;
                        finalString += "CMCHI01" + GroupSeperator;
                        finalString += MAWBData.iMAWBId + GroupSeperator;
                        finalString += DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += DateTime.Now.ToString("HHmm", CultureInfo.InvariantCulture);
                        finalString += "\n";
                        finalString += "<consoligm>" + "\n";
                        finalString += "<consmaster>" + "\n";
                        finalString += "F" + GroupSeperator + MAWBData.tblAirClientMaster.sCARNNo + GroupSeperator;
                        finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += MAWBData.sIGMNumber + GroupSeperator;
                        finalString += MAWBData.dtIGMDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += MAWBData.sFlightNumber + GroupSeperator;
                        finalString += MAWBData.dtFlightDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += MAWBData.sMAWBNo + GroupSeperator;
                        finalString += MAWBData.dtIGMDate?.AddDays(-2).ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += MAWBData.sOrigin + GroupSeperator;
                        finalString += MAWBData.sDestination + GroupSeperator;
                        finalString += "T" + GroupSeperator;
                        finalString += MAWBData.nPackages + GroupSeperator;
                        finalString += String.Format("{0:0.##}", MAWBData.nWeight) + GroupSeperator;
                        finalString += "CONSOL";
                        finalString += "\n";
                        finalString += "<END-consmaster>" + "\n";
                        finalString += "<conshouse>" + "\n";
                        foreach (var item in MAWBData.tblAirHAWBMs)
                        {

                            finalString += "F" + GroupSeperator + MAWBData.tblAirClientMaster.sCARNNo + GroupSeperator;
                            finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator;
                            finalString += MAWBData.sIGMNumber + GroupSeperator;
                            finalString += MAWBData.dtIGMDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                            finalString += MAWBData.sFlightNumber + GroupSeperator;
                            finalString += MAWBData.dtFlightDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                            finalString += MAWBData.sMAWBNo + GroupSeperator;
                            finalString += MAWBData.dtIGMDate?.AddDays(-2).ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                            finalString += item.sHAWBNo + GroupSeperator;
                            finalString += MAWBData.dtIGMDate?.AddDays(-2).ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                            finalString += item.sOrigin + GroupSeperator;
                            finalString += item.sDestination + GroupSeperator;
                            finalString += "T" + GroupSeperator;
                            finalString += item.nPackages + GroupSeperator;
                            finalString += String.Format("{0:0.##}", item.nWeight) + GroupSeperator;
                            finalString += item.sDescription;
                            finalString += "\n";
                        }
                        finalString += "<END-conshouse>" + "\n";
                        finalString += "<END-consoligm>" + "\n";
                        finalString += "TREC" + GroupSeperator + MAWBData.iMAWBId;
                    }
                    else
                    {
                        //Without IGM Details
                        finalString += "HREC" + GroupSeperator + mysf + GroupSeperator;
                        finalString += MAWBData.tblAirClientMaster.sICEGateId + GroupSeperator + mysf + GroupSeperator;
                        finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += "ICES1_5" + GroupSeperator + "P" + GroupSeperator + GroupSeperator;
                        finalString += "CMCHI01" + GroupSeperator;
                        finalString += MAWBData.iMAWBId + GroupSeperator;
                        finalString += DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += DateTime.Now.ToString("HHmm", CultureInfo.InvariantCulture);
                        finalString += "\n";
                        finalString += "<consoligm>" + "\n";
                        finalString += "<consmaster>" + "\n";
                        finalString += "F" + GroupSeperator + MAWBData.tblAirClientMaster.sCARNNo + GroupSeperator;
                        finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator + GroupSeperator + GroupSeperator + GroupSeperator + GroupSeperator;
                        finalString += MAWBData.sMAWBNo + GroupSeperator + GroupSeperator;
                        finalString += MAWBData.sOrigin + GroupSeperator;
                        finalString += MAWBData.sDestination + GroupSeperator;
                        finalString += "T" + GroupSeperator;
                        finalString += MAWBData.nPackages + GroupSeperator;
                        finalString += String.Format("{0:0.##}", MAWBData.nWeight) + GroupSeperator;
                        finalString += "CONSOL";
                        finalString += "\n";
                        finalString += "<END-consmaster>" + "\n";
                        finalString += "<conshouse>" + "\n";
                        foreach (var item in MAWBData.tblAirHAWBMs)
                        {
                            finalString += "F" + GroupSeperator + MAWBData.tblAirClientMaster.sCARNNo + GroupSeperator;
                            finalString += MAWBData.tblAirLocationM.sCustomCode + GroupSeperator + GroupSeperator + GroupSeperator + GroupSeperator + GroupSeperator;
                            finalString += MAWBData.sMAWBNo + GroupSeperator + GroupSeperator;
                            finalString += item.sHAWBNo + GroupSeperator + GroupSeperator;
                            finalString += item.sOrigin + GroupSeperator;
                            finalString += item.sDestination + GroupSeperator;
                            finalString += "T" + GroupSeperator;
                            finalString += item.nPackages + GroupSeperator;
                            finalString += String.Format("{0:0.##}", item.nWeight) + GroupSeperator;
                            finalString += item.sDescription;
                            finalString += "\n";
                        }
                        finalString += "<END-conshouse>" + "\n";
                        finalString += "<END-consoligm>" + "\n";
                        finalString += "TREC" + GroupSeperator + MAWBData.iMAWBId;
                    }
                    finalString += "\n";
                    MAWBData.iActionBy = iUserId;
                    MAWBData.dtActionDate = DateTime.Now;
                    MAWBData.iStatus = (byte)AirSchedulingEnum.Transmited;
                    db.Entry(MAWBData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                    sMAWBNumber = "";
                return finalString;
            }
        }

        public ResponseStatus DeleteMAWB(long iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    db.tblAirHAWBMs.RemoveRange(db.tblAirHAWBMs.Where(z => z.iMAWBId == iMAWBId));
                    db.tblAirMAWBMs.RemoveRange(db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId));
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "MAWB deleted successfully" };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }

        public ResponseStatus DeleteHAWB(long iHAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    db.tblAirHAWBMs.RemoveRange(db.tblAirHAWBMs.Where(z => z.iHAWBId == iHAWBId));
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "MAWB deleted successfully" };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }

        public MAWBModel GetMAWBbyId(long iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId).ToList().Select(z => new MAWBModel
                {
                    sFlightDate = z.dtFlightDate.HasValue ? z.dtFlightDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    sIGMDate = z.dtIGMDate.HasValue ? z.dtIGMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    iMAWBId = z.iMAWBId,
                    sDestination = z.sDestination,
                    sOrigin = z.sOrigin,
                    sPackages = Convert.ToString(z.nPackages ?? 0M),
                    sWeight = Convert.ToString(Math.Round(z.nWeight ?? 0M, 2)),
                    sMAWBNo = z.sMAWBNo,
                    sIGMNumber = z.sIGMNumber,
                    sFlightNumber = z.sFlightNumber,
                    iAirClientId = z.iAirClientId ?? 0,
                    iLocationId = z.iLocationId ?? 0,
                    sCARNNo = z.tblAirClientMaster.sCARNNo
                }).SingleOrDefault();
            }
        }

        public HAWBModel GetHAWBbyId(long iHAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirHAWBMs.Where(z => z.iHAWBId == iHAWBId).ToList().Select(z => new HAWBModel
                {
                    iHAWBId = z.iHAWBId,
                    sDestination = z.sDestination,
                    sOrigin = z.sOrigin,
                    sPackages = Convert.ToString(z.nPackages ?? 0M),
                    sWeight = Convert.ToString(Math.Round(z.nWeight ?? 0M, 2)),
                    sHAWBNo = z.sHAWBNo,
                    iMAWBId = z.iMAWBId ?? 0,
                    sDescription = z.sDescription
                }).SingleOrDefault();
            }
        }

        public MAWBMaster GetMAWBMaster(long iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId).Select(z => new MAWBMaster
                {
                    iMAWBId = z.iMAWBId,
                    sDestination = z.sDestination,
                    sOrigin = z.sOrigin,
                    sPackages = (z.nPackages ?? 0M),
                    sWeight = (Math.Round(z.nWeight ?? 0M, 2)),
                    sMAWBNo = z.sMAWBNo,
                    lstHAWBMasters = z.tblAirHAWBMs.Select(c => new HAWBMaster
                    {
                        iHAWBId = c.iHAWBId,
                        sDestination = c.sDestination,
                        sOrigin = c.sOrigin,
                        sPackages = (c.nPackages ?? 0M),
                        sWeight = (Math.Round(c.nWeight ?? 0M, 2)),
                        sHAWBNo = c.sHAWBNo,
                        sDescription = c.sDescription
                    }).ToList()
                }).SingleOrDefault();
            }
        }

        public ResponseStatus SaveHAWB(HAWBModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    var data = db.tblAirHAWBMs.Where(z => z.iHAWBId == model.iHAWBId).SingleOrDefault();
                    if (data == null)
                    {
                        if (db.tblAirHAWBMs.Any(z => z.sHAWBNo == model.sHAWBNo && model.iMAWBId == z.iMAWBId))
                            return new ResponseStatus { Status = false, Message = "HAWB No already exists" };
                        data = new tblAirHAWBM
                        {
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId,
                            sHAWBNo = model.sHAWBNo,
                            nPackages = Convert.ToDecimal(model.sPackages),
                            nWeight = Convert.ToDecimal(model.sWeight),
                            sDescription = model.sDescription,
                            iMAWBId = model.iMAWBId,
                            sOrigin = model.sOrigin,
                            sDestination = model.sDestination
                        };
                        db.tblAirHAWBMs.Add(data);
                    }
                    else
                    {
                        if (db.tblAirHAWBMs.Any(z => z.iHAWBId != model.iHAWBId && z.sHAWBNo == model.sHAWBNo && model.iMAWBId == z.iMAWBId))
                        {
                            return new ResponseStatus { Status = false, Message = "HAWB No already exists" };
                        }
                        data.sOrigin = model.sOrigin;
                        data.sDestination = model.sDestination;
                        data.sDescription = model.sDescription;
                        data.iMAWBId = model.iMAWBId;
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = iUserId;
                        data.sHAWBNo = model.sHAWBNo;
                        data.nPackages = Convert.ToDecimal(model.sPackages);
                        data.nWeight = Convert.ToDecimal(model.sWeight);
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "HAWB saved successfully" };
                }
                catch (Exception ex)
                {
                    return new ResponseStatus { Status = false, Message = ex.Message };
                }
            }
        }

        public object GetTransmitted(string minDate, string maxDate, int displayStart, int displayLength, string search, UserModel userInfo, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                DateTime dtMinDate, dtMaxDate;
                var ProceedToTransmit = (byte)AirSchedulingEnum.ProceedToTransmit;
                var transmitted = (byte)AirSchedulingEnum.Transmited;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtMinDate = DateTime.Now.Date;
                    dtMaxDate = DateTime.Now.Date;
                }
                dtMaxDate = dtMaxDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                if (userInfo.iClientID.HasValue && userInfo.bIsClient)
                {
                  
                var query = from scheduling in db.tblAirMAWBMs
                            where (scheduling.dtActionDate >= dtMinDate && scheduling.dtActionDate <= dtMaxDate)
                            && (scheduling.tblAirClientMaster.sClientName.Contains(search) || scheduling.sMAWBNo.Contains(search))
                            && (scheduling.iStatus == transmitted)
                            && scheduling.iAirClientId == userInfo.iClientID
                            select scheduling;
                    recordsTotal = query.Count();
                    return query.OrderByDescending(z => z.dtActionDate).Skip(displayStart).Take(displayLength).ToList().Select((z, i) => new
                    {
                        SNo = i + 1,
                        z.sMAWBNo,
                        z.tblAirClientMaster.sClientName,
                        z.tblAirLocationM.sCustomLocation,
                        sActionDate = z.dtActionDate?.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                        username = db.tblUserMs.Where(c => c.iUserId == z.iActionBy).Select(c => new { c.sFirstName, c.sLastName }).ToList().Select(c => c.sFirstName + " " + c.sLastName).SingleOrDefault(),
                        z.iMAWBId,
                        bIsTransmitted = (z.iStatus == transmitted),
                        bIsScheduled = (z.nPackages == z.tblAirHAWBMs.Sum(c => c.nPackages) && z.nWeight == z.tblAirHAWBMs.Sum(c => c.nWeight)),
                        bIsAirClient = userInfo.iClientID.HasValue ? true : false
                    }).ToList();
                }
                else
                {                  
                var query = from scheduling in db.tblAirMAWBMs
                            where (scheduling.dtActionDate >= dtMinDate && scheduling.dtActionDate <= dtMaxDate)
                            && (scheduling.tblAirClientMaster.sClientName.Contains(search) || scheduling.sMAWBNo.Contains(search))
                            && (scheduling.iStatus == transmitted || scheduling.iStatus == ProceedToTransmit)
                            select scheduling;
                    recordsTotal = query.Count();
                    return query.OrderByDescending(z => z.dtActionDate).Skip(displayStart).Take(displayLength).ToList().Select((z, i) => new
                    {
                        SNo = i + 1,
                        z.sMAWBNo,
                        z.tblAirClientMaster.sClientName,
                        z.tblAirLocationM.sCustomLocation,
                        sActionDate = z.dtActionDate?.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                        username = db.tblUserMs.Where(c => c.iUserId == z.iActionBy).Select(c => new { c.sFirstName, c.sLastName }).ToList().Select(c => c.sFirstName + " " + c.sLastName).SingleOrDefault(),
                        z.iMAWBId,
                        bIsTransmitted = (z.iStatus == transmitted),
                        bIsScheduled = (z.nPackages == z.tblAirHAWBMs.Sum(c => c.nPackages) && z.nWeight == z.tblAirHAWBMs.Sum(c => c.nWeight)),
                        bIsAirClient = userInfo.iClientID.HasValue ? true : false
                    }).ToList();
                }
              

            }
        }

        public CheckListMAWBPDFModel GeneratePDFData(int iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirMAWBMs.Where(z => z.iMAWBId == iMAWBId).ToList()
                           .Select(MAWB => new CheckListMAWBPDFModel
                           {
                               sCustomLocation = MAWB.tblAirLocationM.sCustomLocation,
                               sCustomLocationCode = MAWB.tblAirLocationM.sCustomCode,
                               sMAWBNo = MAWB.sMAWBNo,
                               sPackages = String.Format("{0:0.##}", MAWB.nPackages),
                               sWeight = String.Format("{0:0.##}", MAWB.nWeight),
                               sPortOfDestination = MAWB.sDestination,
                               sPortOfOrigin = MAWB.sOrigin,
                               sDescription = "CONSOL",
                               sTotalPackages = String.Format("{0:0.##}", MAWB.tblAirHAWBMs.Sum(z => z.nPackages)),
                               sTotalWeight = String.Format("{0:0.##}", MAWB.tblAirHAWBMs.Sum(z => z.nWeight)),
                               lstHAWBData = MAWB.tblAirHAWBMs.Select((HAWB, i) => new CheckListHAWBPDFModel
                               {
                                   iSno = i + 1,
                                   sDescription = HAWB.sDescription,
                                   sHAWBNo = HAWB.sHAWBNo,
                                   sPackages = String.Format("{0:0.##}", HAWB.nPackages),
                                   sWeight = String.Format("{0:0.##}", HAWB.nWeight),
                                   sPortOfDestination = HAWB.sDestination,
                                   sPortOfOrigin = HAWB.sOrigin,
                               }).ToList()
                           }).SingleOrDefault();

            }
        }


        public ResponseStatus ValidateMAWB(string PageName, string MAWBNo)
        {
            using (var db = new EzollutionProEntities())
            {
                if (PageName == "MAWB")
                {
                    if (db.tblAirMAWBMs.Any(z => z.sMAWBNo == MAWBNo))
                    {
                        return new ResponseStatus { Status = false, Message = "MAWB No already exists." };
                    }
                    else
                    {

                    }
                }
                else if (PageName == "AirEGMFlight")
                {
                    if (db.tblAirEGMMAWBMs.Any(z => z.sMAWBNo == MAWBNo))
                    {
                        return new ResponseStatus { Status = false, Message = "MAWB No already exists." };
                    }
                    else
                    {

                    }
                }
                else if (PageName == "AirIGMFlight")
                {
                    
                    if (db.tblAirIGMMAWBMs.Any(z => z.sMAWBNo == MAWBNo))
                    {
                        return new ResponseStatus { Status = false, Message = "MAWB No already exists." };
                    }
                    else
                    {

                    }
                }
                
                return new ResponseStatus { Status = true, Message = "" };
            }
        }
    }
}
