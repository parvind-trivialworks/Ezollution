using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models.Masters
{
    public class AirLocationModel
    {
        public int iLocationId { get; set; }
        [Display(Name="Custom Location")]
        [Required(ErrorMessage = "Custom Location is a required field.")]
        [MaxLength(20,ErrorMessage ="Custom Location cannot exceed 20 characters.")]
        public string sCustomLocation { get; set; }
        [Display(Name="Custom Code")]
        [MaxLength(6,ErrorMessage ="Custom Location cannot exceed 6 characters.")]
        [Required(ErrorMessage = "Custom Code is a required field.")]
        public string sCustomCode { get; set; }
        [Display(Name="Three Letter Code")]
        [Required(ErrorMessage ="Three Letter Code is a required field.")]
        [MaxLength(3,ErrorMessage = "Three Letter Code cannot exceed 3 characters.")]
        public string sThreeLetterCode { get; set; }
    }
}
