using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class PersonalCredsModel
    {
        [Required(ErrorMessage = "The Username field is required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "The Password Confirmation field is required.")]
        [DataType(DataType.Password)]
        public string confirmPass { get; set; }
    }
}