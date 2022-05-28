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
    public class POSService
    {
        private static POSService instance = null;

        private POSService()
        {
        }

        public static POSService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new POSService();
                }
                return instance;
            }
        }

        public List<PODModel> GetPOSs(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblPOSMasters.Where(z => z.sPortName.Contains(search) || z.sPortCode.Contains(search));
                recordsTotal = query.Count();
                var data = query.OrderBy(z => z.sPortName)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new PODModel
                {
                    sPortCode = z.sPortCode,
                    sPortName = z.sPortName,
                    bStatus = z.bStatus ?? false,
                    iPortID = z.iPortID,
                    sDescription = z.sDescription,
                    iSNo = i + 1,
                }).ToList();
                return data;
            }

        }

        public ResponseStatus SavePOS(PODModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblPOSMasters.Where(z => z.iPortID == model.iPortID).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblPOSMasters.Any(z => z.sPortName == model.sPortName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port Of Shipment code already exists."
                        };
                    }
                    data = new tblPOSMaster
                    {
                        dtCreatedDate = DateTime.Now,
                        iCreatedBy = iUserId,
                        sPortCode = model.sPortCode,
                        sPortName = model.sPortName,
                        bStatus = true,
                        sDescription = model.sDescription,
                    };
                    db.tblPOSMasters.Add(data);
                }
                else
                {
                    if (db.tblPOSMasters.Any(z => z.sPortCode == model.sPortCode && z.iPortID != model.iPortID))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port Of Shipment name already exists"
                        };
                    }
                    data.dtCreatedDate = DateTime.Now;
                    data.iCreatedBy = iUserId;
                    data.sPortCode = model.sPortCode;
                    data.sPortName = model.sPortName;
                    data.bStatus = true;
                    data.sDescription = model.sDescription;
                    data.iModifiedBy = iUserId;
                    data.dtModifiedDate = DateTime.Now;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Port Of Shipment saved successfully!"
                };
            }
        }

        public PODModel GetPOSById(int iPortID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPOSMasters.Where(z => z.iPortID == iPortID).Select(model => new PODModel
                {
                    sPortCode = model.sPortCode,
                    sPortName = model.sPortName,
                    bStatus = model.bStatus??false,
                    sDescription = model.sDescription,
                    iPortID=model.iPortID
                }).SingleOrDefault();
            }
        }
    }
}