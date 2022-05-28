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
    public class CountryController : BaseController
    {
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetCountries()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            var data = CountryService.Instance.GetCountries(draw, DisplayStart, DisplayLength, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdateCountry(int iCountryId = 0)
        {
            if (iCountryId == 0)
            {
                return PartialView("pvAddUpdateCountry");
            }
            return PartialView("pvAddUpdateCountry", CountryService.Instance.GetCountryById(iCountryId));
        }
        [HttpPost]
        public JsonResult AddUpdateCountry(CountryModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.CountryImage != null)
                {
                    var extension = Path.GetExtension(model.CountryImage.FileName);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        var fileTimeStamp = DateTime.Now.ToString("ddMMYYYYhhmmss", CultureInfo.InvariantCulture);
                        var picturePath = Server.MapPath("~/Content/UserImages/") + fileTimeStamp + "." + extension;
                        model.CountryImage.SaveAs(picturePath);
                        model.sCountryImageUrl = "/Content/UserImages/" + fileTimeStamp + "." + extension;
                    }
                    else
                        return Json(new ResponseStatus { Status = false, Message = "Only JPG and PNG images are allowed" });
                }
                return Json(CountryService.Instance.SaveCountry(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
    }
}