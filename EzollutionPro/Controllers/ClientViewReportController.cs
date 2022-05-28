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
    public class ClientViewReportController : BaseController
    {
      
        public ActionResult Index()
        {
            //Report Code Start
            ViewBag.Clients = ClientService.Instance.GetAllClientsForDdl();
            ViewBag.ReportViewer = null;
            var model = new ClientViewSearchModel();
            model.sFromDate = System.DateTime.Now.FormatDate();
            model.sToDate = System.DateTime.Now.FormatDate();
            return View(model);
        }


       

        [HttpPost]
        public ActionResult Index(ClientViewSearchModel model)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ServerReport.ReportServerCredentials = new ReportCredentials(CommonConstant.rptUserName, CommonConstant.rptPassword, ".");
            reportViewer.ServerReport.ReportServerUrl = new Uri(CommonConstant.rptServerName); // Add the Reporting Server URL  
            if (model.ReportName == "DSR1111")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/invoiceMaster";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("clientId", Convert.ToString(this.GetUserInfo().iClientID));
                param[1] = new ReportParameter("ClienType", GetClientType());
                param[2] = new ReportParameter("fromDate", model.sFromDate);
                param[3] = new ReportParameter("toDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                
            }
            else if (model.ReportName == "PWI")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_DateWiseInvoice";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("ClientId", Convert.ToString(this.GetUserInfo().iClientID));
                param[1] = new ReportParameter("ClienType", GetClientType());
                param[2] = new ReportParameter("FromDate", model.sFromDate);
                param[3] = new ReportParameter("ToDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
               
            }
            else if (model.ReportName == "DSR")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_ClientDailyStatusReport";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[2];  //new ReportParameter[3];
                param[0] = new ReportParameter("FromDate", model.sFromDate);
                param[1] = new ReportParameter("ToDate", model.sToDate);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
              
            }
          
            else if (model.ReportName == "MAWB")
            {
                reportViewer.ServerReport.ReportPath = "/EZReport_Client/rpt_Air_IGM";
                reportViewer.ShowParameterPrompts = false;
                ReportParameter[] param = new ReportParameter[4];  //new ReportParameter[3];
                param[0] = new ReportParameter("clientId", Convert.ToString(this.GetUserInfo().iClientID));
                param[1] = new ReportParameter("fromDate", model.sFromDate);
                param[2] = new ReportParameter("toDate", model.sToDate);
                param[3] = new ReportParameter("filterBy", model.filterBy);
                reportViewer.ServerReport.SetParameters(param);
                reportViewer.ServerReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
              
            }
            return View(model);
        }
        public string GetClientType()
        {
            return "1";
        }

    }

   
}