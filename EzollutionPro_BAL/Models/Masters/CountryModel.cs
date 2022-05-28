using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class CountryModel
    {
        public int iCountryId { get; set; }
        [Required(ErrorMessage="Country Name is a required field.")]
        [MaxLength(200,ErrorMessage ="Country Name cannot exceed 200 characters.")]
        public string sCountryName { get; set; }
        [Required(ErrorMessage = "Country Code is a required field.")]
        [MaxLength(50, ErrorMessage = "Country Code cannot exceed 50 characters.")]
        public string sCountryCode { get; set; }
        [Required(ErrorMessage = "Country Phone Code is a required field.")]
        [MaxLength(10, ErrorMessage = "Country Phone Code cannot exceed 10 characters.")]
        public string sCountryPhoneCode { get; set; }
        [Required(ErrorMessage = "Currency Code is a required field.")]
        [MaxLength(2, ErrorMessage = "Currency Code cannot exceed 2 characters.")]
        public string sCurrencyCode { get; set; }
        [Required(ErrorMessage = "Country Description is a required field.")]
        [MaxLength(200, ErrorMessage = "Country Description cannot exceed 10 characters.")]
        public string sCountryDescription { get; set; }
        [MaxLength(200, ErrorMessage = "Currency Description cannot exceed 10 characters.")]
        public string sCurrencyDescription { get; set; }
        public HttpPostedFileBase CountryImage { get; set; }
        public string sCountryImageUrl { get; set; }

    }
}