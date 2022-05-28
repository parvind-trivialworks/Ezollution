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
    public class ShippingLineController : BaseController
    {
        // GET: ShippingLine
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetShippingLines()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = ShippingLineService.Instance.GetShippingLines(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdateShippingLine(int iShippingLineId = 0)
        {
            if (iShippingLineId == 0)
            {
                return PartialView("pvAddUpdateShippingLine",new ShippingLineModel());
            }
            var model = ShippingLineService.Instance.GetShippingLineById(iShippingLineId);
            return PartialView("pvAddUpdateShippingLine", model);
        }
        [HttpPost]
        public JsonResult AddUpdateShippingLine(ShippingLineModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(ShippingLineService.Instance.SaveShippingLine(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
        public JsonResult GetStates(int? iCountryId)
        {
            return Json(UserService.Instance.GetStates(iCountryId ?? 0), JsonRequestBehavior.AllowGet);
        }
    }
}