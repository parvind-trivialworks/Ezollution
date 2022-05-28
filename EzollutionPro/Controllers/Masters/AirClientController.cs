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
    public class AirClientController : BaseController
    {
        // GET: AirClient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUpdateClient(int iClientId=0)
        {
            ViewBag.States = new List<SelectListItem>();
            ViewBag.States = UserService.Instance.GetGSTStates();
            if (iClientId == 0)
            {
                return View(new AirClientModel());
            }
            
            var model = AirClientService.Instance.GetAirClientById(iClientId);
            return View(model);
        }


        [HttpPost]
        public JsonResult GetClients()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = AirClientService.Instance.GetAirClients(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        [HttpPost]
        public ActionResult AddUpdateClient(AirClientModel model)
        {

            if (ModelState.IsValid)
            {
                var res = AirClientService.Instance.SaveAirClient(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.States = new List<SelectListItem>();
                    ViewBag.States = UserService.Instance.GetGSTStates();
                    ModelState.AddModelError("", res.Message);
                    return View(model);
                }
            }
            else
            { 
                ViewBag.States = new List<SelectListItem>();
                ViewBag.States = UserService.Instance.GetGSTStates();
                return View(model);
            }
        }
    }
}