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
    public class ClientController : BaseController
    {
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        public JsonResult GetClients()
        {
            int draw = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            int DisplayStart = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int DisplayLength = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var data = ClientService.Instance.GetClients(draw, DisplayStart, DisplayLength, search, out int recordsTotal);
            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddUpdateClient(int iClientId = 0)
        {
            //ViewBag.States = new List<SelectListItem>();
            ViewBag.States = UserService.Instance.GetGSTStates();
            if (iClientId == 0)
            {
                return PartialView("pvAddUpdateClient");
            }
            var model = ClientService.Instance.GetClientById(iClientId);
            return PartialView("pvAddUpdateClient", model);
        }
        [HttpPost]
        public JsonResult AddUpdateClient(ClientModel model)
        {

            if (ModelState.IsValid)
            {
                return Json(ClientService.Instance.SaveClient(model, GetUserInfo().iUserId));
            }
            else
                return Json(new ResponseStatus { Status = false, Message = string.Join(",", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage)) });
        }
        public JsonResult GetStates(int? iCountryId)
        {
            return Json(UserService.Instance.GetStates(iCountryId ?? 0), JsonRequestBehavior.AllowGet);
        }
    }
}