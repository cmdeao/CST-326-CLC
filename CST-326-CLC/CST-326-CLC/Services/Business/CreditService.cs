using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST_326_CLC.Services.Business
{
    public class CreditService
    {
        CreditDAO service = new CreditDAO();
        public bool StoreCreditCard(CreditCardModel model, int userID)
        {
            return service.StoreCredit(model, userID);
        }

        public List<CreditCardModel> RetrieveCards(int userID)
        {
            return service.RetrieveCards(userID);
        }

        public bool CheckCard(long cardNumber)
        {
            return service.CheckCard(cardNumber);
        }
    }
}