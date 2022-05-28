using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class SeaSchedulingController : BaseController
    {
        #region SeaScheduling
        public ActionResult Index()
        {            
            return View();
        }
        [HttpPost]
        public JsonResult GetSchedulingData()
        {
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault() ?? "";
            var data = SeaSchedulingService.Instance.GetScheduling(minDate,GetUserInfo().iClientID, maxDate, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }
        public PartialViewResult AddUpdateSchedule(int iScheduleId = 0)
        {
            ViewBag.Clients = SeaSchedulingService.Instance.GetClients(GetUserInfo().iClientID);
            ViewBag.PODs = SeaSchedulingService.Instance.GetPODs();
            ViewBag.FPODs = SeaSchedulingService.Instance.GetFPODs();
            ViewBag.ShippingLines = SeaSchedulingService.Instance.GetShippingLines();
            ViewBag.LoginUserID = GetUserInfo().iUserId;
            ViewBag.FileStatus_s = Enum.GetValues(typeof(Scheduling)).Cast<Scheduling>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            if (iScheduleId == 0)
            {
                return PartialView("pvAddUpdateSchedule", new SchedulingViewModel());
            }
            return PartialView("pvAddUpdateSchedule", SeaSchedulingService.Instance.AddUdpdateSchedule(iScheduleId));
        }
        [HttpPost]
        public JsonResult SaveSchedule(SchedulingViewModel model)
        {
            if (ModelState.IsValid)
            {

                return Json(SeaSchedulingService.Instance.SaveSchedule(model, GetUserInfo().iUserId));
            }
            else
            {
                return Json(new ResponseStatus { Status = false, Message = ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage });
            }
        }
        public ActionResult Checklist(int iSchedulingId)
        {
            var data = SeaSchedulingService.Instance.GetChecklistData(iSchedulingId);
            return View(data);
        }
        public ActionResult DownloadCheckList(int iSchedulingId)
        {
            var data = SeaSchedulingService.Instance.GeneratePDFData(iSchedulingId);
            if (data != null)
            {
                var pdfPath = Server.MapPath("~/Content/file/") + "CL_" + data.MBLNumber + ".pdf";
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
                GeneratePDFService generate = new GeneratePDFService(pdfPath, data, pdfPath);
                generate.SetOrientation(TemplateOrientation.LANDSCAPE);
                generate.AddFonts(new PDFFonts().init());
                generate.Start();
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                string fileName = "CL_" + data.MBLNumber + ".pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "data is null");
        }
        #endregion

        #region MBL
        public ActionResult AddUpdateMBL(int iSchedulingId, int? iMBLId)
        {
            ViewBag.POS = SeaSchedulingService.Instance.GetPOSs();
            return View(SeaSchedulingService.Instance.AddUpdateMBL(iSchedulingId, iMBLId));
        }
        [HttpPost]
        public ActionResult AddUpdateMBL(MBLData model)
        {
            if (model.sCargoMovement != "LC")
            {
                ModelState.Remove("sCFSCode");
            }
            if (ModelState.IsValid)
            {
                ResponseStatus responseStatus = SeaSchedulingService.Instance.SaveMBL(model, GetUserInfo().iUserId);
                if (responseStatus.Status)
                {
                    TempData["SuccessMessage"] = responseStatus.Message;
                    return RedirectToAction("Checklist", new { model.iSchedulingId });
                }
                else
                {
                    ModelState.AddModelError("", responseStatus.Message);
                    ViewBag.POS = SeaSchedulingService.Instance.GetPOSs();
                    return View("AddUpdateMBL", model);
                }
            }
            else
            {
                ViewBag.POS = SeaSchedulingService.Instance.GetPOSs();
                return View("AddUpdateMBL", model);
            }
        }
        [HttpPost]
        public JsonResult DeleteMBL(int iMBLId)
        {
            return Json(SeaSchedulingService.Instance.DeleteMBL(iMBLId));
        }
        #endregion

        #region HBL
        public ActionResult AddUpdateHBL(int SchedulingId, int MBLId, int? HBLId)
        {
            ViewBag.POS = SeaSchedulingService.Instance.GetPOSs();
            return View(SeaSchedulingService.Instance.AddUpdateHBL(SchedulingId, MBLId, HBLId));
        }
        [HttpPost]
        public ActionResult AddUpdateHBL(HBLData model)
        {
            if (ModelState.IsValid)
            {
                ResponseStatus responseStatus = SeaSchedulingService.Instance.SaveHBL(model, GetUserInfo().iUserId);
                if (responseStatus.Status)
                {
                    TempData["SuccessMessage"] = responseStatus.Message;
                    return RedirectToAction("Checklist", new { model.iSchedulingId });
                }
                else
                {
                    ModelState.AddModelError("", responseStatus.Message);
                    return View("AddUpdateHBL", model);
                }
            }
            else
            {
                return View("AddUpdateHBL", model);
            }
        }
        [HttpPost]
        public JsonResult DeleteHBL(int iHBLId)
        {
            return Json(SeaSchedulingService.Instance.DeleteHBL(iHBLId));
        }
        #endregion

        #region Container
        public ActionResult AddUpdateContainer(int SchedulingId, int HBLId, int? iContainerId)
        {
            return View(SeaSchedulingService.Instance.AddUpdateContainer(SchedulingId, HBLId, iContainerId));
        }
        [HttpPost]
        public ActionResult AddUpdateContainer(ContainerData model)
        {
            if (ModelState.IsValid)
            {
                ResponseStatus responseStatus = SeaSchedulingService.Instance.SaveContainer(model, GetUserInfo().iUserId);
                if (responseStatus.Status)
                {
                    TempData["SuccessMessage"] = responseStatus.Message;
                    return RedirectToAction("Checklist", new { model.iSchedulingId });
                }
                else
                {
                    ModelState.AddModelError("", responseStatus.Message);
                    return View("AddUpdateHBL", model);
                }
            }
            else
            {
                return View("AddUpdateHBL", model);
            }
        }
        [HttpPost]
        public JsonResult DeleteContainer(int iContainerId)
        {
            return Json(SeaSchedulingService.Instance.DeleteContainer(iContainerId));
        }
        #endregion

        #region BondDetailss
        public ActionResult AddUpdateBondDetails(int SchedulingId, int? iBondId)
        {
            return View(SeaSchedulingService.Instance.AddUpdateBondDetails(SchedulingId, iBondId));
        }
        [HttpPost]
        public ActionResult AddUpdateBondDetails(BondData model)
        {
            if (ModelState.IsValid)
            {
                ResponseStatus responseStatus = SeaSchedulingService.Instance.SaveBondDetails(model, GetUserInfo().iUserId);
                if (responseStatus.Status)
                {
                    TempData["SuccessMessage"] = responseStatus.Message;
                    return RedirectToAction("Checklist", new { model.iSchedulingId });
                }
                else
                {
                    ModelState.AddModelError("", responseStatus.Message);
                    return View("AddUpdateBondDetails", model);
                }
            }
            else
            {
                return View("AddUpdateBondDetails", model);
            }
        }
        #endregion

        public ActionResult BulkUploadHBL()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BulkUploadHBL(BulkUploadHBLModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fileName = model.HBLFile.FileName;
                    var extension = System.IO.Path.GetExtension(model.HBLFile.FileName);
                    var sMBLNumber = fileName.TrimEnd(extension.ToCharArray());
                    if (!SeaSchedulingService.Instance.VerifyMBLNumber(sMBLNumber.Trim()))
                    {
                        ModelState.AddModelError("HBLFile", "File Name (MBL Number) is not mapped in our database.");
                        return View();
                    }
                    if (!(new string[] { ".xls", ".xlsx" }).Contains(extension))
                    {
                        ModelState.AddModelError("HBLFile", "Please Upload a valid Excel File.");
                        return View();
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName);
                    }
                    var filePath = Server.MapPath("~/Content/File") + "\\"+ model.HBLFile.FileName;
                    model.HBLFile.SaveAs(filePath);
                    var res = SeaSchedulingService.Instance.BulkUploadHBL(SeaSchedulingService.Instance.GetRequestsDataFromExcel(filePath), sMBLNumber, GetUserInfo().iUserId);
                    if (res.Status)
                    {
                        TempData["SuccessMsg"] = res.Message;
                    }
                    else
                    {
                        ModelState.AddModelError("",res.Message);

                    }
                    return View();
                }
                else
                {
                    return View();
                }
            }
            catch(Exception ex)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName))
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName);
                }
                ModelState.AddModelError("", ex.Message);
            }
            finally
            {
                if (System.IO.File.Exists(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName))
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/File") + "\\" + model.HBLFile.FileName);
                }
            }
            return View();
        }
    }
}