using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST_326_CLC.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View("PaymentDetails");
        }

        /* Refactor to work with payment form */

        [HttpPost]
        public ActionResult ProcessPayment(PaymentInformation model)
        {
            Log.Information("User attempting to create shipment...");

            ShipmentInformation shipmentInfo = null;
            CreditService creditService = new CreditService();

            if(!creditService.CheckCard(model.CreditCard))
            {
                creditService.StoreCreditCard(model.CreditCard, 8);
            }

            if(creditService.ProcessCard(model.CreditCard.cardNumber))
            {
                shipmentInfo = UserManagement.Instance._currentShipment;
            }
            else
            {
                return View("Error");
            }

            decimal shipmentCost = shipmentInfo.shipment.CalculateCost(shipmentInfo.recipient.zip, shipmentInfo.shipment.Length, shipmentInfo.shipment.Width,
                shipmentInfo.shipment.Height, shipmentInfo.shipment.Weight, shipmentInfo.shipment.DeliveryOption);
            
            ShipmentService shipmentService = new ShipmentService();
            if(shipmentService.TestNewShipment(shipmentInfo))
            {
                PaymentService paymentService = new PaymentService();
                int shipmentID = UserManagement.Instance.shipmentInsert;
                if (paymentService.CreateTransaction(UserManagement.Instance.shipmentInsert, shipmentCost))
                {
                    UserManagement.Instance.shipmentInsert = 0;
                    UserManagement.Instance._currentShipment = null;
                    return Content(String.Format("Created shipment: {0} Total cost: {1}", shipmentID, shipmentCost));
                }
                else
                {
                    UserManagement.Instance.shipmentInsert = 0;
                    UserManagement.Instance._currentShipment = null;
                    return Content(String.Format("Failed to create shipment: {0}", shipmentID));
                }
            }
            else
            {
                return View("Error");
            }
        }
    }

    public class PaymentInformation
    {
        public ShipmentModel Shipment { get; set; }
        public AddressModel Billing { get; set; }
        public CreditCardModel CreditCard { get; set; }
    }
}