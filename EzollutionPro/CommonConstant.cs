using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EzollutionPro
{
    public class CommonConstant
    {
        public const string rptUserName = "administrator";// ConfigurationManager.AppSettings["rptServerUserName"];
        public const string rptPassword = "97J8lYeS=9LJ;U4q;bk*LkrVBF4Mp4Bk";// ConfigurationManager.AppSettings["rptServerPassword"];
        public static string rptServerName = ConfigurationManager.AppSettings["rptServerUrl"];
    }
    public class ReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        string _userName, _password, _domain;
        public ReportCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                return new System.Net.NetworkCredential(_userName, _password, _domain);
            }
        }
        public bool GetFormsCredentials(out System.Net.Cookie authCoki, out string userName, out string password, out string authority)
        {
            userName = _userName;
            password = _password;
            authority = _domain;
            authCoki = new System.Net.Cookie(".ASPXAUTH", ".ASPXAUTH", "/", "Domain");
            return true;
        }


    }
}