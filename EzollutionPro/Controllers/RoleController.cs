using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetRoles()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            var data = RoleService.Instance.GetRoles(draw, DisplayStart, DisplayLength, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data }); ;
        }
        public PartialViewResult AddUpdateRole(int iRoleId = 0)
        {
            if (iRoleId != 0)
            {
                return PartialView("pvAddUpdateRole", RoleService.Instance.GetRoleById(iRoleId));
            }
            return PartialView("pvAddUpdateRole");
        }
        [HttpPost]
        public JsonResult AddUpdateRole(RoleModel model)
        {
            if (ModelState.IsValid)
                return Json(RoleService.Instance.SaveRole(model,1));
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
    }
}