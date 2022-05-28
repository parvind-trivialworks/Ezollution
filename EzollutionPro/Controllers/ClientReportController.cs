using EzollutionPro_BAL;
using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Services.MasterServices;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EzollutionPro.Controllers
{
    public class ClientReportController : BaseController
    {
        //string rptUserName = "administrator";// ConfigurationManager.AppSettings["rptServerUserName"];
        //string rptPassword = "8r.joPEHx7&srYIdhfA6L8ELX$=wzQ8r";// ConfigurationManager.AppSettings["rptServerPassword"];
        //string rptServerName = ConfigurationManager.AppSettings["rptServerUrl"];
        // GET: ClientReport
        public ActionResult Index()
        {
            //Report Code Start
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            ViewBag.ReportViewer = null;
            var model = new ClientSearchModel();
            model.sFromDate = (System.DateTime.Now.AddDays(-7)).FormatDate();
            model.sToDate = System.DateTime.Now.FormatDate();
            return View(model);
        }


        [HttpPost]
        public JsonResult GetClients(string ClientType)
        {
            if (ClientType == "sea")
            {
                return Json(ClientService.Instance.GetAllClientsForDdl(), JsonRequestBehavior.AllowGet);
            }
            else if (ClientType == "air")
            {
                return Json(AirClientService.Instance.GetAllClientsForDdl(), JsonRequestBehavior.AllowGet);   
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(ClientSearchModel model)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ServerReport.ReportServerCredentials = new ReportCredentials(CommonConstant.rptUserName, CommonConstant.rptPassword, ".");
            reportViewer.ServerReport.ReportServerUrl = new Uri(CommonConstant.rptServerName); // Add the Reporting Server URL  
            if (model.ClientType == "sea" && model.ReportName == "MWR")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_ClientReportMBL";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("clientId", Convert.ToString(model.iClientId));
                param[1] = new ReportParameter("fromDate", model.sFromDate);
                param[2] = new ReportParameter("toDate", model.sToDate);
                param[3] = new ReportParameter("filterBy", model.filterBy);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                ClientBind(model.ClientType);
            }
            else if (model.ReportName == "INV")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/invoiceMaster";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("clientId", Convert.ToString(model.iClientId));
                param[1] = new ReportParameter("ClienType", GetClientType(model.ClientType));
                param[2] = new ReportParameter("fromDate", model.sFromDate);
                param[3] = new ReportParameter("toDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                ClientBind(model.ClientType);
            }
            else if (model.ReportName == "PWI")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_DateWiseInvoice";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("ClientId", Convert.ToString(model.iClientId));
                param[1] = new ReportParameter("ClienType", GetClientType(model.ClientType));
                param[2] = new ReportParameter("FromDate", model.sFromDate);
                param[3] = new ReportParameter("ToDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                ClientBind(model.ClientType);
            }
            else if (model.ReportName == "DWI")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_AllDateWiseInvoice";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[2];  //new ReportParameter[3];
                param[0] = new ReportParameter("FromDate", model.sFromDate);
                param[1] = new ReportParameter("ToDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                ClientBind(model.ClientType);
            }
            //else if (model.ClientType == "air" && model.ReportName == "INV")
            //{
            //    reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_Invoice";
            //    reportViewer.ShowParameterPrompts = false;
            //    ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
            //    param[0] = new ReportParameter("clientId", Convert.ToString(model.iClientId));
            //    param[1] = new ReportParameter("ClienType", "1");
            //    reportViewer.ServerReport.SetParameters(param);
            //    reportViewer.ServerReport.Refresh();
            //    ViewBag.ReportViewer = reportViewer;
            //    ClientBind(model.ClientType);
            //}
            else if (model.ClientType == "air" && model.ReportName == "MAWB")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_Air_IGM";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("clientId", Convert.ToString(model.iClientId));
                param[1] = new ReportParameter("fromDate", model.sFromDate);
                param[2] = new ReportParameter("toDate", model.sToDate);
                param[3] = new ReportParameter("filterBy", model.filterBy);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                ClientBind(model.ClientType);
            }
            return View(model);
        }
        public string GetClientType(string clientType)
        {
            return clientType == "sea" ? "0" : "1";
        }

        public void ClientBind(string clienttype)
        {
            if (clienttype == "sea")
                ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            if (clienttype == "air")
                ViewBag.Clients = AirClientService.Instance.GetAllClientsForDdl();
        }
    }

   
}