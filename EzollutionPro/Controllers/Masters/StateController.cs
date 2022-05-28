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
    public class StateController : BaseController
    {
        // GET: State
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetStates()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            var data = StateService.Instance.GetStates(draw, DisplayStart, DisplayLength, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdateState(int iStateId = 0)
        {
            ViewBag.Countries = UserService.Instance.GetCountries();
            if (iStateId == 0)
            {
                return PartialView("pvAddUpdateState");
            }
            return PartialView("pvAddUpdateState", StateService.Instance.GetStateById(iStateId));
        }
        [HttpPost]
        public JsonResult AddUpdateState(StateModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(StateService.Instance.SaveState(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
    }
}