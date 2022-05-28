using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class ShippingLineModel
    {
        public int iSNo { get; set; }
        public int iShippingID { get; set; }
        [Required(ErrorMessage ="MLO Code is a required field.")]
        [MaxLength(10, ErrorMessage = "MLO Code should have 10 characters.")]
        [MinLength(10, ErrorMessage = "MLO Code should have 10 characters.")]
        public string sMLOCode { get; set; }
        [Required(ErrorMessage = "Shipping Line Name is a required field.")]
        public string sShippingLineName { get; set; }
        [MaxLength(50, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string sDescription { get; set; }
        public bool bStatus { get; set; }
    }
}