using EzollutionPro_DAL;
using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro_BAL.Services
{
    public class UserService
    {
        private static UserService instance = null;

        private UserService()
        {
        }

        public static UserService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserService();
                }
                return instance;
            }
        }

        public List<UserModel> GetUsers(int draw, int displayStart, int displayLength, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblUserMs.OrderBy(z => z.sUsername).Skip(displayStart).Take(displayLength).Select(z => new UserModel
                {
                    iUserId = z.iUserId,
                    sUsername = z.sUsername,
                    sEmailID = z.sEmailID,
                    sFirstName = z.sFirstName,
                    sLastName = z.sLastName,
                    sPhoneNo = z.sPhoneNo,
                    sZipCode = z.sZipCode,
                    sCityName = z.tblCityM.sCityName,
                    sStateName = z.tblCityM.tblStateM.sStateName,
                    sCountryName = z.tblCityM.tblStateM.tblCountryM.sCountryName,
                    sRoleName = z.tblRoleM.sRoleName
                }).ToList();
                recordsTotal = db.tblUserMs.Count();
                return data;
            }
        }

        public List<SelectListItem> GetRoles()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblRoleMs.ToList().Select(z => new SelectListItem
                {
                    Text = z.sRoleName,
                    Value = z.iRoleId.ToString()
                }).ToList();
            }
        }

        public List<SelectListItem> GetCountries()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblCountryMs.ToList().Select(z => new SelectListItem
                {
                    Text = z.sCountryName,
                    Value = z.iCountryId.ToString()
                }).ToList();
            }
        }

        public List<SelectListItem> GetStates(int iCountryId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblStateMs.Where(z => z.iCountryId == iCountryId).ToList().Select(z => new SelectListItem
                {
                    Text = z.sStateName,
                    Value = z.iStateId.ToString()
                }).ToList();
            }
        }

        public List<SelectListItem> GetGSTStates()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblStateMs.Where(z => z.iCountryId == 4).ToList().Select(z => new SelectListItem
                {
                    Text = z.sStateName,
                    Value = z.sStateCode.ToString()
                }).OrderBy(o=>o.Text).ToList();
            }
        }

        public List<SelectListItem> GetCities(int iStateId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblCityMs.Where(z => z.iStateId == iStateId).ToList().Select(z => new SelectListItem
                {
                    Text = z.sCityName,
                    Value = z.iCityId.ToString()
                }).ToList();
            }
        }

        public UserModel GetUserById(int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblUserMs.Where(x => x.iUserId == iUserId).Select(z => new UserModel
                {
                    iRoleId = z.iRoleId ?? 0,
                    iUserId = z.iUserId,
                    sUsername = z.sUsername,
                    sEmailID = z.sEmailID,
                    sFirstName = z.sFirstName,
                    sLastName = z.sLastName,
                    sPhoneNo = z.sPhoneNo,
                    sZipCode = z.sZipCode,
                    iCityId = z.iCityId ?? 0,
                    iStateId = z.tblCityM.iStateId,
                    iCountryId = z.tblCityM.tblStateM.iCountryId,
                    sAddress = z.sAddress,
                    sAddress2 = z.sAddress2,
                    sPhotoUrl = z.sPhotoUrl,
                    sPassword = z.sPassword,
                    iClientID=z.iClientID
                }).SingleOrDefault();
            }
        }

        public ResponseStatus SaveUser(UserModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblUserMs.Where(z => z.iUserId == model.iUserId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblUserMs.Any(z => z.sUsername == model.sUsername))
                    {
                        return new ResponseStatus { Status = false, Message = "User already exists" };
                    }
                    else
                    {
                        data = new tblUserM
                        {
                            iCityId = model.iCityId,
                            sAddress = model.sAddress,
                            sAddress2 = model.sAddress2,
                            sEmailID = model.sEmailID,
                            sFirstName = model.sFirstName,
                            sLastName = model.sLastName,
                            sPassword = Crypto.Encrypt(model.sPassword),
                            iRoleId = model.iRoleId,
                            sPhoneNo = model.sPhoneNo,
                            sPhotoUrl = model.sPhotoUrl,
                            sZipCode = model.sZipCode,
                            sUsername = model.sUsername,
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId,
                            iClientID =model.iClientID
                        };
                        db.tblUserMs.Add(data);
                        db.SaveChanges();
                        return new ResponseStatus { Status = true, Message = "User saved successfully!" };
                    }
                }
                else
                {
                    if (db.tblUserMs.Any(z => z.sUsername == model.sUsername && z.iUserId != model.iUserId))
                    {
                        return new ResponseStatus { Status = false, Message = "User already exists" };
                    }
                    else
                    {

                        data.iCityId = model.iCityId;
                        data.sAddress = model.sAddress;
                        data.sAddress2 = model.sAddress2;
                        data.sEmailID = model.sEmailID;
                        data.sFirstName = model.sFirstName;
                        data.sLastName = model.sLastName;
                        data.sPassword = Crypto.Encrypt(model.sPassword);
                        data.iRoleId = model.iRoleId;
                        data.sPhoneNo = model.sPhoneNo;
                        data.sPhotoUrl = model.sPhotoUrl;
                        data.sZipCode = model.sZipCode;
                        data.sUsername = model.sUsername;
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = iUserId;
                        data.iClientID = model.iClientID;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return new ResponseStatus { Status = true, Message = "User saved successfully!" };
                    }
                }
            }
        }
    }
}