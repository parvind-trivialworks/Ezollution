using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models.Masters
{
    
    public class BondModel
    {
        public int iBondId { get; set; }
        [Required(ErrorMessage="Shipping Line is a required field.")]
        public Nullable<int> iShippingId { get; set; }
        [Required(ErrorMessage ="Port of Destination is a required field.")]
        public Nullable<int> iPODId { get; set; }
        [Required(ErrorMessage = "Final Destination is a required field.")]
        public Nullable<int> iFPODId { get; set; }
        [Required(ErrorMessage = "Bond Number is a required field.")]
        [Range(0,9999999999,ErrorMessage ="Bond Number cannot exceed 10 characters and must be a valid number.")]
        public Nullable<decimal> nBondNo { get; set; }
        [MaxLength(10,ErrorMessage ="Carrier Code cannot exceed 10 characters.")]
        [Required(ErrorMessage = "Carrier Code is a required field.")]
        public string sCarrierCode { get; set; }
        [Required(ErrorMessage ="Mode of Transport is a required field.")]
        public string sModeOfTransport { get; set; }
        [Required(ErrorMessage ="CFS Name is a required field.")]
        [MaxLength(50,ErrorMessage = "CFS Name cannot exceed 50 characters.")]
        public string sCFSName { get; set; }
        [Required(ErrorMessage = "CFS Code is a required field.")]
        [MaxLength(10, ErrorMessage = "CFS Code cannot exceed 10 characters.")]
        public string sCFSCode { get; set; }
        [Required(ErrorMessage ="Cargo Movement is a required field.")]
        public string sCargoMovement { get; set; }
        [Required(ErrorMessage ="MLO Code is a required field.")]
        [MaxLength(10,ErrorMessage = "MLO Code should have 10 characters.")]
        [MinLength(10, ErrorMessage = "MLO Code should have 10 characters.")]
        public string sMLOCode { get; set; }
    }
}
