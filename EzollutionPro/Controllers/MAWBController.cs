using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class MAWBController : BaseController
    {
        // GET: MAWB
        public ActionResult Index()
        {
            ViewBag.iRoleId = GetUserInfo().iRoleId;
            return View();
        }

        [HttpPost]
        public JsonResult GetMAWBs()
        {
            ViewBag.iRoleId = GetUserInfo().iRoleId;
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault();
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault();
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = MAWBService.Instance.GetScheduling(minDate, maxDate,DisplayStart,DisplayLength,search,GetUserInfo(), out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult AddUpdateMAWB(long iMAWBId = 0)
        {
            var clients = MAWBService.Instance.GetAirClients();
            var userInfo = this.GetUserInfo();
            ViewBag.iRoleId = userInfo.iRoleId;
            if (userInfo.bIsClient)
            {
                clients = clients.Where(x => x.Id == userInfo.iClientID).ToList();
            }

            // if (ViewBag.CurrentRole != null && Convert.ToString(ViewBag.CurrentRole))
            ViewBag.Clients = clients;
            ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
            if (iMAWBId == 0)
            {
                return View(new MAWBModel());
            }
            else
            {
                return View(MAWBService.Instance.GetMAWBbyId(iMAWBId));
            }
        }

        [HttpPost]
        public ActionResult AddUpdateMAWB(MAWBModel model)
        {
            if (ModelState.IsValid)
            {
                long iMAWBId = model.iMAWBId;
                ViewBag.Clients = MAWBService.Instance.GetAirClients();
                ViewBag.CustomLocations = MAWBService.Instance.GetCustomLocations();
                var res = MAWBService.Instance.SaveMAWB(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    
                    if (iMAWBId == 0)
                    {
                        return RedirectToAction("ViewMAWB", new { iMAWBId = model.iMAWBId });
                    }
                    else
                    {
                        TempData["MAWBSuccess"] = res.Message;
                        return RedirectToAction("ViewMAWB", new { iMAWBId = model.iMAWBId });
                        //return RedirectToAction("AddUpdateMAWB", new { iMAWBId = model.iMAWBId });
                    }
                }
                else
                {
                    TempData["MAWBError"] = res.Message;
                    return View(new MAWBModel());
                }
            }
            return View(new MAWBModel());
        }

        public ActionResult AddUpdateHAWB(long iMAWBId, long iHAWBId = 0)
        {
            if (iHAWBId == 0)
            {
                var data = MAWBService.Instance.GetMAWBbyId(iMAWBId);
                return View(new HAWBModel { iMAWBId = iMAWBId,sOrigin=data.sOrigin,sDestination=data.sDestination });
            }
            else
            {
                return View(MAWBService.Instance.GetHAWBbyId(iHAWBId));
            }
        }

        [HttpPost]
        public ActionResult AddUpdateHAWB(HAWBModel model)
        {
            if (ModelState.IsValid)
            {
                var res = MAWBService.Instance.SaveHAWB(model, GetUserInfo().iUserId);
                if (res.Status)
                {
                    TempData["HAWBSuccess"] = res.Message;
                    return RedirectToAction("ViewMAWB", new { iMAWBId = model.iMAWBId });
                }
                else
                {
                    TempData["HAWBError"] = res.Message;
                    return View();
                }
            }
            return View();
        }

        public ActionResult ViewMAWB(long iMAWBId)
        {
            return View(MAWBService.Instance.GetMAWBMaster(iMAWBId));
        }

        public JsonResult DeleteMAWB(long iMAWBId)
        {
            return Json(MAWBService.Instance.DeleteMAWB(iMAWBId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteHAWB(long iHAWBId)
        {
            return Json(MAWBService.Instance.DeleteHAWB(iHAWBId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Transmit(long iMAWBId)
        {
            var fileData = MAWBService.Instance.GetTransmitFileData(iMAWBId,GetUserInfo().iUserId,out string sMAWBNumber);
            byte[] btFile = Encoding.ASCII.GetBytes(fileData);
            return File(btFile, System.Net.Mime.MediaTypeNames.Application.Octet, sMAWBNumber + ".cgm");
        }

        public ActionResult Transmitted()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult GetTransmitteds()
        {
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault();
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault();
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var userInfo = this.GetUserInfo();
            var data = MAWBService.Instance.GetTransmitted(minDate, maxDate, DisplayStart, DisplayLength, search,userInfo, out int recordsTotal);

            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult DownloadCheckList(int iMAWBId)
        {
            var data = MAWBService.Instance.GeneratePDFData(iMAWBId);
            if (data != null)
            {
                var pdfPath = Server.MapPath("~/Content/file/") + "CL_" + data.sMAWBNo + ".pdf";
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
                GeneratePDFService generate = new GeneratePDFService(pdfPath, data, pdfPath);
                generate.SetOrientation(TemplateOrientation.LANDSCAPE);
                generate.AddFonts(new PDFFonts().init());
                generate.StartMAWB();
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                string fileName = "CL_" + data.sMAWBNo + ".pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "data is null");
        }

        public JsonResult ProceedToTransmit(int iMAWBId)
        {
            var status = MAWBService.Instance.ProceedToTransmit(iMAWBId);
            return Json(status,JsonRequestBehavior.AllowGet);
        }

    }
}