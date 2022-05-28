using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Services.MasterServices;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            var data = UserService.Instance.GetUsers(draw, DisplayStart, DisplayLength, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data }); ;
        }

        public PartialViewResult AddUpdateUser(int iUserId = 0)
        {
            ViewBag.Countries = UserService.Instance.GetCountries();
            ViewBag.Roles = UserService.Instance.GetRoles();
            if (iUserId != 0)
            {
                var model = UserService.Instance.GetUserById(iUserId);
                model.sPassword = Crypto.Decrypt(model.sPassword);
                ViewBag.States = UserService.Instance.GetStates(model.iCountryId);
                ViewBag.Cities = UserService.Instance.GetCities(model.iStateId);
                if (model.iClientID.HasValue)
                    ViewBag.Clients = AirClientService.Instance.GetAirClients(model.iClientID.Value);
                else
                    ViewBag.Clients = AirClientService.Instance.GetAirClients();
                return PartialView("pvAddUpdateUser", model);
            }
            ViewBag.States = new List<SelectListItem>();
            ViewBag.Cities = new List<SelectListItem>();
            ViewBag.Clients = AirClientService.Instance.GetAirClients();
            return PartialView("pvAddUpdateUser");
        }

        [HttpPost]
        public JsonResult AddUpdateUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Picture != null)
                {
                    var extension = Path.GetExtension(model.Picture.FileName);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        var fileTimeStamp = DateTime.Now.ToString("ddMMYYYYhhmmss", CultureInfo.InvariantCulture);
                        var picturePath = Server.MapPath("~/Content/UserImages/") + fileTimeStamp + "." + extension;
                        model.Picture.SaveAs(picturePath);
                        model.sPhotoUrl = "/Content/UserImages/" + fileTimeStamp + "." + extension;
                    }
                    else
                        return Json(new ResponseStatus { Status = false, Message = "Only JPG and PNG images are allowed" });
                }
                return Json(UserService.Instance.SaveUser(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }

        public JsonResult GetCities(int? iStateId)
        {
            return Json(UserService.Instance.GetCities(iStateId??0), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates(int? iCountryId)
        {
            return Json(UserService.Instance.GetStates(iCountryId??0), JsonRequestBehavior.AllowGet);
        }
    }
}