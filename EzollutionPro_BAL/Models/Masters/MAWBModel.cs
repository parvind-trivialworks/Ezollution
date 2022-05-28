using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models.Masters
{
    public class MAWBModel
    {
        public long iMAWBId { get; set; }

        [Display(Name = "IGM Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "IGM Number must be numeric")]
        [MaxLength(7, ErrorMessage = "IGM Number cannot exceed 7 characters.")]
        public string sIGMNumber { get; set; }

        [Display(Name = "IGM Date")]
        public string sIGMDate { get; set; }

        [MaxLength(15, ErrorMessage = "Flight Number cannot exceed 15 characters.")]
        [Display(Name = "Flight Number")]
        public string sFlightNumber { get; set; }

        [Display(Name = "Flight Date")]
        public string sFlightDate { get; set; }

        [Display(Name = "MAWB Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "MAWB Number must be numeric")]
        [MaxLength(11, ErrorMessage = "MAWB Number should have 11 characters.")]
        [MinLength(11, ErrorMessage = "MAWB Number should have 11 characters.")]
        [Required(ErrorMessage = "MAWB is a required field.")]
        public string sMAWBNo { get; set; }

        [Required(ErrorMessage = "Origin is a required field.")]
        [MaxLength(3, ErrorMessage = "Origin should have 3 characters.")]
        [MinLength(3, ErrorMessage = "Origin should have 3 characters.")]
        [Display(Name = "Origin")]
        public string sOrigin { get; set; }

        [Required(ErrorMessage = "Destination is a required field.")]
        [MaxLength(3, ErrorMessage = "Destination should have 3 characters.")]
        [MinLength(3, ErrorMessage = "Destination should have 3 characters.")]
        [Display(Name = "Destination")]
        public string sDestination { get; set; }

        [Display(Name = "Packages")]
        [Required(ErrorMessage = "Packages is a required field.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Packages must be numeric")]
        [MaxLength(7, ErrorMessage = "Packages cannot exceed 7 characters.")]
        public string sPackages { get; set; }

        [Display(Name = "Weight")]
        [Required(ErrorMessage = "Weight is a required field.")]
        [RegularExpression(@"^(0|-?\d{0,9}(\.\d{0,3})?)$", ErrorMessage = "Weight cannot exceed 9 digits and 3 decimals.")]
        public string sWeight { get; set; }
        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "Client Name is a required field.")]
        public int iAirClientId { get; set; }
        [Display(Name = "Custom Location")]
        [Required(ErrorMessage = "Location is a required field.")]
        public int iLocationId { get; set; }
        [MaxLength(10, ErrorMessage = "CARN No should be of 10 characters.")]
        [MinLength(10, ErrorMessage = "CARN No should be of 10 characters.")]
        [Required(ErrorMessage = "CARN No is a required field.")]
        [Display(Name = "CARN No")]
        public string sCARNNo { get; set; }
    }


    public class MAWBMaster
    {
        public long iMAWBId { get; set; }
        public string sMAWBNo { get; set; }
        public string sOrigin { get; set; }
        public string sDestination { get; set; }
        public decimal sPackages { get; set; }
        public decimal sWeight { get; set; }
        public List<HAWBMaster> lstHAWBMasters { get; set; }
    }

    public class HAWBMaster
    {
        public long iHAWBId { get; set; }
        public string sHAWBNo { get; set; }
        public string sOrigin { get; set; }
        public string sDestination { get; set; }
        public decimal sPackages { get; set; }
        public decimal sWeight { get; set; }
        public string sDescription { get; set; }
    }


    public class HAWBModel
    {

        [Required(ErrorMessage = "MAWB is a required field.")]
        public long iMAWBId { get; set; }
        public long iHAWBId { get; set; }

        [Display(Name = "HAWB Number")]
        [MaxLength(20, ErrorMessage = "HAWB Number cannot exceed 20 characters.")]
        [Required(ErrorMessage = "HAWB is a required field.")]
        public string sHAWBNo { get; set; }

        [Required(ErrorMessage = "Origin is a required field.")]
        [MaxLength(3, ErrorMessage = "Origin should have 3 characters.")]
        [MinLength(3, ErrorMessage = "Origin should have 3 characters.")]
        [Display(Name = "Origin")]
        public string sOrigin { get; set; }

        [Required(ErrorMessage = "Destination is a required field.")]
        [MaxLength(3, ErrorMessage = "Destination should have 3 characters.")]
        [MinLength(3, ErrorMessage = "Destination should have 3 characters.")]
        [Display(Name = "Destination")]
        public string sDestination { get; set; }

        [Display(Name = "Packages")]
        [Required(ErrorMessage = "Packages is a required field.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Packages must be numeric")]
        [MaxLength(7, ErrorMessage = "Packages cannot exceed 7 characters.")]
        public string sPackages { get; set; }

        [Display(Name = "Weight")]
        [Required(ErrorMessage = "Weight is a required field.")]
        [RegularExpression(@"^(0|-?\d{0,9}(\.\d{0,3})?)$", ErrorMessage = "Weight cannot exceed 9 digits and 3 decimals.")]
        public string sWeight { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is a required field.")]
        [MaxLength(30, ErrorMessage = "Description cannot exceed 30 characters.")]
        public string sDescription { get; set; }
    }

    public class DropDownData
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public enum AirSchedulingEnum
    {
        Scheduled = 0,
        Transmited = 1,
        ProceedToTransmit=2
    }
}
