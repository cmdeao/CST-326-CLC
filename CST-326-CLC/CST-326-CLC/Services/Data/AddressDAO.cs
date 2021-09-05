using CST_326_CLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Configuration;

namespace CST_326_CLC.Services.Data
{
    public class AddressDAO
    {
        public bool InsertAddress(AddressModel model)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            conn.Open();
            return false;
        }

        public bool RemoveAddress(int addressID)
        {
            Log.Information("AddressDAO: Deleting Address: {0} in the database", addressID);

            string query = "DELETE FROM dbo.Addresses WHERE address_id = @ID";
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                command.Parameters.Add("@ID", SqlDbType.Int).Value = addressID;
                command.ExecuteNonQuery();

                Log.Information("ShipmentDAO: Successfully deleted Address: {0} from the database.", addressID);

                return true;
            }
            catch (SqlException e)
            {
                Log.Information("ShipmentDAO: There was an SQL exception when deleting a new shipment in the database.");
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public AddressModel ViewAddress(int addressID)
        {
            return null;
        }

        public bool UpdateAddress(AddressModel newModel, int oldAddress)
        {
            return false;
        }

        public List<AddressModel> ViewAllAddresses()
        {
            return null;
        }
    }
}