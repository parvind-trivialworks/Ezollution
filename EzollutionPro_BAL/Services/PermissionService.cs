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
    public class PermissionService
    {
        private static PermissionService instance = null;
        private PermissionService()
        {
        }
        public static PermissionService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PermissionService();
                }
                return instance;
            }
        }
        public List<PermissionModel> GetAllPermissions()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblPermissionsMs.Select(z => new PermissionModel
                {
                    sPath = z.sPath,
                    iPermissionId = z.iPermissionId,
                    sPermissionName = z.sPermissionName
                }).ToList();
            }
        }

        public List<int> GetPermissionsByRoleId(int iRoleId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblRolePermissionMaps
                       .Where(z => z.iRoleId == iRoleId)
                       .Select(z => z.iPermissionId??0).ToList();
            }
        }
        
        public ResponseStatus SavePermissions(List<int> iPermissionIds,int iRoleId,int iUserId)
        {
            using (var db= new EzollutionProEntities())
            {
                try
                {
                    var roles = db.tblRolePermissionMaps.Where(z => z.iRoleId == iRoleId);
                    var permissionsToAdd = iPermissionIds.Except(roles.Select(z => z.iPermissionId ?? 0));
                    db.tblRolePermissionMaps.RemoveRange(roles.Where(z => !iPermissionIds.Contains(z.iPermissionId ?? 0)));
                    db.SaveChanges();
                    foreach (var permission in permissionsToAdd)
                    {
                        db.tblRolePermissionMaps.Add(new tblRolePermissionMap
                        {
                            dtActionDate = DateTime.Now,
                            iActionBy = iUserId,
                            iRoleId = iRoleId,
                            iPermissionId = permission,
                        });
                        db.SaveChanges();
                    }
                    return new ResponseStatus { Status = true, Message = "Permission mapped successfully." };
                }
                catch (Exception)
                {
                    return new ResponseStatus { Status = false, Message = "Something went wrong" };
                }
            }
        }
    }
}