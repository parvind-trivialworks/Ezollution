using EzollutionPro_DAL;
using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EzollutionPro_BAL.Services
{
    public class ClientService
    {
        private static ClientService instance = null;

        private ClientService()
        {
        }

        public static ClientService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientService();
                }
                return instance;
            }
        }

        public List<ClientModel> GetClients(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblClientMasters.Where(z => z.sClientName.Contains(search) || z.sCARN.Contains(search) || z.sCompanyName.Contains(search));
                recordsTotal = query.Count();
                var data = query.OrderBy(z => z.sClientName)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new ClientModel
                {
                    iClientID = z.iClientID,
                    sClientName = z.sClientName,
                    sCARN = z.sCARN,
                    sCompanyName = z.sCompanyName,
                    iSNo = i + 1,
                    sGSTNNo=z.sGSTNNo,
                    sEmailID=z.sEmailID
                }).ToList();
                return data;
            }

        }

        public List<DropDownData> GetAllClientsForDdl()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientMasters.Select(z => new DropDownData
                {
                    Text = z.sClientName,
                    Value = z.iClientID.ToString(),
                    Id = z.iClientID
                }).ToList();
            }
        }


        public ResponseStatus SaveClient(ClientModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblClientMasters.Where(z => z.iClientID == model.iClientID).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblClientMasters.Any(z => z.sClientName == model.sClientName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Client name already exists"
                        };
                    }
                    data = new tblClientMaster
                    {
                        dtCreatedDate = DateTime.Now,
                        iCreatedBy = iUserId,
                        sClientName = model.sClientName,
                        sCARN = model.sCARN,
                        sClientCode = model.sClientCode,
                        sCompanyName = model.sCompanyName,
                        sFaxNumber = model.sFaxNumber,
                        sICEGateAirID = model.sICEGateAirID,
                        sEmailID = model.sEmailID,
                        sICEGateAirPassword = model.sICEGateAirPassword,
                        sICEGateSeaID = model.sICEGateSeaID,
                        sICEGateSeaPassword = model.sICEGateSeaPassword,
                        bIsActive = true,
                        sLandLineNumber = model.sLandLineNumber,
                        sMobileNumber = model.sMobileNumber,
                        sOfficeAddress = model.sOfficeAddress,
                        sPinCode = model.sPinCode,
                        sGSTNNo = model.sGSTNNo,
                        sStateCode=model.sStateCode,
                        sStateName = db.tblStateMs.Where(w => w.sStateCode == model.sStateCode).Select(s => s.sStateName).SingleOrDefault(),
                        dPricePerUnit = model.dPricePerUnit,

                    };
                    db.tblClientMasters.Add(data);
                }
                else
                {
                    if (db.tblClientMasters.Any(z => z.sClientName == model.sClientName && z.iClientID != model.iClientID))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Client name already exists"
                        };
                    }
                    data.sClientName = model.sClientName;
                    data.sCARN = model.sCARN;
                    data.sClientCode = model.sClientCode;
                    data.sCompanyName = model.sCompanyName;
                    data.sFaxNumber = model.sFaxNumber;
                    data.sICEGateAirID = model.sICEGateAirID;
                    data.sEmailID = model.sEmailID;
                    data.sICEGateAirPassword = model.sICEGateAirPassword;
                    data.sICEGateSeaID = model.sICEGateSeaID;
                    data.sICEGateSeaPassword = model.sICEGateSeaPassword;
                    data.bIsActive = true;// model.bIsActive;
                    data.sLandLineNumber = model.sLandLineNumber;
                    data.sMobileNumber = model.sMobileNumber;
                    data.sOfficeAddress = model.sOfficeAddress;
                    data.iModifiedBy = iUserId;
                    data.dtModifiedDate = DateTime.Now;
                    data.sPinCode = model.sPinCode;
                    data.sGSTNNo = model.sGSTNNo;
                    data.sStateCode = model.sStateCode;
                    data.sStateName = db.tblStateMs.Where(w => w.sStateCode == model.sStateCode).Select(s => s.sStateName).SingleOrDefault();
                    data.dPricePerUnit = model.dPricePerUnit;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Client saved successfully!"
                };
            }
        }

        public ClientModel GetClientById(int iClientId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientMasters.Where(z => z.iClientID == iClientId).Select(model => new ClientModel
                {
                    iClientID = model.iClientID,
                    sClientName = model.sClientName,
                    sCARN = model.sCARN,
                    sClientCode = model.sClientCode,
                    sCompanyName = model.sCompanyName,
                    sFaxNumber = model.sFaxNumber,
                    sICEGateAirID = model.sICEGateAirID,
                    sEmailID = model.sEmailID,
                    sICEGateAirPassword = model.sICEGateAirPassword,
                    sICEGateSeaID = model.sICEGateSeaID,
                    sICEGateSeaPassword = model.sICEGateSeaPassword,
                    bIsActive = model.bIsActive,
                    sLandLineNumber = model.sLandLineNumber,
                    sMobileNumber = model.sMobileNumber,
                    sOfficeAddress = model.sOfficeAddress,
                    dtModifiedDate = model.dtModifiedDate,
                    iModifiedBy = model.iModifiedBy,
                    iCreatedBy = model.iCreatedBy,
                    dtCreatedDate = model.dtCreatedDate,
                    sPinCode = model.sPinCode,
                    sGSTNNo = model.sGSTNNo,
                    sStateCode = model.sStateCode,
                    sStateName = model.sStateName,
                    dPricePerUnit=model.dPricePerUnit
                }).SingleOrDefault();
            }
        }

        public vw_ClientMasterModel GetVwClient(int iClientId, string iClientType)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.vw_ClientMaster.Where(z => z.iClientId == iClientId && z.iClientType == iClientType).Select(model => new vw_ClientMasterModel
                {
                    iClientId = model.iClientId,
                    iClientType = model.iClientType,
                    ClientName = model.ClientName,
                    sClientCode = model.sClientCode,
                    sStateCode = model.sStateCode,
                    dPricePerUnit = model.dPricePerUnit
                }).FirstOrDefault();
            }
        }
    }
}