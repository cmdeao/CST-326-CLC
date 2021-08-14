using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The Username field is required for submission.")]
        [StringLength(50, ErrorMessage = "The Username should not exceed 50 characters.")]
        public string username { get; set; }

        [Required(ErrorMessage = "The Password field is required for submission.")]
        [StringLength(50, ErrorMessage = "The Password should not exceed 50 characters.")]
        public string password { get; set; }
    }
}