using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models.Masters
{
    public class PODModel
    {
        public int iSNo { get; set; }
        public int iPortID { get; set; }
        [MaxLength(50,ErrorMessage ="Port Code cannot exceed 50 characters.")]
        [Required(ErrorMessage ="Port Code is a required field.")]
        public string sPortCode { get; set; }
        [MaxLength(50, ErrorMessage = "Port Name cannot exceed 50 characters.")]
        [Required(ErrorMessage = "Port Name is a required field.")]
        public string sPortName { get; set; }
        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string sDescription { get; set; }
        public bool bStatus { get; set; }
    }
}