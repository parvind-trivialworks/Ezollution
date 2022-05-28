using EzollutionPro_BAL.Services;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class FormIIIController : BaseController
    {
        // GET: FormIII
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetManifestedData()
        {
            string minDate = Request.Form.GetValues("minDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("maxDate").FirstOrDefault() ?? "";
            var data = SeaManifestedService.Instance.GetFORMIIIScheduling(minDate, maxDate, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }
        public ActionResult DownloadFormIIIData(int iSchedulingId)
        {
            var data = SeaManifestedService.Instance.GenerateFormIIIData(iSchedulingId);
            if (data != null)
            {
                var pdfPath = Server.MapPath("~/Content/file/") + "FORMIII_" + data.MBLNumber + ".pdf";
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
                GeneratePDFService generate = new GeneratePDFService(pdfPath, data, pdfPath);
                generate.SetOrientation(TemplateOrientation.LANDSCAPE);
                generate.AddFonts(new PDFFonts().init());
                generate.StartFormIII();
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                string fileName = "FORMIII_" + data.MBLNumber + ".pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "data is null");
        }


    }
}