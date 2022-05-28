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
    public class AirIGMFlightService
    {
        private static AirIGMFlightService instance = null;

        private AirIGMFlightService()
        {
        }

        public static AirIGMFlightService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirIGMFlightService();
                }
                return instance;
            }
        }

        public string GetTransmitFileData(int iFlightId, int iUserId, out string sFlightNumber)
        {
            using (var db = new EzollutionProEntities())
            {
                string mysf = "ZZ";
                string GroupSeperator = Encoding.ASCII.GetString(new byte[] { 29 });
                var FlightData = db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).FirstOrDefault();
                string finalString = "";
                if (FlightData != null)
                {
                    sFlightNumber = FlightData.sFlightNo;
                    finalString += "HREC" + GroupSeperator + mysf + GroupSeperator;
                    finalString += FlightData.tblAirClientMaster.sICEGateId + GroupSeperator + mysf + GroupSeperator;
                    finalString += FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                    finalString += "ICES1_5" + GroupSeperator + "P" + GroupSeperator + GroupSeperator;
                    finalString += "ALCHI01" + GroupSeperator;
                    finalString += iFlightId + GroupSeperator;
                    finalString += DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + GroupSeperator;
                    finalString += DateTime.Now.ToString("HHmm", CultureInfo.InvariantCulture);
                    finalString += "\n";
                    finalString += "<igm>" + "\n";
                    finalString += "<flightigm>" + "\n";
                    finalString += "F" + GroupSeperator;
                    finalString += FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                    finalString += FlightData.sFlightNo + GroupSeperator;
                    finalString += FlightData.dtDepartureDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator;
                    finalString += FlightData.dtArrivalDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + " ";
                    finalString += new DateTime().Add(FlightData.tTime ?? new TimeSpan()).ToString("HH:mm", CultureInfo.InvariantCulture) + GroupSeperator;
                    finalString += FlightData.sPortOfOrigin + GroupSeperator;
                    finalString += FlightData.sPortOfDestination + GroupSeperator;
                    finalString += FlightData.sFlightRegistrationNo + GroupSeperator;
                    finalString += "N" + GroupSeperator + GroupSeperator;
                    finalString += "\n";
                    finalString += "<END-flightigm>" + "\n";
                    finalString += "<mawbigm>" + "\n";
                    foreach (var item in FlightData.tblAirIGMMAWBMs)
                    {

                        finalString += "F" + GroupSeperator + FlightData.tblAirLocationM.sCustomCode + GroupSeperator;
                        finalString += FlightData.sFlightNo + GroupSeperator;
                        finalString += FlightData.dtDepartureDate?.ToString("ddMMyyyy", CultureInfo.InvariantCulture) + GroupSeperator + "BULK" + GroupSeperator;
                        finalString += item.sMAWBNo + GroupSeperator;
                        finalString += GroupSeperator;
                        finalString += item.sPortOfOrigin + GroupSeperator;
                        finalString += item.sPortOfDestination + GroupSeperator;
                        finalString += item.cShipmentType + GroupSeperator;
                        finalString += (int)item.dNoOfPackages + GroupSeperator;
                        finalString += String.Format("{0:0.##}", item.dTotalWeight) + GroupSeperator;
                        finalString += item.sGoodsDescription + GroupSeperator + "BUP" + GroupSeperator + GroupSeperator;
                        finalString += "\n";
                    }
                    finalString += "<END-mawbigm>" + "\n";
                    finalString += "<END-igm>" + "\n";
                    finalString += "TREC" + GroupSeperator + FlightData.iFlightId;
                    finalString += "\n";
                }
                else
                {
                    sFlightNumber = "";
                }
                return finalString;
            }
        }


        public ResponseStatus SaveMAWB(AirIGMMAWBModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    var data = db.tblAirIGMMAWBMs.Where(z => z.iAirIGMMAWBId == model.iAirIGMMAWBId).SingleOrDefault();
                    if (data == null)
                    {
                        //if (db.tblAirIGMMAWBMs.Any(z => z.sMAWBNo == model.sMAWBNo))
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        data = new tblAirIGMMAWBM
                        {
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId,
                            sPortOfDestination = model.sPortOfDestination,
                            sMAWBNo = model.sMAWBNo,
                            sPortOfOrigin = model.sPortOfOrigin,
                            dNoOfPackages = Convert.ToDecimal(model.sNoOfPackages),
                            dTotalWeight = Convert.ToDecimal(model.sTotalWeight),
                            iAirIGMFlightDetailsId = model.iFlightId,
                            sGoodsDescription = model.sGoodsDescription,
                            cShipmentType = model.cShipmentType,
                            sPalletNo = model.sPalletNo,
                            sSpecialHandlingCode = model.sSpecialHandlingCode
                        };
                        db.tblAirIGMMAWBMs.Add(data);
                    }
                    else
                    {
                        //if (db.tblAirIGMMAWBMs.Any(z => z.iAirIGMMAWBId != model.iAirIGMMAWBId && z.sMAWBNo == model.sMAWBNo))
                        //{
                        //    return new ResponseStatus { Status = false, Message = "MAWB No already exists" };
                        //}
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = iUserId;
                        data.sPalletNo = model.sPalletNo;
                        data.sSpecialHandlingCode = model.sSpecialHandlingCode;
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


        public AirIGMMAWBModel GetAirMAWBbyiMAWBId(int? iMAWBId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirIGMMAWBMs.Where(z => z.iAirIGMMAWBId == iMAWBId)
                    .ToList().Select(c => new AirIGMMAWBModel
                    {
                        cShipmentType = c.cShipmentType,
                        iAirIGMMAWBId = c.iAirIGMMAWBId,
                        iFlightId = c.iAirIGMFlightDetailsId ?? 0,
                        sGoodsDescription = c.sGoodsDescription,
                        sMAWBNo = c.sMAWBNo,
                        sNoOfPackages = Convert.ToString(c.dNoOfPackages ?? 0),
                        sPortOfDestination = c.sPortOfDestination,
                        sTotalWeight = Convert.ToString(c.dTotalWeight ?? 0),
                        sPortOfOrigin = c.sPortOfOrigin,
                        sPalletNo = c.sPalletNo,
                        sSpecialHandlingCode = c.sSpecialHandlingCode
                    }).SingleOrDefault();
            }
        }

        public ResponseStatus ProceedToTransmit(int iFlightId)
        {
            try
            {

                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).SingleOrDefault();
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

        public AirIGMFlightViewModel GetViewDataByFlightId(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).ToList().Select((z, i) => new AirIGMFlightViewModel
                {
                    iFlightId = z.iFlightId,
                    sClientName = z.tblAirClientMaster.sClientName,
                    sFlightNo = z.sFlightNo,
                    sNo = i + 1,
                    lstAirIGMMAWBViewModel = z.tblAirIGMMAWBMs.Select(c => new AirIGMMAWBViewModel
                    {
                        cShipmentType = c.cShipmentType,
                        iAirIGMMAWBId = c.iAirIGMMAWBId,
                        iFlightId = c.iAirIGMFlightDetailsId ?? 0,
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



        public List<AirIGMFlightModel> GetFlights(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblAirIGMFlightDetailsMs.Where(z => (z.bIsReadyForTransmit ?? false) == false && (z.tblAirClientMaster.sClientName.Contains(search) || z.sFlightNo.Contains(search)));
                recordsTotal = query.Count();
                var data = query.OrderByDescending(z => z.dtActionDate)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirIGMFlightModel
                           {
                               iFlightId = z.iFlightId,
                               sClientName = z.tblAirClientMaster.sClientName,
                               sFlightNo = z.sFlightNo,
                               sNo = i + 1
                           }).ToList();
                return data;
            }

        }

        public List<AirIGMFlightModel> GetTransmittedFlights(int draw, int displayStart, int displayLength, string search, string minDate, string maxDate, out int recordsTotal)
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
                var query = db.tblAirIGMFlightDetailsMs.Where(z => (z.dtActionDate >= dtMinDate && z.dtActionDate <= dtMaxDate) && (z.bIsReadyForTransmit ?? false) && (z.tblAirClientMaster.sClientName.Contains(search) || z.sFlightNo.Contains(search)));
                recordsTotal = query.Count();
                var data = query.OrderByDescending(z => z.dtActionDate)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirIGMFlightModel
                           {
                               iFlightId = z.iFlightId,
                               sClientName = z.tblAirClientMaster.sClientName,
                               sFlightNo = z.sFlightNo,
                               sLocation = z.tblAirLocationM.sCustomCode,
                               sDateTime = z.dtActionDate?.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                               sUserName = db.tblUserMs.Where(v => v.iUserId == z.iActionBy).Select(b => b.sFirstName + " " + b.sLastName).FirstOrDefault(),
                               sNo = i + 1
                           }).ToList();
                return data;
            }

        }


        public ResponseStatus SaveAirIGMFlight(AirIGMFlightModel model, int iUserId, out int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == model.iFlightId).SingleOrDefault();
                if (data == null)
                {
                    data = new tblAirIGMFlightDetailsM
                    {
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        iClientId = model.iClientId,
                        iLocationId = model.iLocationId,
                        sFlightNo = model.sFlightNo,
                        dtArrivalDate = DateTime.ParseExact(model.sArrivalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        dtDepartureDate = DateTime.ParseExact(model.sDepartureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        sFlightRegistrationNo = model.sFlightRegistrationNo,
                        sPortOfDestination = model.sPortOfDestination,
                        sPortOfOrigin = model.sPortOfOrigin,
                        tTime = string.IsNullOrEmpty(model.sTime) ? (TimeSpan?)null : DateTime.ParseExact(model.sTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay
                    };
                    db.tblAirIGMFlightDetailsMs.Add(data);
                }
                else
                {
                    data.dtActionDate = DateTime.Now;
                    data.iActionBy = iUserId;
                    data.iClientId = model.iClientId;
                    data.iLocationId = model.iLocationId;
                    data.sFlightNo = model.sFlightNo;
                    data.tTime = string.IsNullOrEmpty(model.sTime) ? (TimeSpan?)null : DateTime.ParseExact(model.sTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                    data.dtArrivalDate = DateTime.ParseExact(model.sArrivalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    data.dtDepartureDate = DateTime.ParseExact(model.sDepartureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

        public ResponseStatus DeleteFlightDetails(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                try
                {
                    db.tblAirIGMMAWBMs.RemoveRange(db.tblAirIGMMAWBMs.Where(z => z.iAirIGMFlightDetailsId == iFlightId));
                    db.tblAirIGMFlightDetailsMs.RemoveRange(db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId));
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
                    db.tblAirIGMMAWBMs.RemoveRange(db.tblAirIGMMAWBMs.Where(z => z.iAirIGMMAWBId == iMAWBId));
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "MAWB deleted successfully" };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }

        public AirIGMFlightModel GetAirIGMFlightById(int iFlightId)
        {
            using (var db = new EzollutionProEntities())
            {
                var flight = db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).SingleOrDefault();
                flight.bIsReadyForTransmit = false;
                db.Entry(flight).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).ToList()
                       .Select(model => new AirIGMFlightModel
                       {
                           iFlightId = model.iFlightId,
                           iClientId = model.iClientId ?? 0,
                           iLocationId = model.iLocationId ?? 0,
                           sArrivalDate = model.dtArrivalDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                           sDepartureDate = model.dtDepartureDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                           sTime = new DateTime().Add(model.tTime ?? new TimeSpan()).ToString("HH:mm", CultureInfo.InvariantCulture),
                           sFlightNo = model.sFlightNo,
                           sFlightRegistrationNo = model.sFlightRegistrationNo,
                           sPortOfDestination = model.sPortOfDestination,
                           sPortOfOrigin = model.sPortOfOrigin
                       }).SingleOrDefault();
            }
        }

        public CheckListIGMCLPDFModel GeneratePDFData(int iFlightId)
        {
            
                using (var db = new EzollutionProEntities())
            {
                return db.tblAirIGMFlightDetailsMs.Where(z => z.iFlightId == iFlightId).ToList()
                           .Select(MAWB => new CheckListIGMCLPDFModel
                           {
                               sCustomLocation = MAWB.tblAirLocationM.sCustomLocation,
                               sPortOfDestination = MAWB.sPortOfDestination,
                               sPortOfOrigin = MAWB.sPortOfOrigin,
                               sFlightNo = MAWB.sFlightNo,
                               sDepatureDate = MAWB.dtDepartureDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                               sArrivalDate = MAWB.dtArrivalDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                               sFlightRegNo = MAWB.sFlightRegistrationNo,
                               sTime = MAWB.tTime.Value.ToString("t"),
                               lstMAWBData = MAWB.tblAirIGMMAWBMs.Select((mawb, i) => new CheckListAirIGMMAWBPDFModel
                               {
                                   sDescription = mawb.sGoodsDescription,
                                   sMAWBNo = mawb.sMAWBNo,
                                   sPackages = mawb.dNoOfPackages.Value,
                                   sWeight = mawb.dTotalWeight.Value,
                                   sPortOfDestination = mawb.sPortOfDestination,
                                   sPortOfOrigin = mawb.sPortOfOrigin,
                               }).ToList()
                           }).SingleOrDefault();

            }
          
        }

    }
}
