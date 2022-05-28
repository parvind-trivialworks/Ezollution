using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace EzollutionPro.Controllers
{
    public class AirIGMFlightController : BaseController
    {
        // GET: AirIGMFlight
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
            var data = AirIGMFlightService.Instance.GetFlights(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult AddUpdateFlightDetails(int iFlightId = 0)
        {
            ViewBag.Clients = MAWBService.Instance.GetAirClients();
            ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
            if (iFlightId == 0)
            {
                return View(new AirIGMFlightModel
                {
                    sTime = DateTime.Now.ToString("hh:mm", CultureInfo.InvariantCulture)
                });
            }
            var model = AirIGMFlightService.Instance.GetAirIGMFlightById(iFlightId);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUpdateFlightDetails(AirIGMFlightModel model)
        {
            if (ModelState.IsValid)
            {
                var res = AirIGMFlightService.Instance.SaveAirIGMFlight(model, GetUserInfo().iUserId, out int iFlightId);
                if (res.Status)
                {
                    return RedirectToAction("ViewFlightDetails", new { iFlightId = iFlightId });
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
        public ActionResult ViewFlightDetails(int iFlightId)
        {
            AirIGMFlightViewModel model = AirIGMFlightService.Instance.GetViewDataByFlightId(iFlightId);
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteMAWB(int iMAWBId)
        {
            return Json(AirIGMFlightService.Instance.DeleteMAWB(iMAWBId));
        }
        [HttpPost]
        public JsonResult DeleteFlight(int iFlightId)
        {
            return Json(AirIGMFlightService.Instance.DeleteFlightDetails(iFlightId));
        }

        public ActionResult AddUpdateMAWB(int iFlightId, int? iMAWBId)
        {
            if (iMAWBId == null)
            {
                var flightData = AirIGMFlightService.Instance.GetAirIGMFlightById(iFlightId);
                return View(new AirIGMMAWBModel
                {
                    iFlightId = iFlightId,
                    sPortOfDestination = flightData.sPortOfDestination,
                    sPortOfOrigin = flightData.sPortOfOrigin,
                    sPalletNo = "BULK",
                    sSpecialHandlingCode = "BUP"
                });
            }
            else
            {
                return View(AirIGMFlightService.Instance.GetAirMAWBbyiMAWBId(iMAWBId));
            }
        }

        [HttpPost]
        public JsonResult ProceedToTransmit(int iFlightId)
        {
            return Json(AirIGMFlightService.Instance.ProceedToTransmit(iFlightId));
        }

        [HttpPost]
        public ActionResult AddUpdateMAWB(AirIGMMAWBModel model)
        {
            if (ModelState.IsValid)
            {
                var res = AirIGMFlightService.Instance.SaveMAWB(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    TempData["MAWBSuccess"] = res.Message;
                    return RedirectToAction("ViewFlightDetails",new { model.iFlightId});
                }
                else
                {
                    TempData["MAWBError"] = res.Message;
                    var FlightData = AirIGMFlightService.Instance.GetAirIGMFlightById(model.iFlightId);
                    return View(new AirIGMMAWBModel
                    {
                        iFlightId = model.iFlightId,
                        sPortOfOrigin = FlightData.sPortOfOrigin,
                        sPortOfDestination = FlightData.sPortOfDestination
                    });
                }
            }
            return View(new AirIGMMAWBModel());
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
            var data = AirIGMFlightService.Instance.GetTransmittedFlights(draw, DisplayStart, DisplayLength, search,minDate,maxDate, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }
        public ActionResult TransmitFile(int iFlightId)
        {
            var fileData = AirIGMFlightService.Instance.GetTransmitFileData(iFlightId, GetUserInfo().iUserId, out string sFightNumber);
            byte[] btFile = Encoding.ASCII.GetBytes(fileData);
            return File(btFile, System.Net.Mime.MediaTypeNames.Application.Octet, sFightNumber + DateTime.Now.ToString("ddMMMyy", CultureInfo.InvariantCulture) + ".IGM");
        }

        public ActionResult DownloadCheckList(int iFlightId)
        {
            var data = AirIGMFlightService.Instance.GeneratePDFData(iFlightId);
            if (data != null)
            {
                var pdfPath = Server.MapPath("~/Content/file/") + "CL_" + iFlightId + ".pdf";
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
                GeneratePDFService generate = new GeneratePDFService(pdfPath, data, pdfPath);
                generate.SetOrientation(TemplateOrientation.LANDSCAPE);
                generate.AddFonts(new PDFFonts().init());
                generate.StartAIRIGMFlight();
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                string fileName = "CL_" + iFlightId + ".pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "data is null");
        }
    }
}