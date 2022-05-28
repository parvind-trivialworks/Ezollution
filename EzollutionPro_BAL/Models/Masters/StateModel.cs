using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class StateModel
    {
        public int iStateId { get; set; }
        public int iCountryId { get; set; }
        [MaxLength(200,ErrorMessage ="Country Name cannot exceed 200 characters.")]
        public string sCountryName { get; set; }
        [Required(ErrorMessage = "State Name is a required field.")]
        [MaxLength(200, ErrorMessage = "State Name cannot exceed 200 characters.")]
        public string sStateName { get; set; }
        [Required(ErrorMessage = "State Code is a required field.")]
        [MaxLength(50, ErrorMessage = "State Code cannot exceed 50 characters.")]
        public string sStateCode { get; set; }
        [MaxLength(200, ErrorMessage = "State Description cannot exceed 200 characters.")]
        public string sStateDescription { get; set; }
    }
}