using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EzollutionPro_BAL.Models
{
    public class ClientSearchModel
    {
        [Required(ErrorMessage ="This field is required")]
        public string ClientType { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int iClientId { get; set; }
        public string sMBLNumber { get; set; }
        
        public string sFromDate { get; set; }
        
        public string sToDate { get; set; }
        
        public string filterBy { get; set; }
        public string ReportName { get; set; }
    }

    public class ClientViewSearchModel
    {
        
      
        public int iClientId { get; set; }
        public string sMBLNumber { get; set; }

        public string sFromDate { get; set; }

        public string sToDate { get; set; }

        public string filterBy { get; set; }
        public string ReportName { get; set; }
    }
}
