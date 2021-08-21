using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CST_326_CLC.Services.Data
{
    public class ShipmentDAO
    {
        public ShipmentModel RetrieveShipment(int shipmentID)
        {
            string query = "SELECT * FROM dbo.Shipment WHERE Shipment_ID = @Shipment_ID";

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@Shipment_ID", SqlDbType.Int).Value = shipmentID;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        ShipmentModel retrievedModel = new ShipmentModel();
                        retrievedModel.ShipmentId = reader.GetInt32(0);
                        retrievedModel.Status = reader.GetString(3);
                        retrievedModel.PackageSize = reader.GetString(4);
                        retrievedModel.Weight = reader.GetInt32(5);
                        retrievedModel.Height = reader.GetInt32(6);
                        retrievedModel.Width = reader.GetInt32(7);
                        retrievedModel.Length = reader.GetInt32(8);
                        retrievedModel.Zip = reader.GetInt32(9);
                        retrievedModel.PackagingType = reader.GetString(10);
                        retrievedModel.DeliveryOption = reader.GetString(11);

                        int residential = (int)reader.GetSqlByte(12);
                        retrievedModel.IsResidential = Convert.ToBoolean(residential);

                        return retrievedModel;
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public bool CreateShipment(ShipmentModel model)
        {
            int operationSuccess = 0;
            string query = "INSERT INTO dbo.Shipment(User_ID, Address_ID, Status, PackageSize, Weight, " +
                "Height, Width, Length, Zip_Code, Packaging, Delivery_Options, Is_Residential) VALUES (@userID, @addressID, @status, " +
                "@packageSize, @weight, @height, @width, @length, @zip, @packaging, @delivery, @residential)";

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                if(UserManagement.Instance._loggedUser == null)
                {
                    command.Parameters.Add("@userID", SqlDbType.Int).Value = 3005;
                }
                else
                {
                    command.Parameters.Add("@userID", SqlDbType.Int).Value = UserManagement.Instance._loggedUser.userID;
                }

                //Currently inserting a test value for Address_ID
                command.Parameters.Add("@addressID", SqlDbType.Int).Value = 987654;

                command.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = model.Status;
                command.Parameters.Add("@packageSize", SqlDbType.VarChar, 50).Value = model.PackageSize;
                command.Parameters.Add("@weight", SqlDbType.Int).Value = model.Weight;
                command.Parameters.Add("@height", SqlDbType.Int).Value = model.Height;
                command.Parameters.Add("@width", SqlDbType.Int).Value = model.Width;
                command.Parameters.Add("@length", SqlDbType.Int).Value = model.Length;
                command.Parameters.Add("@zip", SqlDbType.Int).Value = model.Zip;
                command.Parameters.Add("@packaging", SqlDbType.NVarChar, 50).Value = model.PackagingType;
                command.Parameters.Add("@delivery", SqlDbType.NVarChar, 100).Value = model.DeliveryOption;
                command.Parameters.Add("@residential", SqlDbType.TinyInt).Value = model.IsResidential;

                command.Prepare();

                operationSuccess = command.ExecuteNonQuery();
                return Convert.ToBoolean(operationSuccess);
            }
            catch (SqlException e)
            {
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool DeleteShipment(int shipmentID)
        {
            string query = "DELETE FROM dbo.Shipment WHERE Shipment_ID = @ShipmentID";
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                command.Parameters.Add("@ShipmentID", SqlDbType.Int).Value = shipmentID;
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
    }
}