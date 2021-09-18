using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST_326_CLC.Services.Business
{
    public class PaymentService
    {
        PaymentDAO service = new PaymentDAO();

        public bool CreateTransaction(int shipmentID, decimal amount)
        {
            return service.CreateTransaction(shipmentID, amount);
        }
        
        public Dictionary<int, decimal> RetrieveTransactions()
        {
            return service.RetrieveTransaction();
        }
    }
}