using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models
{
    public class LoginModel
    {
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [Required(ErrorMessage = "Username is a required field.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain alphabets and number without any white spaces.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is a required field.")]
        [MaxLength(12, ErrorMessage = "Password cannot exceed 12 characters.")]
        public string Password { get; set; }
    }

}