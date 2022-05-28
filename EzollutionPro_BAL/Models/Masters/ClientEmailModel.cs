using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models.Masters
{

    public class EmailSearchModel
    {
        public int iClientId { get; set; }
        public Int16 iClientType { get; set; }

    }
    public class ClientEmailModel
    {
        public ClientEmailModel()
        {
            blsActive = true;
        }
        public int iMailId { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public int? iClientType { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int? iClientId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string sEmailId { get; set; }
        public string sEmailPersonName { get; set; }
        public bool bIsDefault { get; set; }
        public bool blsActive { get; set; }
        public string sClientName { get; set; }

    }
}
