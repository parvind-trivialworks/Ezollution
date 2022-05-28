using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models.Masters
{
    public class AirClientModel
    {
        public int iAirClientId { get; set; }
        [Required(ErrorMessage="Client Name is a required field.")]
        [MaxLength(20,ErrorMessage="Client Name cannot exceed 20 characters.")]
        [Display(Name ="Client Name")]
        public string sClientName { get; set; }
        [Required(ErrorMessage ="Password is a required field.")]
        [MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters.")]
        [Display(Name = "Password")]
        public string sPassword { get; set; }
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Display(Name = "Address")]
        public string sAddress { get; set; }
        [MaxLength(15, ErrorMessage = "GST No cannot exceed 15 characters.")]
        [Display(Name = "GST No")]
        public string sGSTNo { get; set; }
        [MaxLength(10, ErrorMessage = "CARN No should be of 10 characters.")]
        [MinLength(10, ErrorMessage = "CARN No should be of 10 characters.")]
        [Required(ErrorMessage = "CARN No is a required field.")]
        [Display(Name = "CARN No")]
        public string sCARNNo { get; set; }
        [MaxLength(20,ErrorMessage ="ICEGate Id cannot exceed 20 characters.")]
        [Display(Name = "ICEGate Id")]
        [Required(ErrorMessage = "ICEGate Id is a required field.")]
        public string sICEGateId { get; set; }

        [Display(Name ="Company Name")]
        [MaxLength(200,ErrorMessage = "Company Name cannot exceed 70 characters.")]
        [Required(ErrorMessage = "Company Name is a required field.")]
        public string sCompanyName { get; set; }

        [Display(Name = "Pincode")]
        [MaxLength(6, ErrorMessage = "Pincode cannot exceed 6 characters.")]
       // [Required(ErrorMessage = "Company Name is a required field.")]
        public string sPinCode { get; set; }

        [Display(Name = "Email Id")]
        [MaxLength(60, ErrorMessage = "Email Id cannot exceed 60 characters.")]
        //[Required(ErrorMessage = "Company Name is a required field.")]
        public string sEmailId { get; set; }

        public string sStateName { get; set; }
        public string sStateCode { get; set; }

        public decimal? dPricePerUnit { get; set; }
    }
}
