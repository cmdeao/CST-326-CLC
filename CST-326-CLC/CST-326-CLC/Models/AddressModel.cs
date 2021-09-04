using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST_326_CLC.Models
{
    public class AddressModel
    {
        public int addressID { get; set; }

        public string aptSuite { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string address { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string city { get; set; }

        public string state { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public string country { get; set; }

        [Required(ErrorMessage = "The Zip field is required.")]
        public int zip { get; set; }

        public int countryCode { get; set; }

    }
}