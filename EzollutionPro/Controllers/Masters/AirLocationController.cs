using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services.MasterServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers.Masters
{
    public class AirLocationController : BaseController
    {
        // GET: AirLocation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUpdateLocation(int iLocationId = 0)
        {
            if (iLocationId == 0)
            {
                return View(new AirLocationModel());
            }
            var model = AirLocationService.Instance.GetAirLocationById(iLocationId);
            return View(model);
        }


        [HttpPost]
        public JsonResult GetLocations()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = AirLocationService.Instance.GetAirLocations(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        [HttpPost]
        public ActionResult AddUpdateLocation(AirLocationModel model)
        {

            if (ModelState.IsValid)
            {
                var res = AirLocationService.Instance.SaveAirLocation(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", res.Message);
                    return View();
                }
            }
            else
                return View(model);
        }
    }

}
