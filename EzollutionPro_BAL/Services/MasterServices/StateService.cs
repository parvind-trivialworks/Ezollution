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
    public class StateService
    {
        private static StateService instance = null;

        private StateService()
        {
        }

        public static StateService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StateService();
                }
                return instance;
            }
        }

        public List<StateModel> GetStates(int draw, int displayStart, int displayLength, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblStateMs.OrderBy(z => z.sStateName).Skip(displayStart).Take(displayLength).Select(z => new StateModel
                {
                    iStateId = z.iStateId,
                    sStateName = z.sStateName,
                    sStateCode = z.sStateCode,
                    sStateDescription = z.sDescription,
                    sCountryName = z.tblCountryM.sCountryName
                }).ToList();
                recordsTotal = db.tblStateMs.Count();
                return data;
            }

        }

        public ResponseStatus SaveState(StateModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblStateMs.Where(z => z.iStateId == model.iStateId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblStateMs.Any(z => z.sStateName == model.sStateName))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "State name already exists"
                        };
                    }
                    data = new tblStateM
                    {
                        sStateCode = model.sStateCode,
                        dtActionDate = DateTime.Now,
                        iActionBy = iUserId,
                        sStateName = model.sStateName,
                        sDescription= model.sStateDescription,
                        iCountryId=model.iCountryId
                    };
                    db.tblStateMs.Add(data);
                }
                else
                {
                    if (db.tblStateMs.Any(z => z.sStateName == model.sStateName && z.iStateId != model.iStateId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "State name already exists"
                        };
                    }
                    data.sStateCode = model.sStateCode;
                    data.dtActionDate = DateTime.Now;
                    data.iActionBy = iUserId;
                    data.sDescription = model.sStateDescription;
                    data.sStateName = model.sStateName;
                    data.iCountryId= model.iCountryId;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "State saved successfully!"
                };
            }
        }

        public StateModel GetStateById(int iStateId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblStateMs.Where(z => z.iStateId == iStateId).Select(y => new StateModel
                {
                    iStateId = y.iStateId,
                    sStateCode = y.sStateCode,
                    sStateName = y.sStateName,
                    iCountryId=y.iCountryId,
                    sStateDescription = y.sDescription
                }).SingleOrDefault();
            }
        }
    }
}