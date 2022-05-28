using EzollutionPro_BAL.Models;
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
    public class AirEGMFlightService
    {
        private static AirEGMFlightService instance = null;

        private AirEGMFlightService()
        {
        }

        public static AirEGMFlightService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirEGMFlightService();
                }
                return instance;
            }
        }

        public ResponseStatus SaveMAWB(AirEGMMAWBModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    var data = db.tblAirEGMMAWBMs.Where(z => z.iAirEGMMAWBId == model.iAirEGMMAWBId).SingleOrDefault();
                    if (data == null)
                    {
                        //if (db.tblAirEGMMAWBMs.Any(z => z.sMAWBNo == model.sMAWBNo))
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        data = new tblAirEGMMAWBM
                        {
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId,
                            sPortOfDestination = model.sPortOfDestination,
                            sMAWBNo = model.sMAWBNo,
                            sPortOfOrigin = model.sPortOfOrigin,
                            dNoOfPackages = Convert.ToDecimal(model.sNoOfPackages),
                            dTotalWeight = Convert.ToDecimal(model.sTotalWeight),
                            iAirEGMFlightDetailsId = model.iFlightId,
                            sGoodsDescription = model.sGoodsDescription,
                            cShipmentType = model.cShipmentType
                        };
                        db.tblAirEGMMAWBMs.Add(data);
                    }
                    else
                    {
                        //if (db.tblAirEGMMAWBMs.Any(z => z.iAirEGMMAWBId != model.iAirEGMMAWBId && z.sMAWBNo == model.sMAWBNo))
                        //{
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        //}
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = iUserId;
                        data.sPortOfDestination = model.sPortOfDestination;
                        data.cShipmentType = model.cShipmentType;
                        data.sMAWBNo = model.sMAWBNo;
                        data.sPortOfOrigin = model.sPortOfOrigin;
                        data.sGoodsDescription = model.sGoodsDescription;
                        data.dNoOfPackages = Convert.ToDecimal(model.sNoOfPackages);
                        data.dTotalWeight = Convert.ToDecimal(model.sTotalWeight);
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "MAWB saved successfully" };
                }
                catch (Exception ex)
                {
                    return new ResponseStatus { Status = false, Message = ex.Message };
                }
            }
        }


        public List<AirEGMFlightModel> GetFlights(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblAirEGMFlightDetailsMs.Where(z => (z.bIsReadyForTransmit ?? false) == false && (z.tblAirClientMaster.sClientName.Contains(search) || z.sFlightNo.Contains(search) || z.sEGMNo.Contains(search)));
                recordsTotal = query.Count();
                var data = query.OrderByDescending(z => z.dtActionDate)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirEGMFlightModel
                           {
                               iFlightId = z.iFlightId,
                               sClientName = z.tblAirClientMaster.sClientName,
                               sFlightNo = z.sFlightNo,
                               sEGMNo = z.sEGMNo,
                               sNo = i + 1
                           }).ToList();
                return data;
            }

        }

        public ResponseStatus ProceedToTransmit(int iFlightId)
        {
            try
            {

                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).SingleOrDefault();
                    data.bIsReadyForTransmit = true;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return new ResponseStatus { Status = true };
                }
            }
            catch (Exception)
            {
                return new ResponseStatus { Status = false, Message = "Something went wrong" };
            }
        }

        public List<AirEGMFlightModel> GetTransmittedFlights(int draw, int displayStart, int displayLength, string search, string minDate, string maxDate, out int recordsTotal)
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
                    dtMaxDate = DateTime.Now.Date;
                }
                dtMaxDate = dtMaxDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                var query = db.tblAirEGMFlightDetailsMs.Where(z => (z.dtActionDate >= dtMinDate && z.dtActionDate <= dtMaxDate) && (z.bIsReadyForTransmit ?? false) && (z.tblAirClientMaster.sClientName.Contains(search) || z.sFlightNo.Contains(search) || z.sEGMNo.Contains(search)));
                recordsTotal = query.Count();
                var data = query.OrderByDescending(z => z.dtActionDate)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirEGMFlightModel
                           {
                               iFlightId = z.iFlightId,
                               sClientName = z.tblAirClientMaster.sClientName,
                               sFlightNo = z.sFlightNo,
                               sEGMNo = z.sEGMNo,
                               sLocation = z.tblAirLocationM.sCustomLocation,
                               sDateTime = z.dtActionDate?.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                               sUserName = db.tblUserMs.Where(v => v.iUserId == z.iActionBy).Select(b => b.sFirstName + " " + b.sLastName).FirstOrDefault(),
                               sNo = i + 1
                           }).ToList();
                return data;
            }
        }

        public string GetTransmitFileData(int iFlightId, int iUserId, out string sEGMNumber, out string sPortOfOrigin)
        {
            using (var db = new EzollutionProEntities())
            {
                string mysf = "ZZ";
                string GroupSeperator = Encoding.ASCII.GetString(new byte[] { 29 });
                var FlightData = db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId== iFlightId).FirstOrDefault();
                string finalString = "";
                if (FlightData != null)
                {
                    sEGMNumber = FlightData.sEGMNo;
                    sPortOfOrigin = FlightData.sPortOfOrigin;
                    if (FlightData.sEGMNo != null)
                    {
                        finalString += "HREC" + GroupSeperator + mysf + GroupSeperator;
                        finalString += FlightData.tblAirClientMaster.sICEGateId + GroupSeperator + mysf + GroupSeperator;
                        finalString += FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += "ICES1_5" + GroupSeperator + "P" + GroupSeperator + GroupSeperator;
                        finalString += "ALCHE01" + GroupSeperator;
                        finalString += iFlightId + GroupSeperator;
                        finalString += DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += DateTime.Now.ToString("HHmm", CultureInfo.InvariantCulture);
                        finalString += "\n";
                        finalString += "<egm>" + "\n";
                        finalString += "<flightegm>" + "\n";
                        finalString += "F" + GroupSeperator;
                        finalString += FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += FlightData.sEGMNo + GroupSeperator;
                        finalString += FlightData.dtFlightDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += FlightData.sFlightNo + GroupSeperator;
                        finalString += FlightData.dtFlightDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                        finalString += FlightData.sPortOfOrigin + GroupSeperator;
                        finalString += FlightData.sPortOfDestination + GroupSeperator;
                        finalString += FlightData.sFlightRegistrationNo + GroupSeperator;
                        finalString += "N";
                        finalString += "\n";
                        finalString += "<END-flightegm>" + "\n";
                        finalString += "<mawbegm>" + "\n";
                        foreach (var item in FlightData.tblAirEGMMAWBMs)
                        {

                            finalString += "F" + GroupSeperator + FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                            finalString += FlightData.sEGMNo + GroupSeperator;
                            finalString += FlightData.dtFlightDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;    
                            finalString += item.sMAWBNo + GroupSeperator;
                            finalString +=  GroupSeperator;
                            finalString += item.sPortOfOrigin + GroupSeperator;
                            finalString += item.sPortOfDestination + GroupSeperator;
                            finalString += item.cShipmentType + GroupSeperator;
                            finalString += (int)item.dNoOfPackages + GroupSeperator;
                            finalString += String.Format("{0:0.##}", item.dTotalWeight) + GroupSeperator;
                            finalString += item.sGoodsDescription;
                            finalString += "\n";
                        }
                        finalString += "<END-mawbegm>" + "\n";
                        finalString += "<END-egm>" + "\n";
                        finalString += "TREC" + GroupSeperator + FlightData.iFlightId;
                    }
                    finalString += "\n";
                }
                else
                {
                    sPortOfOrigin = "";
                    sEGMNumber = "";
                }
                return finalString;
            }
        }

        public AirEGMFlightViewModel GetViewDataByFlightId(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).ToList().Select((z, i) => new AirEGMFlightViewModel
                {
                    iFlightId = z.iFlightId,
                    sClientName = z.tblAirClientMaster.sClientName,
                    sFlightNo = z.sFlightNo,
                    sEGMNo = z.sEGMNo,
                    sNo = i + 1,
                    lstAirEGMMAWBViewModel = z.tblAirEGMMAWBMs.Select(c => new AirEGMMAWBViewModel
                    {
                        cShipmentType = c.cShipmentType,
                        iAirEGMMAWBId = c.iAirEGMMAWBId,
                        iFlightId = c.iAirEGMFlightDetailsId ?? 0,
                        sGoodsDescription = c.sGoodsDescription,
                        sMAWBNo = c.sMAWBNo,
                        sNoOfPackages = c.dNoOfPackages ?? 0,
                        sPortOfDestination = c.sPortOfDestination,
                        sTotalWeight = c.dTotalWeight ?? 0,
                        sPortOfOrigin = c.sPortOfOrigin
                    }).ToList()
                }).SingleOrDefault();
            }
        }

        public ResponseStatus DeleteFlightDetails(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    db.tblAirEGMMAWBMs.RemoveRange(db.tblAirEGMMAWBMs.Where(z => z.iAirEGMFlightDetailsId == iFlightId));
                    db.tblAirEGMFlightDetailsMs.RemoveRange(db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId));
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "Flight Details deleted successfully" };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }

        public ResponseStatus DeleteMAWB(int iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    db.tblAirEGMMAWBMs.RemoveRange(db.tblAirEGMMAWBMs.Where(z => z.iAirEGMMAWBId == iMAWBId));
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "MAWB deleted successfully" };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }

        public ResponseStatus SaveAirEGMFlight(AirEGMFlightModel model, int iUserId,out int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == model.iFlightId).SingleOrDefault();
                if (data == null)
                {
                    data = new tblAirEGMFlightDetailsM
                    {
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        iClientId = model.iClientId,
                        iLocationId = model.iLocationId,
                        sFlightNo = model.sFlightNo,
                        dtFlightDate = DateTime.ParseExact(model.sFlightDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        sEGMNo = model.sEGMNo,
                        sFlightRegistrationNo = model.sFlightRegistrationNo,
                        sPortOfDestination = model.sPortOfDestination,
                        sPortOfOrigin = model.sPortOfOrigin,
                    };
                    db.tblAirEGMFlightDetailsMs.Add(data);
                }
                else
                {
                    data.dtActionDate = DateTime.Now;
                    data.iActionBy = iUserId;
                    data.iClientId = model.iClientId;
                    data.iLocationId = model.iLocationId;
                    data.sFlightNo = model.sFlightNo;
                    data.dtFlightDate = DateTime.ParseExact(model.sFlightDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    data.sEGMNo = model.sEGMNo;
                    data.sFlightRegistrationNo = model.sFlightRegistrationNo;
                    data.sPortOfDestination = model.sPortOfDestination;
                    data.sPortOfOrigin = model.sPortOfOrigin;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                iFlightId = data.iFlightId;
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Flight Details saved successfully!"
                };
            }
        }

        public AirEGMMAWBModel GetAirMAWBbyiMAWBId(int? iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirEGMMAWBMs.Where(z => z.iAirEGMMAWBId == iMAWBId)
                    .ToList().Select(c => new AirEGMMAWBModel
                    {
                        cShipmentType = c.cShipmentType,
                        iAirEGMMAWBId = c.iAirEGMMAWBId,
                        iFlightId = c.iAirEGMFlightDetailsId ?? 0,
                        sGoodsDescription = c.sGoodsDescription,
                        sMAWBNo = c.sMAWBNo,
                        sNoOfPackages = Convert.ToString(c.dNoOfPackages ?? 0),
                        sPortOfDestination = c.sPortOfDestination,
                        sTotalWeight = Convert.ToString(c.dTotalWeight ?? 0),
                        sPortOfOrigin = c.sPortOfOrigin,
                    }).SingleOrDefault();
            }
        }

        public AirEGMFlightModel GetAirEGMFlightById(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                var flight= db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).SingleOrDefault();
                flight.bIsReadyForTransmit = false;
                db.Entry(flight).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return db.tblAirEGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).ToList()
                       .Select(model => new AirEGMFlightModel
                       {
                           iFlightId = model.iFlightId,
                           iClientId = model.iClientId ?? 0,
                           iLocationId = model.iLocationId ?? 0,
                           sEGMNo = model.sEGMNo,
                           sFlightDate = model.dtFlightDate.GetValueOrDefault().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                           sFlightNo = model.sFlightNo,
                           sFlightRegistrationNo = model.sFlightRegistrationNo,
                           sPortOfDestination = model.sPortOfDestination,
                           sPortOfOrigin = model.sPortOfOrigin
                       }).SingleOrDefault();
            }
        }

    }
}
