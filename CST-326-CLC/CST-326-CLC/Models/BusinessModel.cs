using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class BusinessModel
    {
        public int businessID { get; set; }

        [Required(ErrorMessage = "The First Name field is required.")]
        [StringLength(25, ErrorMessage = "The First Name should not exceed 25 characters.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [StringLength(25, ErrorMessage = "The Last Name should not exceed 25 characters.")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "The Company Name field is required.")]
        public string companyName { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required.")]
        [StringLength(15, ErrorMessage = "The Phone Number should not exceed 15 characters.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        [DataType(DataType.EmailAddress)]
        public string companyEmail { get; set; }

        [StringLength(50, ErrorMessage = "The Username should not exceed 50 characters.")]
        public string username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The Password should not exceed 50 characters.")]
        public string password { get; set; }
        public bool isBusinessAccount { get; set; }
        public bool isAdmin { get; set; }
    }
}