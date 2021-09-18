using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CST_326_CLC.Models
{
    public class CreditCardModel
    {
        public enum CardType
        {
            Visa,
            MasterCard,
            AmericanExpress,
            Discover
        }

        [Required(ErrorMessage = "The Card Holder Name field is required.")]
        public string cardHolderName { get; set; }

        [Required(ErrorMessage = "The Card Type field is required")]
        public CardType cardType { get; set; }

        [Required(ErrorMessage = "The Card Number field is required.")]
        public long cardNumber { get; set; }

        [Required(ErrorMessage = "The CVV Code field is required.")]
        public int cvvCode { get; set; }

        [Required(ErrorMessage = "The Expiration Month field is required.")]
        public int expirationMonth { get; set; }

        [Required(ErrorMessage = "The Expiration Year field is required.")]
        public int expirationYear { get; set; }

    }
}
