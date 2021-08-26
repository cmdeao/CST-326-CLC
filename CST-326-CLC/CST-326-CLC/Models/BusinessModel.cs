using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [Required(ErrorMessage = "The Address field is required.")]
        public string companyAddress { get; set; }

        public string suite { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string city { get; set; }

        public string state { get; set; }

        [Required(ErrorMessage = "The Zip Code field is required.")]
        public int zipCode { get; set; }

        [Required(ErrorMessage = "The Country / Territory field is required.")]
        public string country { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required.")]
        [StringLength(15, ErrorMessage = "The Phone Number should not exceed 15 characters.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        [DataType(DataType.EmailAddress)]
        public string companyEmail { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public bool isBusinessAccount { get; set; }
        public bool isAdmin { get; set; }
    }
}