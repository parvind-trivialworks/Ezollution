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
    public class CountryService
    {
        private static CountryService instance = null;

        private CountryService()
        {
        }

        public static CountryService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CountryService();
                }
                return instance;
            }
        }

        public List<CountryModel> GetCountries(int draw, int displayStart, int displayLength, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblCountryMs.OrderBy(z => z.sCountryName).Skip(displayStart).Take(displayLength).Select(z => new CountryModel
                {
                    iCountryId = z.iCountryId,
                    sCountryName = z.sCountryName,
                    sCountryCode = z.sCountryCode,
                    sCountryPhoneCode = z.sCountryPhoneCode,
                    sCurrencyCode = z.sCurrencyCode,
                    sCountryDescription = z.sCountryDescription,
                    sCurrencyDescription = z.sCurrencyDescription
                }).ToList();
                recordsTotal = db.tblCountryMs.Count();
                return data;
            }

        }

        public ResponseStatus SaveCountry(CountryModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblCountryMs.Where(z => z.iCountryId == model.iCountryId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblCountryMs.Any(z => z.sCountryName == model.sCountryName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Country name already exists"
                        };
                    }
                    data = new tblCountryM
                    {
                        sCountryCode = model.sCountryCode,
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        sCountryDescription = model.sCountryDescription,
                        sCountryImageUrl = model.sCountryImageUrl,
                        sCountryName = model.sCountryName,
                        sCountryPhoneCode = model.sCountryPhoneCode,
                        sCurrencyCode = model.sCurrencyCode,
                        sCurrencyDescription = model.sCurrencyDescription
                    };
                    db.tblCountryMs.Add(data);
                }
                else
                {
                    if (db.tblCountryMs.Any(z => z.sCountryName == model.sCountryName && z.iCountryId != model.iCountryId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "Country name already exists"
                        };
                    }
                    data.sCountryCode = model.sCountryCode;
                    data.dtActionDate = DateTime.Now;
                    data.iActionBy = iUserId;
                    data.sCountryDescription = model.sCountryDescription;
                    if (!string.IsNullOrEmpty(model.sCountryImageUrl))
                        data.sCountryImageUrl = model.sCountryImageUrl;
                    data.sCountryName = model.sCountryName;
                    data.sCountryPhoneCode = model.sCountryPhoneCode;
                    data.sCurrencyCode = model.sCurrencyCode;
                    data.sCurrencyDescription = model.sCurrencyDescription;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Country saved successfully!"
                };
            }
        }

        public CountryModel GetCountryById(int iCountryId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblCountryMs.Where(z => z.iCountryId == iCountryId).Select(y => new CountryModel
                {
                    iCountryId = y.iCountryId,
                    sCountryCode = y.sCountryCode,
                    sCountryDescription = y.sCountryDescription,
                    sCountryName = y.sCountryName,
                    sCountryPhoneCode = y.sCountryPhoneCode,
                    sCurrencyCode = y.sCurrencyCode,
                    sCurrencyDescription = y.sCurrencyDescription
                }).SingleOrDefault();
            }
        }
    }
}