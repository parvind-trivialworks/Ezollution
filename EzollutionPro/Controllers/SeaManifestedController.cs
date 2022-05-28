using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class SeaManifestedController : BaseController
    {
        // GET: SeaManifested
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetManifestedData()
        {
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault() ?? "";
            var data = SeaManifestedService.Instance.GetScheduling(minDate, maxDate, out int recordsTotal);
            var jsonResult = Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}