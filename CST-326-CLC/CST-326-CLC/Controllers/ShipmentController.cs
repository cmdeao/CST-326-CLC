using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST_326_CLC.Controllers
{
    public class ShipmentController : Controller
    {
        // GET: Shipment
        public ActionResult Index()
        {
            return View("ShipmentIndexTEST");
        }

        [HttpPost]
        public ActionResult ViewShipment(int shipmentID)
        {
            ShipmentService service = new ShipmentService();
            ShipmentModel retrievedShipment = service.ViewShipment(shipmentID);
            return View("ViewShipmentTEST", retrievedShipment);
        }

        [HttpPost]
        public ActionResult CreateShipment(ShipmentModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("ShipmentIndexTEST");
            }

            ShipmentService service = new ShipmentService();

            if(service.AddShipment(model))
            {
                return Content(String.Format("Created shipment: {0}", model.ShipmentId));
            }
            else
            {
                return Content(String.Format("Failed to create shipment: {0}", model.ShipmentId));
            }
        }

        [HttpPost]
        public ActionResult RemoveShipment(int shipmentID)
        {
            ShipmentService service = new ShipmentService();
            
            if(service.RemoveShipment(shipmentID))
            {
                return Content(String.Format("Removed shipment: {0}", shipmentID));
            }
            else
            {
                return Content(String.Format("Failed to remove shipment: {0}", shipmentID));
            }
        }
    }
}