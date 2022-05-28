using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Services;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EzollutionPro_BAL.Models
{
    public class ClientManagementSearchModel
    {     
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ClientManagementModel> _List { get; set; }
    }
    public class ClientManagementModel
    {
        public int iClientManagementId { get; set; }
        public string sClientCode { get; set; }
        public string dtDate { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public string sNewClientName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string sExistingClientName { get; set; }
        public string sContactPersonName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sContactPersonContactNo { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sDecisionMakerName { get; set; }
        public string sDecisionMakerContactNo { get; set; }
        public string sReference { get; set; }
        public string sBranch { get; set; }
        public string sAddress { get; set; }
        public string dtNextFollowUpDate { get; set; }
        public string sEmailId { get; set; }
        public string sServiceType { get; set; }
        public Nullable<decimal> dRateOffered { get; set; }
        public Nullable<decimal> dFinalRate { get; set; }
        public Nullable<decimal> dRevenueExpected { get; set; }
        public Nullable<decimal> dActualRevenue { get; set; }
        public string dtBusinessStartDate { get; set; }
        public string sAgreementImageName { get; set; }
        public Nullable<bool> blsActive { get; set; }

        public string dtAddedOn { get; set; }
        public List<ClientManagementFollowupModel> _List { get; set; }
    }
    public class ClientManagementFollowupModel
    {

        public int iClientManagementFollowupId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int iClientManagementId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sFollowUpType { get; set; }
        public string sDifference { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string sRemark { get; set; }
        public string sManagementRemark { get; set; }
        public Nullable<decimal> dRateOffered { get; set; }
        public Nullable<decimal> dFinalRate { get; set; }
        public Nullable<decimal> dRevenueExpected { get; set; }
        public Nullable<decimal> dActualRevenue { get; set; }
        public string dtNextFollowUpDate { get; set; }
        public string dtAddedOn { get; set; }

        public Nullable<bool> blsActive { get; set; }

    }
    
}
