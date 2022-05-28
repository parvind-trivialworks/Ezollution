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
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index()
        {
            //Report Code Start
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            var model = new InvoiceSearchModel();
            model.FromDate = System.DateTime.Now.AddMonths(-1).FormatDate();
            model.ToDate = System.DateTime.Now.FormatDate();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetInvoiceList()
        {
            string minDate = Request.Form.GetValues("FromDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("ToDate").FirstOrDefault() ?? "";
            string PaymentStatus= Request.Form.GetValues("PaymentStatus").FirstOrDefault() ?? "";
            int iClientType = Convert.ToInt32(Request.Form.GetValues("iClientType").FirstOrDefault());
            int iClientID = Convert.ToInt32(Request.Form.GetValues("iClientID").FirstOrDefault());
            var data = InvoiceService.Instance.GetInvoiceList(minDate,iClientID, maxDate,iClientType, PaymentStatus, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }
        //[HttpPost]
        //public ActionResult Index(InvoiceModel model)
        //{
        //    var Clients = ClientService.Instance.GetAllClientsForDdl();
        //    if (ModelState.IsValid)
        //    {
        //        model._InvoiceItemModel.iClientId = model.iClientId;
        //        model._InvoiceItemModel.iYear = model.iYear;
        //        model._InvoiceItemModel.iMonth = model.iMonth;
        //        InvoiceService.Instance.AddInvoiceItem(model._InvoiceItemModel, GetUserInfo().iUserId,model.iClientType);
        //        ViewBag.Message = "success";
        //        ModelState.Clear();
        //        var newmodel = new InvoiceModel();
        //        newmodel.iClientId = model.iClientId;
        //        newmodel.iYear = model.iYear;
        //        newmodel.iMonth = model.iMonth;
        //        if(newmodel.iClientId>0)
        //        newmodel._ItemList = InvoiceService.Instance.GetInvoiceItemsByClient(newmodel.iYear.GetValueOrDefault(), newmodel.iMonth.GetValueOrDefault(), newmodel.iClientId, model.iClientType);
        //        model = newmodel;
        //        ViewBag.Message = "success";
        //    }
        //    ViewBag.Clients = Clients;
        //    return View(model);
        //}

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

        [HttpPost]
        public JsonResult GetItemCount(string sHSN_SAC,int iInvoiceId)
        {
            var invoice= InvoiceService.Instance.GetInvoice(iInvoiceId);
            if(invoice!=null)
            {
               int Cnt= InvoiceService.Instance.GetItemCount(sHSN_SAC, invoice.iClientId, invoice.iClientType, 
                    invoice.FromInvoiceDate.ConvertDate().GetValueOrDefault(), invoice.ToInvoiceDate.ConvertDate().GetValueOrDefault());
                return Json(Cnt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult AddItem(InvoiceModel model)
        //{
        //    ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
        //    if (ModelState.IsValid)
        //    {
        //        model._InvoiceItemModel.iInvoiceID = model.iInvoiceID;
        //        InvoiceService.Instance.AddInvoiceItem(model._InvoiceItemModel, GetUserInfo().iUserId, model.iClientType);
        //        model._InvoiceItemModel = null;
        //        ModelState.Clear();

        //    }
        //    return View("Index",model);
        //}
        public ActionResult AddEdit(int? InvoiceId = 0)
        {

            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            ViewBag.Companies = CompanyService.Instance.GetCompaniesForDdl();                                                                                            
            var model = new InvoiceModel();
            if (InvoiceId.GetValueOrDefault() > 0)
            {
                model = InvoiceService.Instance.GetInvoice(InvoiceId.GetValueOrDefault());
                if(model==null)
                {
                    return RedirectToAction("AddEdit");
                }
            }
            else
            {
                model = new InvoiceModel();
                model.dtPaymentDate = System.DateTime.Now.FormatDate();
                model.dtReceivedDate = System.DateTime.Now.FormatDate();
                model.ToInvoiceDate = System.DateTime.Now.FormatDate();
                model.FromInvoiceDate = System.DateTime.Now.AddMonths(-1).FormatDate();
                model.iInvoiceID = InvoiceId.GetValueOrDefault();
                model.sPOS = "DELHI";
            }
            return View("AddEdit", model);
        }

        [HttpPost]
        public ActionResult AddEdit(InvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                int InvoiceId= InvoiceService.Instance.AddUpdateInvoice(model, GetUserInfo().iUserId);
                if (InvoiceId > 0)
                {
                    ViewBag.Message = "success";
                    return RedirectToAction("AddEdit", new { InvoiceId = InvoiceId });
                }
                else
                {
                    return RedirectToAction("AddEdit");
                }
            }
            else
            {
                ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
                ViewBag.Companies = CompanyService.Instance.GetCompaniesForDdl();
                return View("AddEdit", model);
            }
        }

        public PartialViewResult AddEditInvoiceItem(int iInvoiceID,int iInvoiceItemID=0)
        {
            var model = new InvoiceItemModel();
            if (iInvoiceItemID > 0)
            {
                model = InvoiceService.Instance.AddUdpdateInvoiceItem(iInvoiceItemID);
                model.IsStateCodeSame = InvoiceService.Instance.IsStateCodeSame(iInvoiceID);
                return PartialView("pvAddEditInvoiceItem", model);
            }
            else
            {
                if (iInvoiceID > 0)
                {
                    var invoice = InvoiceService.Instance.GetInvoice(iInvoiceID);
                    if (invoice != null)
                    {
                        var client = ClientService.Instance.GetVwClient(invoice.iClientId, invoice.iClientType.ToString());
                        if (client != null)
                        {
                            model.dAmountPerUnit = client.dPricePerUnit;
                        }
                    }
                }
                model.IsStateCodeSame = InvoiceService.Instance.IsStateCodeSame(iInvoiceID);
                model.iInvoiceID = iInvoiceID;
                return PartialView("pvAddEditInvoiceItem", model);
            }
        }

        [HttpPost]
        public JsonResult SaveInvoiceItem(InvoiceItemModel model)
        {
            if (ModelState.IsValid)
            {                
                return Json(InvoiceService.Instance.AddInvoiceItem(model, GetUserInfo().iUserId));
            }
            else
            {
                return Json(new ResponseStatus { Status = false, Message = ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage });
            }
        }

        [HttpPost]
        public JsonResult GetInvoiceItems(int iInvoiceID)
        {
            var data = InvoiceService.Instance.GetInvoiceItems(iInvoiceID);
            int recordsTotal = data.Count();
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public ActionResult Payments(int? InvoiceId = 0)
        {
            InvoicePaymentContainer model = new InvoicePaymentContainer();
            if (InvoiceId.GetValueOrDefault() > 0)
            {
                var invoice = InvoiceService.Instance.GetInvoice(InvoiceId.GetValueOrDefault());
                if (invoice == null)
                {
                    return View("Index", model);
                }
                else
                {
                    model._InvoiceModel = invoice;
                    return View("Payments", model);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }   
        }

        [HttpPost]
        public JsonResult GetInvoicePayments(int iInvoiceID)
        {
            var data = InvoiceService.Instance.GetInvoicePayments(iInvoiceID);
            int recordsTotal = data.Count();
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public PartialViewResult AddEditInvoicePayment(int iInvoiceID, int iInvoicePaymentID = 0)
        {
            var model = new InvoicePaymentModel();
            if (iInvoicePaymentID > 0)
            {
                model = InvoiceService.Instance.AddUdpdateInvoicePayment(iInvoicePaymentID);
                return PartialView("pvAddEditInvoicePayment", model);
            }
            else
            {
                model.iInvoiceId = iInvoiceID;
                model.dTds = 0;
                model.blsPaymentStatus = true;
                model.dtReceivedDate = System.DateTime.UtcNow.FormatDate();
                return PartialView("pvAddEditInvoicePayment", model);
            }
        }

        [HttpPost]
        public JsonResult SaveInvoicePayment(InvoicePaymentModel model)
        {
            int id = 0;
            if (ModelState.IsValid)
            {
                id = InvoiceService.Instance.SaveInvoicePayment(model, GetUserInfo().iUserId);
                if (id > 0)
                {
                    return Json(new ResponseStatus
                    {
                        Status = true,
                        Message = "Invoice payment updated successfully!"
                    });
                }
                else
                {
                    return Json(new ResponseStatus { Status = false, Message = "Some error occured, please try again" });
                }

            }
            else
            {
                return Json(new ResponseStatus { Status = false, Message = ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage });
            }
        }

        public ActionResult PaymentsList()
        {
            //Report Code Start
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            var model = new InvoicePaymentSearchModel();
           // model.FromDate = System.DateTime.Now.AddMonths(-1).FormatDate();
           // model.ToDate = System.DateTime.Now.FormatDate();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetPaymentsList()
        {
            
            string sInvoiceNo = Request.Form.GetValues("sInvoiceNo").FirstOrDefault() ?? "";
            string minDate = Request.Form.GetValues("FromDate").FirstOrDefault() ?? "";
            string maxDate = Request.Form.GetValues("ToDate").FirstOrDefault() ?? "";
            int iClientType = Convert.ToInt32(Request.Form.GetValues("iClientType").FirstOrDefault());
            int iClientID = Convert.ToInt32(Request.Form.GetValues("iClientID").FirstOrDefault());
            var data = InvoiceService.Instance.GetPaymentsList(minDate, iClientID, maxDate, iClientType, sInvoiceNo, out int recordsTotal);
            return Json(new { recordsFiltered = recordsTotal, recordsTotal, data });
        }

    }
}