using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Services.MasterServices;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers.Masters
{
    public class BondMasterController : BaseController
    {
        // GET: BondMaster
        public ActionResult Index()
        {
            ViewBag.PODs = SeaSchedulingService.Instance.GetPODs();
            ViewBag.FPODs = SeaSchedulingService.Instance.GetFPODs();
            ViewBag.ShippingLines = SeaSchedulingService.Instance.GetShippingLines();
            return View();
        }
        [HttpPost]
        public JsonResult GetBonds()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            int iShippingId = Convert.ToInt32(string.IsNullOrEmpty(Request.Form.GetValues("iShippingId").FirstOrDefault())?"0": Request.Form.GetValues("iShippingId").FirstOrDefault());
            int iPOD = Convert.ToInt32(string.IsNullOrEmpty(Request.Form.GetValues("iPOD").FirstOrDefault()) ? "0" : Request.Form.GetValues("iPOD").FirstOrDefault());
            int iFPOD = Convert.ToInt32(string.IsNullOrEmpty(Request.Form.GetValues("iFPOD").FirstOrDefault()) ? "0" : Request.Form.GetValues("iFPOD").FirstOrDefault());
            string sModeOfTransport = Request.Form.GetValues("sModeOfTransport").FirstOrDefault();
            string sCargoMovement = Request.Form.GetValues("sCargoMovement").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = BondService.Instance.GetBondMasters(draw, DisplayStart, DisplayLength, search,iShippingId,iPOD,iFPOD,sModeOfTransport,sCargoMovement, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }
        public PartialViewResult AddUpdateBond(int iBondId = 0)
        {
            ViewBag.PODs = SeaSchedulingService.Instance.GetPODs();
            ViewBag.FPODs = SeaSchedulingService.Instance.GetFPODs();
            ViewBag.ShippingLines = SeaSchedulingService.Instance.GetShippingLines();
            if (iBondId == 0)
            {
                return PartialView("pvAddUpdateBond");
            }
            return PartialView("pvAddUpdateBond", BondService.Instance.GetBondById(iBondId));
        }
        [HttpPost]
        public JsonResult SaveBond(BondModel model)
        {
            if (model.sCargoMovement != "LC")
            {
                ModelState.Remove("sCFSCode");
                ModelState.Remove("sCFSName");
            }
            if (ModelState.IsValid)
            {
                return Json(BondService.Instance.SaveBond(model, 1));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
        public JsonResult GetMLOCode(int iShippingId)
        {
            return Json(BondService.Instance.GetMLOCode(iShippingId),JsonRequestBehavior.AllowGet);
        }

    }
}
