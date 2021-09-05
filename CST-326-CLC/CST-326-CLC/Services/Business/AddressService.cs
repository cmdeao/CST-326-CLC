using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using Serilog;

namespace CST_326_CLC.Services.Business
{
    public class AddressService
    {
        AddressDAO service = new AddressDAO();

        public bool InsertAddress(AddressModel model)
        {
            Log.Information("AddressService: Inserting Address information: {0}", model.address);
            return service.InsertAddress(model);
        }

        public bool RemoveAddress(int addressID)
        {
            Log.Information("AddressService: Removing Address information: {0}", addressID);
            return service.RemoveAddress(addressID);
        }

        public AddressModel ViewAddress(int addressID)
        {
            Log.Information("AddressService: Retrieving Address information: {0}", addressID);
            return service.ViewAddress(addressID);
        }

        public bool UpdateAddress(AddressModel newModel, int oldAddress)
        {
            Log.Information("AddressService: Updating Address information: {0}", newModel.address);
            return service.UpdateAddress(newModel, oldAddress);
        }

        public List<AddressModel> ViewAllAddresses()
        {
            Log.Information("AddressService: Retrieving all Addresses.");
            return service.ViewAllAddresses();
        }
    }
}