using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models
{
    public class AirEGMFlightModel
    {
        public int iFlightId { get; set; }
        [Required(ErrorMessage ="Client Name is a required field.")]
        public int iClientId { get; set; }
        [Required(ErrorMessage ="Custom Location is a required field.")]
        public int iLocationId { get; set; }
        [Required(ErrorMessage ="Flight No is a required field.")]
        [MaxLength(15,ErrorMessage ="Flight No cannot exceed 15 characters.")]
        public string sFlightNo { get; set; }
        [Required(ErrorMessage ="Flight Date is a required field.")]
        public string sFlightDate { get; set; }
        [MaxLength(7,ErrorMessage ="EGM No cannot exceed 7 characters.")]
        [Required(ErrorMessage = "EGM No is a required field.")]
        public string sEGMNo { get; set; }
        [Required(ErrorMessage ="Port of Origin is a required field.")]
        [MaxLength(3, ErrorMessage = "Port of Origin cannot exceed 3 characters.")]
        public string sPortOfOrigin { get; set; }
        [Required(ErrorMessage = "Port of Destination is a required field.")]
        [MaxLength(3, ErrorMessage = "Port of Destination cannot exceed 3 characters.")]
        public string sPortOfDestination { get; set; }
        [Required(ErrorMessage = "Flight Registration No is a required field.")]
        [MaxLength(10, ErrorMessage = "Port of Destination cannot exceed 10 characters.")]
        public string sFlightRegistrationNo { get; set; }
        public object sClientName { get; internal set; }
        public int sNo { get; internal set; }
        public string sLocation { get; set; }
        public string sDateTime { get; set; }
        public string sUserName { get; set; }
    }

    public class AirEGMMAWBModel
    {
        public int iFlightId { get; set; }
        public int iAirEGMMAWBId { get; set; }
        [Required(ErrorMessage ="MAWB No is a required field.")]
        [MaxLength(11,ErrorMessage ="MAWB No should have 11 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "MAWB No must be numeric")]
        [MinLength(11, ErrorMessage = "MAWB No should have 11 characters.")]
        public string sMAWBNo { get; set; }
        [MaxLength(3,ErrorMessage ="Port of Origin should have 3 characters")]
        [MinLength(3,ErrorMessage = "Port of Origin should have 3 characters")]
        [Required(ErrorMessage ="Port of Origin is a required field.")]
        public string sPortOfOrigin { get; set; }
        [Required(ErrorMessage ="Port of Destination is a required field.")]
        [MaxLength(3,ErrorMessage ="Port of Destination should have 3 characters.")]
        [MinLength(3,ErrorMessage = "Port of Destination should have 3 characters.")]
        public string sPortOfDestination { get; set; }
        [Required(ErrorMessage ="No of Packages is a required field.")]
        [MaxLength(8,ErrorMessage ="No of Packages cannot exceed 8 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No of Packages must be numeric")]
        public string sNoOfPackages { get; set; }
        [Display(Name = "Total Weight")]
        [Required(ErrorMessage = "Total Weight is a required field.")]
        [RegularExpression(@"^(0|-?\d{0,9}(\.\d{0,3})?)$", ErrorMessage = "Total Weight cannot exceed 9 digits and 3 decimals.")]
        public string sTotalWeight { get; set; }
        [Required(ErrorMessage ="Goods Description is a required field.")]
        [MaxLength(30,ErrorMessage ="Goods Description cannot exceed 30 characters.")]
        public string sGoodsDescription { get; set; }
        public string cShipmentType { get; set; }

    }


    public class AirEGMFlightViewModel
    {
        public int iFlightId { get; set; }
        public int iClientId { get; set; }
        public int iLocationId { get; set; }
        public string sFlightNo { get; set; }
        public string sFlightDate { get; set; }
        public string sEGMNo { get; set; }
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public string sFlightRegistrationNo { get; set; }
        public object sClientName { get; internal set; }
        public int sNo { get; internal set; }
        public List<AirEGMMAWBViewModel> lstAirEGMMAWBViewModel { get; set; }
    }

    public class AirEGMMAWBViewModel
    {
        public int iFlightId { get; set; }
        public int iAirEGMMAWBId { get; set; }
        public string sMAWBNo { get; set; }
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public decimal sNoOfPackages { get; set; }
        public decimal sTotalWeight { get; set; }
        public string sGoodsDescription { get; set; }
        public string cShipmentType { get; set; }
    }

}
