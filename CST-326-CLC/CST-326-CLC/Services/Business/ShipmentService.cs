using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;
using CST_326_CLC.Controllers;

namespace CST_326_CLC.Services.Business
{
    public class ShipmentService
    {
        ShipmentDAO service = new ShipmentDAO();
        public ShipmentModel ViewShipment(int shipmentID)
        {
            Log.Information("ShipmentService: Retrieving shipment Information for shipmentID: {0}", shipmentID);
            return service.RetrieveShipment(shipmentID);
        }

        public bool AddShipment(ShipmentModel model)
        {
            Log.Information("ShipmentService: Creating a new shipment");
            return service.CreateShipment(model);
        }

        public bool RemoveShipment(int shipmentID)
        {
            Log.Information("ShipmentService: Deleting shipment for shipmentID: {0}", shipmentID);
            return service.DeleteShipment(shipmentID);
        }

        public List<ShipmentInformation> RetrieveAllShipments()
        {
            return service.ViewAllShipments();
        }

        public bool TestNewShipment(ShipmentInformation model)
        {
            return service.NewShipmentTest(model);
        }

        public ShipmentInformation RetrieveNewShipment(int shipmentID)
        {
            return service.RetrieveShipmentInformation(shipmentID);
        }
    }
}