using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class BusinessLoginModel
    {
        [Required(ErrorMessage = "The Username field is required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [DataType(DataType.Password)]
        public string confirmPass { get; set; }

        public string securityQuestion { get; set; }

        [Required(ErrorMessage = "The Security Answer field is required.")]
        public string securityAnswer { get; set; }
    }
}