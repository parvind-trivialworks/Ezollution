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
    public class LoginService
    {
        private static LoginService instance = null;

        private LoginService()
        {
        }

        public static LoginService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginService();
                }
                return instance;
            }
        }

        public UserModel ValidateUser(LoginModel model)
        {
            using (var db = new EzollutionProEntities())
            {
                model.Password = Crypto.Encrypt(model.Password);
                if (db.tblUserMs.Any(x => x.sUsername == model.Username && x.sPassword == model.Password))
                {
                    return db.tblUserMs.Where(x => x.sUsername == model.Username).Select(x => new UserModel
                    {
                        iCityId = x.iCityId ?? 0,
                        iCountryId = x.tblCityM.tblStateM.iCountryId,
                        iRoleId = x.iRoleId ?? 0,
                        iStateId = x.tblCityM.iStateId,
                        iUserId = x.iUserId,
                        sAddress = x.sAddress,
                        sAddress2 = x.sAddress2,
                        sCityName = x.tblCityM.sCityName,
                        sCountryName = x.tblCityM.tblStateM.tblCountryM.sCountryName,
                        sEmailID = x.sEmailID,
                        sFirstName = x.sFirstName,
                        sLastName = x.sLastName,
                        sPassword = x.sPassword,
                        sPhoneNo = x.sPhoneNo,
                        sPhotoUrl = x.sPhotoUrl,
                        sRoleName = x.tblRoleM.sRoleName,
                        sStateName = x.tblCityM.tblStateM.sStateName,
                        sUsername = x.sUsername,
                        sZipCode = x.sZipCode,
                        bIsClient = x.tblRoleM.bIsClient ?? false,
                        iClientID = x.iClientID
                    }).SingleOrDefault();
                }
                else
                    return null;
            }
        }

        public bool ValidateUsername(string Username)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblUserMs.Any(x => x.sUsername == Username);
            }
        }
    }
}