using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models
{
    public class RoleModel
    {
        public int iRoleId { get; set; }
        [Required(ErrorMessage ="Role Name is a required field.")]
        [MaxLength(100,ErrorMessage ="Role Name cannot exceed 100 characters.")]
        public string sRoleName { get; set; }
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string sDescription { get; set; }
        public bool bIsClient { get; set; }
    }
}