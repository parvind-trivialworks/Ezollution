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
    public class POFDService
    {
        private static POFDService instance = null;

        private POFDService()
        {
        }

        public static POFDService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new POFDService();
                }
                return instance;
            }
        }

        public List<PODModel> GetPOFDs(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblPOFDMasters.Where(z => z.sPortName.Contains(search) || z.sPortCode.Contains(search));
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

        public ResponseStatus SavePOFD(PODModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblPOFDMasters.Where(z => z.iPortID == model.iPortID).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblPOFDMasters.Any(z => z.sPortName == model.sPortName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port of final destination code already exists."
                        };
                    }
                    data = new tblPOFDMaster
                    {
                        dtCreatedDate = DateTime.Now,
                        iCreatedBy = iUserId,
                        sPortCode = model.sPortCode,
                        sPortName = model.sPortName,
                        bStatus = true,
                        sDescription = model.sDescription,
                    };
                    db.tblPOFDMasters.Add(data);
                }
                else
                {
                    if (db.tblPOFDMasters.Any(z => z.sPortCode == model.sPortCode && z.iPortID != model.iPortID))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Port of final destination name already exists"
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
                    Message = "Port of final destination saved successfully!"
                };
            }
        }

        public PODModel GetPOFDById(int iPortID)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPOFDMasters.Where(z => z.iPortID == iPortID).Select(model => new PODModel
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