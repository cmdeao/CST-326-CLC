using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace CST_326_CLC.Controllers
{
    public class ShipmentController : Controller
    {
        // GET: Shipment
        public ActionResult Index()
        {
            Log.Information("Navigating to View Shipments...");
            return View("SearchShipments");
        }

        [HttpPost]
        public ActionResult ViewShipment(int shipmentID)
        {
            Log.Information("Shipment: User is attempting to view shipment by shipmentID: {0}", shipmentID);
            ShipmentService service = new ShipmentService();
            ShipmentModel retrievedShipment = service.ViewShipment(shipmentID);
            return View("ViewShipmentTEST", retrievedShipment);
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
}