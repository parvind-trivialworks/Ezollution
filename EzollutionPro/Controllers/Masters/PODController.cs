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
    public class PODController : BaseController
    {
        // GET: POD
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPODs()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = PODService.Instance.GetPODs(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdatePOD(int iPortId = 0)
        {
            if (iPortId == 0)
            {
                return PartialView("pvAddUpdatePOD");
            }
            var model = PODService.Instance.GetPODById(iPortId);
            return PartialView("pvAddUpdatePOD", model);
        }
        [HttpPost]
        public JsonResult AddUpdatePOD(PODModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(PODService.Instance.SavePOD(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
    }
}