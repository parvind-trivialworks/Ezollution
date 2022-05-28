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
    public class AirLocationService
    {
        private static AirLocationService instance = null;

        private AirLocationService()
        {
        }

        public static AirLocationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirLocationService();
                }
                return instance;
            }
        }

        public List<AirLocationModel> GetAirLocations(int draw, int displayStart, int displayLength, string search, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblAirLocationMs.Where(z => z.sCustomCode.Contains(search) || z.sCustomLocation.Contains(search) || z.sThreeLetterCode.Contains(search));
                recordsTotal = query.Count();
                var data = query.OrderBy(z => z.sCustomCode)
                           .Skip(displayStart)
                           .Take(displayLength).ToList().Select((z, i) => new AirLocationModel
                           {
                               iLocationId = z.iLocationId,
                               sCustomCode = z.sCustomCode,
                               sThreeLetterCode = z.sThreeLetterCode,
                               sCustomLocation = z.sCustomLocation,
                           }).ToList();
                return data;
            }

        }

        public ResponseStatus SaveAirLocation(AirLocationModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblAirLocationMs.Where(z => z.iLocationId == model.iLocationId).SingleOrDefault();
                if (data == null)
                {
                    data = new tblAirLocationM
                    {
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        sCustomLocation = model.sCustomLocation,
                        sCustomCode = model.sCustomCode,
                        sThreeLetterCode = model.sThreeLetterCode,
                    };
                    db.tblAirLocationMs.Add(data);
                }
                else
                {
                    data.sCustomLocation = model.sCustomLocation;
                    data.sCustomCode = model.sCustomCode;
                    data.sThreeLetterCode = model.sThreeLetterCode;
                    data.iActionBy = iUserId;
                    data.dtActionDate = DateTime.Now;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Location saved successfully!"
                };
            }
        }

        public AirLocationModel GetAirLocationById(int iLocationId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblAirLocationMs.Where(z => z.iLocationId == iLocationId).Select(model => new AirLocationModel
                {
                    iLocationId = model.iLocationId,
                    sCustomLocation = model.sCustomLocation,
                    sCustomCode = model.sCustomCode,
                    sThreeLetterCode = model.sThreeLetterCode,
                }).SingleOrDefault();
            }
        }
    }
}
