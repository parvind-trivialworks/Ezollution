using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class ClientModel
    {
        public int iSNo { get; set; }

        public int iClientID { get; set; }

        [Required(ErrorMessage ="Client name is a required field.")]
        [MaxLength(50,ErrorMessage ="Client name cannot exceed 50 characters.")]
        public string sClientName { get; set; }

        [MaxLength(50, ErrorMessage = "Client code cannot exceed 50 characters.")]
        public string sClientCode { get; set; }
        [Required(ErrorMessage = "CARN is a required field.")]
        [MaxLength(50, ErrorMessage = "CARN cannot exceed 50 characters.")]
        public string sCARN { get; set; }
        [MaxLength(250, ErrorMessage = "Office address cannot exceed 250 characters.")]
        public string sOfficeAddress { get; set; }
        [MaxLength(250, ErrorMessage = "Company Name cannot exceed 250 characters.")]
        public string sCompanyName { get; set; }
        [MaxLength(12, ErrorMessage = "Landline Number cannot exceed 12 characters.")]
        [Phone(ErrorMessage = "Please enter valid Landline number")]
        public string sLandLineNumber { get; set; }
        [MaxLength(10,ErrorMessage = "Mobile Number cannot exceed 10 characters.")]
        [Phone(ErrorMessage ="Please enter valid mobile number")]
        public string sMobileNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Fax Number cannot exceed 50 characters.")]
        public string sFaxNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Email Id cannot exceed 50 characters.")]
        public string sEmailID { get; set; }
        [Required(ErrorMessage ="IceGate SeaID is a required field.")]
        [MaxLength(50, ErrorMessage = "IceGate SeaID cannot exceed 50 characters.")]
        public string sICEGateSeaID { get; set; }
        public string sICEGateSeaPassword { get; set; }
        public string sICEGateAirID { get; set; }
        public string sICEGateAirPassword { get; set; }
        public Nullable<bool> bIsActive { get; set; }
        public Nullable<System.DateTime> dtCreatedDate { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dtModifiedDate { get; set; }
        public Nullable<int> iModifiedBy { get; set; }

        [MaxLength(15, ErrorMessage = "GST No cannot exceed 15 characters.")]
        [Display(Name = "GST No")]
        public string sGSTNNo { get; set; }

        [Display(Name = "Pincode")]
        [MaxLength(6, ErrorMessage = "Pincode cannot exceed 6 characters.")]
        public string sPinCode { get; set; }

        public string sStateName { get; set; }
        public string sStateCode { get; set; }

        public decimal? dPricePerUnit { get; set; }
    }

    public partial class vw_ClientMasterModel
    {
        public int iClientId { get; set; }
        public string ClientName { get; set; }
        public string iClientType { get; set; }
        public string sStateCode { get; set; }
        public string sClientCode { get; set; }
        public Nullable<decimal> dPricePerUnit { get; set; }
    }
}