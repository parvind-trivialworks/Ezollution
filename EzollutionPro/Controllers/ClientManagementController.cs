using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EzollutionPro_BAL;
using EzollutionPro_BAL.Services.MasterServices;
using EzollutionPro_BAL.Utilities;

namespace EzollutionPro.Controllers
{
    public class ClientManagementController : BaseController
    {
        // GET: ClientManagement
        bool IsEdit = false;
        public ActionResult Index()
        {
            //Report Code Start
            var model = new ClientManagementSearchModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetClientManagementList()
        {
            string minDate = Request.Form.GetValues("FromDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("ToDate").FirstOrDefault() ?? "";
            var data = ClientManagementService.Instance.GetClientManagementList(minDate,maxDate, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }
      
        public ActionResult AddEdit(int? iClientManagementId = 0)
        {
            bool AllowEdit = false;
            var user = GetUserInfo();
            if (user.iRoleId == 1)
            {
                AllowEdit = true;
            }
            if (iClientManagementId == 0)
            {
                AllowEdit = true;
            }
            ViewBag.AllowEdit = AllowEdit;
            var model = new ClientManagementModel();
            if (iClientManagementId.GetValueOrDefault() > 0)
            {
                model = ClientManagementService.Instance.GetClientManagement(iClientManagementId.GetValueOrDefault());
                if(model==null)
                {
                    return RedirectToAction("AddEdit");
                }
            }
            else
            {
                model = new ClientManagementModel();
                model.dtDate = System.DateTime.Now.FormatDate();
                model.blsActive = true;
            }
            return View("AddEdit", model);
        }

        [HttpPost]
        public ActionResult AddEdit(ClientManagementModel model)
        {
            if (ModelState.IsValid)
            {
                int ClientManagementId= ClientManagementService.Instance.AddUpdateClientManagement(model, GetUserInfo().iUserId);
                if (ClientManagementId > 0)
                {
                    TempData["Message"] = "success";
                    return RedirectToAction("AddEdit", new { iClientManagementId = ClientManagementId });
                }
                else
                {
                    return RedirectToAction("AddEdit");
                }
            }
            else
            {
                return View("AddEdit", model);
            }
        }

        public PartialViewResult AddEditClientManagementFollowUp(int iClientManagementID,int iClientManagementFollowupId=0)
        {
            var model = new ClientManagementFollowupModel();
            bool AllowEdit = false;
            var user = GetUserInfo();
            if (user.iRoleId == 1)
            {
                AllowEdit = true;
            }
            if (iClientManagementID == 0)
            {
                AllowEdit = true;
            }
            ViewBag.AllowEdit = AllowEdit;
            if (iClientManagementFollowupId > 0)
            {
                model = ClientManagementService.Instance.GetClientManagementFollowup(iClientManagementFollowupId);
                return PartialView("pvAddEditClientManagementFollowUp", model);
            }
            else
            {
                model.iClientManagementId = iClientManagementID;
                return PartialView("pvAddEditClientManagementFollowUp", model);
            }
        }

        [HttpPost]
        public JsonResult SaveClientManagementFollowUp(ClientManagementFollowupModel model)
        {
            if (ModelState.IsValid)
            {                
                return Json(ClientManagementService.Instance.AddUdpdateClientManagementFollowup(model, GetUserInfo().iUserId));
            }
            else
            {
                return Json(new ResponseStatus { Status = false, Message = ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage });
            }
        }

        [HttpPost]
        public JsonResult GetClientManagementFollowUps (int iClientManagementId)
        {
            var data = ClientManagementService.Instance.GetClientManagementFollowupsList(iClientManagementId);
            int recordsTotal = data.Count();
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }
    }
}