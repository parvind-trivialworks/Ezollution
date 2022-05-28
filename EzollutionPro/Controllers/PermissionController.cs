using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class PermissionController : BaseController
    {
        // GET: Permission
        public ActionResult Index()
        {
            ViewBag.Roles = UserService.Instance.GetRoles();
            return View(PermissionService.Instance.GetAllPermissions());
        }

        public JsonResult GetPermissions(int iRoleId)
        {
            return Json(PermissionService.Instance.GetPermissionsByRoleId(iRoleId),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddUpdatePermissions(List<int> permissions,int RoleId)
        {
            return Json(PermissionService.Instance.SavePermissions(permissions, RoleId, GetUserInfo().iUserId));
        }
    }
}