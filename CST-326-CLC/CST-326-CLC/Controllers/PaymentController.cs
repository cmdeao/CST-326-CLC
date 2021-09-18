using CST_326_CLC.Models;
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
    }

    /* Refactor to work with payment form */

    //[HttpPost]
    //public ActionResult ProcessPayment(ShipmentModel model)
    //{
    //    Log.Information("User attempting to create shipment...");

    //    // Can be used to display cost to user
    //    decimal shipmentCost = model.CalculateCost(model.Zip, model.Length, model.Width,
    //        model.Height, model.Weight, model.DeliveryOption);

    //    if (!ModelState.IsValid)
    //    {
    //        Log.Information("Create Shipment: The ModelState was invalid.");
    //        return View("ShipmentIndexTEST");
    //    }

    //    ShipmentService service = new ShipmentService();

    //    if (service.AddShipment(model))
    //    {
    //        Log.Information("Create Shipment: Shipment succesfully created ID: {0}", model.ShipmentId);
    //        return Content(String.Format("Created shipment: {0}", model.ShipmentId));
    //    }
    //    else
    //    {
    //        Log.Information("Create Shipment: Failed to create the shipment.");
    //        return Content(String.Format("Failed to create shipment: {0}", model.ShipmentId));
    //    }
    //}

    public class PaymentInformation
    {
        public ShipmentModel Shipment { get; set; }
        public AddressModel Billing { get; set; }
        public CreditCardModel CreditCard { get; set; }
    }
}