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
    public class PODService
    {
        private static PODService instance = null;

        private PODService()
        {
        }

        public static PODService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PODService();
                }
                return instance;
            }
        }

        public List<PODModel> GetPODs(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblPODMasters.Where(z => z.sPortName.Contains(search) || z.sPortCode.Contains(search));
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

        public ResponseStatus SavePOD(PODModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblPODMasters.Where(z => z.iPortID == model.iPortID).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblPODMasters.Any(z => z.sPortName == model.sPortName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port of destination code already exists."
                        };
                    }
                    data = new tblPODMaster
                    {
                        dtCreatedDate = DateTime.Now,
                        iCreatedBy = iUserId,
                        sPortCode = model.sPortCode,
                        sPortName = model.sPortName,
                        bStatus = true,
                        sDescription = model.sDescription,
                    };
                    db.tblPODMasters.Add(data);
                }
                else
                {
                    if (db.tblPODMasters.Any(z => z.sPortCode == model.sPortCode && z.iPortID != model.iPortID))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port of Destination name already exists"
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
                    Message = "Port of destination saved successfully!"
                };
            }
        }

        public PODModel GetPODById(int iPortID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPODMasters.Where(z => z.iPortID == iPortID).Select(model => new PODModel
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