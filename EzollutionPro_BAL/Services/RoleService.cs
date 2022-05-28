using EzollutionPro_DAL;
using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Services
{
    public class RoleService
    {
        private static RoleService instance = null;

        private RoleService()
        {
        }

        public static RoleService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleService();
                }
                return instance;
            }
        }

        public List<RoleModel> GetRoles(int draw, int displayStart, int displayLength, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblRoleMs.OrderBy(z => z.sRoleName).Skip(displayStart).Take(displayLength).Select(z => new RoleModel { iRoleId = z.iRoleId, sDescription = z.sDescription, sRoleName = z.sRoleName }).ToList();
                recordsTotal = db.tblRoleMs.Count();
                return data;
            }
        }

        public RoleModel GetRoleById(int iRoleId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblRoleMs.Where(x => x.iRoleId == iRoleId).Select(z => new RoleModel
                {
                    iRoleId = z.iRoleId,
                    sDescription = z.sDescription,
                    sRoleName = z.sRoleName,
                    bIsClient = z.bIsClient ?? false
                }).SingleOrDefault();
            }
        }

        public ResponseStatus SaveRole(RoleModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblRoleMs.Where(z => z.iRoleId == model.iRoleId).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblRoleMs.Any(z => z.sRoleName == model.sRoleName))
                    {
                        return new ResponseStatus { Status = false, Message = "Role already exists" };
                    }
                    else
                    {
                        data = new tblRoleM
                        {
                            sDescription = model.sDescription,
                            sRoleName = model.sRoleName,
                            bIsClient = model.bIsClient,
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId
                        };
                        db.tblRoleMs.Add(data);
                        db.SaveChanges();
                        return new ResponseStatus { Status = true, Message = "Role saved successfully!" };
                    }
                }
                else
                {
                    if (db.tblRoleMs.Any(z => z.sRoleName == model.sRoleName && z.iRoleId != model.iRoleId))
                    {
                        return new ResponseStatus { Status = false, Message = "Role already exists" };
                    }
                    else
                    {
                        data.bIsClient = model.bIsClient;
                        data.sDescription = model.sDescription;
                        data.sRoleName = model.sRoleName;
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = iUserId;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return new ResponseStatus { Status = true, Message = "Role saved successfully!" };
                    }
                }
            }
        }
    }
}