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
    public class ClientMailController : BaseController
    {
        // GET: ClientMail
        public ActionResult Index()
        {
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            var model = new EmailSearchModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetMail(int iClientType, int iClientId)
        {
            int recordsTotal = 0;
            var data = ClientEmailService.Instance.GetClientMails(iClientType, iClientId);
            recordsTotal=data.Count;
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        [HttpPost]
        public JsonResult GetClients(string ClientType)
        {
            if (ClientType == "0") //sea
            {
                return Json(ClientService.Instance.GetAllClientsForDdl(), JsonRequestBehavior.AllowGet);
            }
            else if (ClientType == "1") //air
            {
                return Json(AirClientService.Instance.GetAllClientsForDdl(), JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddEditEmail(int iClientId, int iMailId = 0)
        {
            var model = new ClientEmailModel();
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            if (iMailId > 0)
            {
                model = ClientEmailService.Instance.AddUdpdateEmail(iMailId);
                return PartialView("pvAddUpdateEmail", model);
            }
            else
            {
                model.iClientId = iClientId;
                return PartialView("pvAddUpdateEmail", model);
            }
        }

        [HttpPost]
        public JsonResult SaveEmail(ClientEmailModel model)
        {
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            if (ModelState.IsValid)
            {
                return Json(ClientEmailService.Instance.SaveEmail(model, GetUserInfo().iUserId));
            }
            else
            {
                return Json(new ResponseStatus { Status = false, Message = ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage });
            }
        }
    }
}