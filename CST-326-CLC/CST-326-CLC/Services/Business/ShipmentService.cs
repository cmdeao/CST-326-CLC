using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST_326_CLC.Services.Business
{
    public class ShipmentService
    {
        ShipmentDAO service = new ShipmentDAO();
        public ShipmentModel ViewShipment(int shipmentID)
        {
            return service.RetrieveShipment(shipmentID);
        }

        public bool AddShipment(ShipmentModel model)
        {
            return service.CreateShipment(model);
        }

        public bool RemoveShipment(int shipmentID)
        {
            return service.DeleteShipment(shipmentID);
        }
    }
}