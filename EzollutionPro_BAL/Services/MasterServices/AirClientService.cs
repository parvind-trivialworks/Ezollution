using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Services.MasterServices
{
    public class AirClientService
    {
        private static AirClientService instance = null;

        private AirClientService()
        {
        }

        public static AirClientService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirClientService();
                }
                return instance;
            }
        }
        public List<AirClientModel> GetAirClients(int iclientId)
        {
            using (var db = new EzollutionProEntities())
            {

                var data = db.tblAirClientMasters.Where(w=>w.iAirClientId==iclientId).OrderBy(z => z.sClientName)
                          .ToList().Select((z, i) => new AirClientModel
                          {
                              iAirClientId = z.iAirClientId,
                              sClientName = z.sClientName,
                              sCARNNo = z.sCARNNo,
                              sCompanyName = z.sCompanyName,
                              sEmailId = z.sEmailId,
                              sGSTNo = z.sGSTNo
                          }).ToList();
                return data;
            }

        }
        public List<AirClientModel> GetAirClients()
        {
            using (var db = new EzollutionProEntities())
            {
               
                var data = db.tblAirClientMasters.OrderBy(z => z.sClientName)
                          .ToList().Select((z, i) => new AirClientModel
                           {
                               iAirClientId = z.iAirClientId,
                               sClientName = z.sClientName,
                               sCARNNo = z.sCARNNo,
                               sCompanyName = z.sCompanyName,
                               sEmailId = z.sEmailId,
                               sGSTNo = z.sGSTNo
                           }).ToList();
                return data;
            }

        }
        public List<AirClientModel> GetAirClients(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblAirClientMasters.Where(z => z.sClientName.Contains(search) || z.sCARNNo.Contains(search) || z.sCompanyName.Contains(search));
                recordsTotal = query.Count();
                var data = query.OrderBy(z => z.sClientName)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirClientModel
                           {
                               iAirClientId = z.iAirClientId,
                               sClientName = z.sClientName,
                               sCARNNo = z.sCARNNo,
                               sCompanyName = z.sCompanyName,
                               sEmailId=z.sEmailId,
                               sGSTNo=z.sGSTNo
                           }).ToList();
                return data;
            }

        }

        public ResponseStatus SaveAirClient(AirClientModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblAirClientMasters.Where(z => z.iAirClientId == model.iAirClientId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblAirClientMasters.Any(z => z.sClientName == model.sClientName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "AirClient name already exists"
                        };
                    }
                    data = new tblAirClientMaster
                    {
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        sClientName = model.sClientName,
                        sCARNNo = model.sCARNNo,
                        sCompanyName = model.sCompanyName,
                        sICEGateId = model.sICEGateId,
                        sPassword = model.sPassword,
                        sGSTNo = model.sGSTNo,
                        sAddress = model.sAddress,
                        sPinCode = model.sPinCode,
                        sEmailId = model.sEmailId,
                        sStateName= db.tblStateMs.Where(w=>w.sStateCode== model.sStateCode).Select(s=>s.sStateName).SingleOrDefault(),
                        sStateCode = model.sStateCode,
                        dPricePerUnit=model.dPricePerUnit
                    };
                    db.tblAirClientMasters.Add(data);
                }
                else
                {
                    if (db.tblAirClientMasters.Any(z => z.sClientName == model.sClientName && z.iAirClientId != model.iAirClientId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "AirClient name already exists"
                        };
                    }
                    data.sClientName = model.sClientName;
                    data.sCARNNo = model.sCARNNo;
                    data.sCompanyName = model.sCompanyName;
                    data.sICEGateId = model.sICEGateId;
                    data.sPassword = model.sPassword;
                    data.sAddress = model.sAddress;
                    data.iActionBy = iUserId;
                    data.sGSTNo = model.sGSTNo;
                    data.dtActionDate = DateTime.Now;
                    data.sPinCode = model.sPinCode;
                    data.sEmailId = model.sEmailId;
                    data.sStateName = db.tblStateMs.Where(w => w.sStateCode == model.sStateCode).Select(s => s.sStateName).SingleOrDefault();
                    data.sStateCode = model.sStateCode;
                    data.dPricePerUnit = model.dPricePerUnit;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "AirClient saved successfully!"
                };
            }
        }

        public AirClientModel GetAirClientById(int iAirClientId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirClientMasters.Where(z => z.iAirClientId == iAirClientId).Select(model => new AirClientModel
                {
                    iAirClientId = model.iAirClientId,
                    sClientName = model.sClientName,
                    sCARNNo = model.sCARNNo,
                    sCompanyName = model.sCompanyName,
                    sICEGateId = model.sICEGateId,
                    sPassword = model.sPassword,
                    sGSTNo = model.sGSTNo,
                    sAddress = model.sAddress,
                    sPinCode = model.sPinCode,
                    sEmailId = model.sEmailId,
                    sStateCode=model.sStateCode.ToString(),
                    sStateName=model.sStateName.ToString(),
                    dPricePerUnit=model.dPricePerUnit
                }).SingleOrDefault();
            }
        }

        public List<DropDownData> GetAllClientsForDdl()  
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirClientMasters.Select(z => new DropDownData
                {
                    Text = z.sClientName,
                    Value = z.iAirClientId.ToString(),
                    Id = z.iAirClientId
                }).ToList();
            }
        }
    }
}
