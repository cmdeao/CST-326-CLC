using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;
using System.Diagnostics;

namespace CST_326_CLC.Controllers
{
    public class ShipmentController : Controller
    {
        // GET: Shipment
        public ActionResult Index()
        {
            Log.Information("Navigating to View Shipments...");

            //THESE LINES ARE FOR TESTING PURPOSES ONLY! THEY WILL BE RELOCATED SOON.
            //ShipmentService service = new ShipmentService();
            //List<ShipmentInformation> shipments = new List<ShipmentInformation>();
            //shipments = service.RetrieveAllShipments();
            //return View("ViewAllShipments", shipments);

            //CreditService service = new CreditService();
            //CreditCardModel model = new CreditCardModel();

            //model.cardHolderName = "Cameron Deao";
            //model.cardType = CreditCardModel.CardType.Visa;
            //model.cardNumber = 1234123412341234;
            //model.cvvCode = 123;
            //model.expirationMonth = 12;
            //model.expirationYear = 12;

            //if(service.StoreCreditCard(model, 8))
            //{
            //    Debug.WriteLine("SUCCESS!");
            //}
            //else
            //{
            //    Debug.WriteLine("FAILED!");
            //}

            //PaymentService service = new PaymentService();
            //Dictionary<int, decimal> transactions = new Dictionary<int, decimal>();
            //transactions = service.RetrieveTransactions();

            //foreach(KeyValuePair<int, decimal> ele in transactions)
            //{
            //    Debug.WriteLine("{0} and {1}", ele.Key, ele.Value);
            //}

            //if(service.CreateTransaction(7, 12.34M))
            //{
            //    Debug.WriteLine("SUCCESS!");
            //}
            //else
            //{
            //    Debug.WriteLine("FAILED");
            //}

            return View("SearchShipments");
        }

        public ActionResult ViewAllShipments()
        {
            return View("ViewAllShipments");
        }

        [HttpPost]
        public ActionResult ViewShipment(int shipmentID)
        {
            Log.Information("Shipment: User is attempting to view shipment by shipmentID: {0}", shipmentID);
            ShipmentService service = new ShipmentService();
            ShipmentInformation retrievedShipment = service.RetrieveNewShipment(shipmentID);

            return View("ViewShipmentV2", retrievedShipment);
        }

        [HttpPost]
        public ActionResult CreateShipment(ShipmentModel model)
        {
            Log.Information("User attempting to create shipment...");

            // Can be used to display cost to user
            decimal shipmentCost = model.CalculateCost(model.Zip, model.Length, model.Width,
                model.Height, model.Weight, model.DeliveryOption);

            if(!ModelState.IsValid)
            {
                Log.Information("Create Shipment: The ModelState was invalid.");
                return View("ShipmentIndexTEST");
            }

            ShipmentService service = new ShipmentService();

            if(service.AddShipment(model))
            {
                Log.Information("Create Shipment: Shipment succesfully created ID: {0}", model.ShipmentId);
                return Content(String.Format("Created shipment: {0}", model.ShipmentId));
            }
            else
            {
                Log.Information("Create Shipment: Failed to create the shipment.");
                return Content(String.Format("Failed to create shipment: {0}", model.ShipmentId));
            }
        }

        [HttpPost]
        public ActionResult ShipmentTEST(ShipmentInformation model, string senderState, string recipientState)
        {
            model.shipment.Status = "Shipped";
            model.shipment.PackageSize = "Medium";
            model.shipment.IsPackageStandard = true;
            model.shipment.DeliveryOption = "Standard";
            model.shipment.IsResidential = true;
            model.sender.state = senderState;
            model.recipient.state = recipientState;

            ShipmentService service = new ShipmentService();
            if (service.TestNewShipment(model))
            {
                return Content("SUCCESS");
            }
            else
            {
                return Content("FAILED");
            }
        }

        [HttpPost]
        public ActionResult RemoveShipment(int shipmentID)
        {
            Log.Information("User is attempting to remove a shipment...");
            ShipmentService service = new ShipmentService();
            
            if(service.RemoveShipment(shipmentID))
            {
                Log.Information("Remove Shipment: Shipment: {0} was successfully removed.", shipmentID);
                return Content(String.Format("Removed shipment: {0}", shipmentID));
            }
            else
            {
                Log.Information("Remove Shipment: Failed to remove shipment: {0}", shipmentID);
                return Content(String.Format("Failed to remove shipment: {0}", shipmentID));
            }
        }
    }

    public class ShipmentInformation
    {
        public ShipmentModel shipment { get; set; }
        public AddressModel sender { get; set; }
        public AddressModel recipient { get; set; }
    }
}