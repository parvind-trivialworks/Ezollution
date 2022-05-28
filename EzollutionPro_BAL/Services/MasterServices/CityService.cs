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
    public class CityService
    {
        private static CityService instance = null;

        private CityService()
        {
        }

        public static CityService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CityService();
                }
                return instance;
            }
        }

        public List<CityModel> GetCities(int draw, int displayStart, int displayLength, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblCityMs.OrderBy(z => z.sCityName).Skip(displayStart).Take(displayLength).Select(z => new CityModel
                {
                    iCityId = z.iCityId,
                    sCityName = z.sCityName,
                    sCityDescription = z.sDescription,
                    sCountryName = z.tblStateM.tblCountryM.sCountryName,
                    sStateName=z.tblStateM.sStateName
                }).ToList();
                recordsTotal = db.tblCityMs.Count();
                return data;
            }

        }

        public ResponseStatus SaveCity(CityModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblCityMs.Where(z => z.iCityId == model.iCityId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblCityMs.Any(z => z.sCityName == model.sCityName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "City name already exists"
                        };
                    }
                    data = new tblCityM
                    {
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        sCityName = model.sCityName,
                        sDescription= model.sCityDescription,
                        iStateId=model.iStateId,
                    };
                    db.tblCityMs.Add(data);
                }
                else
                {
                    if (db.tblCityMs.Any(z => z.sCityName == model.sCityName && z.iCityId != model.iCityId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "City name already exists"
                        };
                    }
                    data.iStateId = model.iStateId;
                    data.dtActionDate = DateTime.Now;
                    data.iActionBy = iUserId;
                    data.sDescription = model.sCityDescription;
                    data.sCityName = model.sCityName;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "City saved successfully!"
                };
            }
        }

        public CityModel GetCityById(int iCityId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblCityMs.Where(z => z.iCityId == iCityId).Select(y => new CityModel
                {
                    iCityId = y.iCityId,
                    sCityName = y.sCityName,
                    iCountryId=y.tblStateM.iCountryId,
                    iStateId=y.iStateId,
                    sCityDescription = y.sDescription
                }).SingleOrDefault();
            }
        }
    }
}