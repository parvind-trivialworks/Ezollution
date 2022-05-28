using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model,string returnUrl)
        {
            try
            {
                if(LoginService.Instance.ValidateUsername(model.Username))
                {
                    var data = LoginService.Instance.ValidateUser(model);
                    if (data == null)
                    {
                        ModelState.AddModelError("Password", "Invalid Password");
                    }
                    else
                    {
                        Session["UserInfo"] = data;
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index","Home");
                        else
                            return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Invalid Username");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult DownloadCheckList(int iSchedulingId)
        {
            var pdfPath = Server.MapPath("~/Content/file/") + "abc.pdf";
            if (System.IO.File.Exists(pdfPath))
            {
                System.IO.File.Delete(pdfPath);
            }
            GeneratePDFService generate = new GeneratePDFService(pdfPath, SeaSchedulingService.Instance.GeneratePDFData(iSchedulingId), pdfPath);
            generate.SetOrientation(TemplateOrientation.LANDSCAPE);
            generate.AddFonts(new PDFFonts().init());
            generate.Start();
            return Json("");
        }
    }
}