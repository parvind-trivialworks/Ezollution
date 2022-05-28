using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class CityModel
    {
        public int iCityId { get; set; }
        public int iCountryId { get; set; }
        public string sCountryName { get; set; }
        public string sStateName { get; set; }
        [Required(ErrorMessage = "City Name is a required field.")]
        [MaxLength(200, ErrorMessage = "City Name cannot exceed 200 characters.")]
        public string sCityName { get; set; }
        [MaxLength(200, ErrorMessage = "City Description cannot exceed 200 characters.")]
        public string sCityDescription { get; set; }
        public int iStateId { get; set; }
    }
}