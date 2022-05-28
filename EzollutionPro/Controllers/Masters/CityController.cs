using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers.Masters
{
    public class CityController : BaseController
    {
        // GET: City
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetCities()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            var data = CityService.Instance.GetCities(draw, DisplayStart, DisplayLength, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdateCity(int iCityId = 0)
        {
            ViewBag.Countries = UserService.Instance.GetCountries();
            ViewBag.States = new List<SelectListItem>();
            if (iCityId == 0)
            {
                return PartialView("pvAddUpdateCity");
            }
            var model = CityService.Instance.GetCityById(iCityId);
            ViewBag.States = UserService.Instance.GetStates(model.iCountryId);
            return PartialView("pvAddUpdateCity", model);
        }
        [HttpPost]
        public JsonResult AddUpdateCity(CityModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(CityService.Instance.SaveCity(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
        public JsonResult GetStates(int? iCountryId)
        {
            return Json(UserService.Instance.GetStates(iCountryId??0), JsonRequestBehavior.AllowGet);
        }
    }
}