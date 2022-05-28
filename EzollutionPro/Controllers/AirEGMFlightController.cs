using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class AirEGMFlightController : BaseController
    {
        // GET: AirEGMFlight
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFlights()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = AirEGMFlightService.Instance.GetFlights(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult AddUpdateFlightDetails(int iFlightId = 0)
        {
            ViewBag.Clients = MAWBService.Instance.GetAirClients();
            ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
            if (iFlightId == 0)
            {
                return View(new AirEGMFlightModel());
            }
            var model = AirEGMFlightService.Instance.GetAirEGMFlightById(iFlightId);
            return View(model);
        }

        public ActionResult ViewFlightDetails(int iFlightId)
        {
            AirEGMFlightViewModel model = AirEGMFlightService.Instance.GetViewDataByFlightId(iFlightId);
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteMAWB(int iMAWBId)
        {
            return Json(AirEGMFlightService.Instance.DeleteMAWB(iMAWBId));
        }
        [HttpPost]
        public JsonResult DeleteFlight(int iFlightId)
        {
            return Json(AirEGMFlightService.Instance.DeleteFlightDetails(iFlightId));
        }


        [HttpPost]
        public ActionResult AddUpdateFlightDetails(AirEGMFlightModel model)
        {

            if (ModelState.IsValid)
            {
                var res = AirEGMFlightService.Instance.SaveAirEGMFlight(model, GetUserInfo().iUserId,out int iFlightId);
                if (res.Status)
                {
                    return RedirectToAction("ViewFlightDetails",new { iFlightId});
                }
                else
                {
                    ViewBag.Clients = MAWBService.Instance.GetAirClients();
                    ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
                    ModelState.AddModelError("", res.Message);
                    return View();
                }
            }
            else
            {
                ViewBag.Clients = MAWBService.Instance.GetAirClients();
                ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
                return View(model);
            }
        }

        public ActionResult AddUpdateMAWB(int iFlightId, int? iMAWBId)
        {
            if (iMAWBId == null)
            {
                var flightData = AirEGMFlightService.Instance.GetAirEGMFlightById(iFlightId);
                return View(new AirEGMMAWBModel
                {
                    iFlightId = iFlightId,
                    sPortOfOrigin = flightData.sPortOfOrigin,
                    sPortOfDestination = flightData.sPortOfDestination
                });
            }
            else
            {
                return View(AirEGMFlightService.Instance.GetAirMAWBbyiMAWBId(iMAWBId));
            }
        }

        [HttpPost]
        public JsonResult ProceedToTransmit(int iFlightId)
        {
            return Json(AirEGMFlightService.Instance.ProceedToTransmit(iFlightId));
        }
        [HttpPost]
        public ActionResult AddUpdateMAWB(AirEGMMAWBModel model)
        {
            if (ModelState.IsValid)
            {
                var res = AirEGMFlightService.Instance.SaveMAWB(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    TempData["MAWBSuccess"] = res.Message;
                    return RedirectToAction("ViewFlightDetails", new { model.iFlightId });
                }
                else
                {
                    TempData["MAWBError"] = res.Message;
                    var flightData = AirEGMFlightService.Instance.GetAirEGMFlightById(model.iFlightId);
                    return View(new AirEGMMAWBModel
                    {
                        iFlightId = model.iFlightId,
                        sPortOfOrigin = flightData.sPortOfOrigin,
                        sPortOfDestination=flightData.sPortOfDestination
                    });
                }
            }
            return View(new AirEGMMAWBModel());
        }

        public ActionResult Transmit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTransmittedFlights()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault();
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault();
            var data = AirEGMFlightService.Instance.GetTransmittedFlights(draw, DisplayStart, DisplayLength, search, minDate, maxDate, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult TransmitFile(int iFlightId)
        {
            var fileData = AirEGMFlightService.Instance.GetTransmitFileData(iFlightId, GetUserInfo().iUserId, out string sEGMNumber, out string sPortOfOrigin);
            byte[] btFile = Encoding.ASCII.GetBytes(fileData);
            return File(btFile, System.Net.Mime.MediaTypeNames.Application.Octet, sEGMNumber + sPortOfOrigin + ".egm");
        }
    }
}