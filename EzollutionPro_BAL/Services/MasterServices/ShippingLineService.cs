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
    public class ShippingLineService
    {
        private static ShippingLineService instance = null;

        private ShippingLineService()
        {
        }

        public static ShippingLineService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShippingLineService();
                }
                return instance;
            }
        }

        public List<ShippingLineModel> GetShippingLines(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblShippingLines.Where(z => z.sMLOCode.Contains(search) || z.sShippingLineName.Contains(search));
                recordsTotal = query.Count();
                var data = query.OrderBy(z => z.sShippingLineName).Skip(displayStart).Take(displayLength).ToList().Select((z, i) => new ShippingLineModel
                {
                    iShippingID = z.iShippingID,
                    sShippingLineName = z.sShippingLineName,
                    sDescription = z.sDescription,
                    sMLOCode = z.sMLOCode,
                    iSNo = i + 1
                }).ToList();
                return data;
            }

        }

        public ResponseStatus SaveShippingLine(ShippingLineModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblShippingLines.Where(z => z.iShippingID == model.iShippingID).SingleOrDefault();
                if (data == null)
                {
                    data = new tblShippingLine
                    {
                        dtCreatedDate = DateTime.Now,
                        iCreatedBy = iUserId,
                        sShippingLineName = model.sShippingLineName,
                        sDescription = model.sDescription,
                        sMLOCode = model.sMLOCode,
                        bStatus = true
                    };
                    db.tblShippingLines.Add(data);
                }
                else
                {
                    data.sShippingLineName = model.sShippingLineName;
                    data.sDescription = model.sDescription;
                    data.sMLOCode = model.sMLOCode;
                    data.sDescription = model.sDescription;
                    data.sShippingLineName = model.sShippingLineName;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Shipping Line saved successfully!"
                };
            }
        }

        public ShippingLineModel GetShippingLineById(int iShippingLineId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblShippingLines.Where(z => z.iShippingID == iShippingLineId).Select(y => new ShippingLineModel
                {
                    iShippingID = y.iShippingID,
                    sShippingLineName = y.sShippingLineName,
                    sDescription = y.sDescription,
                    sMLOCode = y.sMLOCode,
                }).SingleOrDefault();
            }
        }
    }
}