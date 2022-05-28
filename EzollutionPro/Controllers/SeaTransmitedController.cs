using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class SeaTransmitedController : BaseController
    {
        // GET: SeaManifested
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetTransmitedData()
        {
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault() ?? "";
            var data = SeaTransmitedService.Instance.GetScheduling(minDate, maxDate, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult TransmitFile(int iSchedulingId, string sMBLNumber)
        {
            var fileData = SeaTransmitedService.Instance.GetTransmitFileData(iSchedulingId);
            byte[] btFile = Encoding.ASCII.GetBytes(fileData);
            return File(btFile, System.Net.Mime.MediaTypeNames.Application.Octet, sMBLNumber + ".cgm");
        }
    }
}