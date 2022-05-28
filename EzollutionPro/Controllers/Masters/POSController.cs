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
    public class POSController : BaseController
    {
        // GET: POS
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPOSs()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = POSService.Instance.GetPOSs(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdatePOS(int iPortId = 0)
        {
            if (iPortId == 0)
            {
                return PartialView("pvAddUpdatePOS");
            }
            var model = POSService.Instance.GetPOSById(iPortId);
            return PartialView("pvAddUpdatePOS", model);
        }
        [HttpPost]
        public JsonResult AddUpdatePOS(PODModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(POSService.Instance.SavePOS(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
    }
}