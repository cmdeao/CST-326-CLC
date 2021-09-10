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
using Serilog;
using CST_326_CLC.Controllers;

namespace CST_326_CLC.Services.Data
{
    public class ShipmentDAO
    {
        public ShipmentModel RetrieveShipment(int shipmentID)
        {
            Log.Information("ShipmentDAO: Retrieving shipment from database with shipmentID: {0}", shipmentID);
            string query = "SELECT * FROM dbo.Shipment WHERE Shipment_ID = @Shipment_ID";

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@Shipment_ID", SqlDbType.Int).Value = shipmentID;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ShipmentModel retrievedModel = new ShipmentModel();
                        retrievedModel.ShipmentId = reader.GetInt32(0);
                        retrievedModel.Status = reader.GetString(4);
                        retrievedModel.PackageSize = reader.GetString(5);
                        retrievedModel.Weight = reader.GetInt32(6);
                        retrievedModel.Height = reader.GetInt32(7);
                        retrievedModel.Width = reader.GetInt32(8);
                        retrievedModel.Length = reader.GetInt32(9);
                        //retrievedModel.Zip = reader.GetInt32(9);
                        int packageType = (int)reader.GetSqlByte(10);
                        retrievedModel.IsPackageStandard = Convert.ToBoolean(packageType);
                        retrievedModel.DeliveryOption = reader.GetString(11);

                        int residential = (int)reader.GetSqlByte(12);
                        retrievedModel.IsResidential = Convert.ToBoolean(residential);

                        Log.Information("ShipmentDAO: ShipmentID: {0} retrieved successfully.", shipmentID);
                        return retrievedModel;
                    }
                }
            }
            catch (SqlException e)
            {
                Log.Information("ShipmentDAO: There was an SQL exception when retrieving shipmentID: {0}", shipmentID);
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
            Log.Information("ShipmentDAO: Creating new shipment in the database");

            int operationSuccess = 0;
            string query = "INSERT INTO dbo.Shipment(User_ID, Address_ID, Status, PackageSize, Weight, " +
                "Height, Width, Length, Zip_Code, Packaging, Delivery_Options, Is_Residential) VALUES (@userID, @addressID, @status, " +
                "@packageSize, @weight, @height, @width, @length, @zip, @packaging, @delivery, @residential)";

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                if (UserManagement.Instance._loggedUser == null)
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
                command.Parameters.Add("@packaging", SqlDbType.TinyInt).Value = model.IsPackageStandard;
                command.Parameters.Add("@delivery", SqlDbType.NVarChar, 100).Value = model.DeliveryOption;
                command.Parameters.Add("@residential", SqlDbType.TinyInt).Value = model.IsResidential;

                command.Prepare();

                operationSuccess = command.ExecuteNonQuery();
                return Convert.ToBoolean(operationSuccess);
            }
            catch (SqlException e)
            {
                Log.Information("ShipmentDAO: There was an SQL exception when creating a new shipment in the database.");
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
            Log.Information("ShipmentDAO: Deleting shipment: {0} in the database", shipmentID);

            string query = "DELETE FROM dbo.Shipment WHERE Shipment_ID = @ShipmentID";
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                command.Parameters.Add("@ShipmentID", SqlDbType.Int).Value = shipmentID;
                command.ExecuteNonQuery();

                Log.Information("ShipmentDAO: Successfully Deleted shipment: {0} from the database", shipmentID);

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

        public bool NewShipmentTest(ShipmentInformation model)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            SqlTransaction transaction;
            transaction = conn.BeginTransaction("Shipment");

            command.Connection = conn;
            command.Transaction = transaction;

            try
            {
                int sentAddressID = 0;
                int recipientAddressID = 0;

                command.CommandText = "SELECT ADDRESS_ID FROM dbo.Address WHERE ADDRESS = @Address";
                command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 100)).Value = model.sender.address;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sentAddressID = reader.GetInt32(0);
                    }
                }
                else
                {
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO dbo.Address(USER_ID, ADDRESS, APT_SUITE, CITY, STATE, ZIP, COUNTRY) " +
                    "VALUES (@ID, @Address, @Suite, @City, @State, @Zip, @Country); SELECT SCOPE_IDENTITY();";

                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = 8;
                    command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 100)).Value = model.sender.address;

                    if (model.sender.aptSuite != null)
                    {
                        command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = model.sender.aptSuite;
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = DBNull.Value;
                    }
                    command.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 50)).Value = model.sender.city;
                    command.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 25)).Value = model.sender.state;
                    command.Parameters.Add(new SqlParameter("@Zip", SqlDbType.Int)).Value = model.sender.zip;
                    command.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 50)).Value = model.sender.country;
                    sentAddressID = Convert.ToInt32(command.ExecuteScalar());
                }
                reader.Close();

                command.Parameters.Clear();
                command.CommandText = "SELECT ADDRESS_ID FROM dbo.Address WHERE ADDRESS = @RecipientAdress";
                command.Parameters.Add(new SqlParameter("@RecipientAdress", SqlDbType.NVarChar, 100)).Value = model.recipient.address;
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        recipientAddressID = reader.GetInt32(0);
                    }
                }
                else
                {
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO dbo.Address(USER_ID, ADDRESS, APT_SUITE, CITY, STATE, ZIP, COUNTRY) " +
                    "VALUES (@ID, @Address, @Suite, @City, @State, @Zip, @Country); SELECT SCOPE_IDENTITY();";

                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = 1002;
                    command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 100)).Value = model.recipient.address;

                    if (model.sender.aptSuite != null)
                    {
                        command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = model.recipient.aptSuite;
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = DBNull.Value;
                    }
                    command.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 50)).Value = model.recipient.city;
                    command.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 25)).Value = model.recipient.state;
                    command.Parameters.Add(new SqlParameter("@Zip", SqlDbType.Int)).Value = model.recipient.zip;
                    command.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 50)).Value = model.recipient.country;
                    recipientAddressID = Convert.ToInt32(command.ExecuteScalar());
                }
                reader.Close();

                command.CommandText = "INSERT INTO dbo.Shipment(User_ID, Address_ID, Recipient_Address_ID, Status, PackageSize, Weight, " +
                "Height, Width, Length, Packaging, Delivery_Options, Is_Residential) VALUES (@userID, @addressID, @recipientID, @status, " +
                "@packageSize, @weight, @height, @width, @length, @packaging, @delivery, @residential)";

                command.Parameters.Add("@userID", SqlDbType.Int).Value = 3011;
                command.Parameters.Add("@addressID", SqlDbType.Int).Value = sentAddressID;
                command.Parameters.Add("@recipientID", SqlDbType.Int).Value = recipientAddressID;
                command.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = model.shipment.Status;
                command.Parameters.Add("@packageSize", SqlDbType.VarChar, 50).Value = model.shipment.PackageSize;
                command.Parameters.Add("@weight", SqlDbType.Int).Value = model.shipment.Weight;
                command.Parameters.Add("@height", SqlDbType.Int).Value = model.shipment.Height;
                command.Parameters.Add("@width", SqlDbType.Int).Value = model.shipment.Width;
                command.Parameters.Add("@length", SqlDbType.Int).Value = model.shipment.Length;
                command.Parameters.Add("@packaging", SqlDbType.TinyInt).Value = model.shipment.IsPackageStandard;
                command.Parameters.Add("@delivery", SqlDbType.NVarChar, 100).Value = model.shipment.DeliveryOption;
                command.Parameters.Add("@residential", SqlDbType.TinyInt).Value = model.shipment.IsResidential;
                command.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.GetType());
                Debug.WriteLine(e.Message);
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (SqlException e2)
                {
                    Debug.WriteLine(e2.GetType());
                    Debug.WriteLine(e2.Message);
                }
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public ShipmentInformation RetrieveShipmentInformation(int shipmentID)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string shipmentQuery = "SELECT * FROM dbo.Shipment WHERE Shipment_ID = @ShipmentID";
            SqlCommand command = new SqlCommand(shipmentQuery, conn);
            ShipmentInformation retrievedShipment = new ShipmentInformation();

            try
            {
                command.Parameters.Add("@ShipmentID", SqlDbType.Int).Value = shipmentID;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ShipmentModel retrievedModel = new ShipmentModel();
                        retrievedModel.ShipmentId = reader.GetInt32(0);
                        retrievedModel.Status = reader.GetString(4);
                        retrievedModel.PackageSize = reader.GetString(5);
                        retrievedModel.Weight = reader.GetInt32(6);
                        retrievedModel.Height = reader.GetInt32(7);
                        retrievedModel.Width = reader.GetInt32(8);
                        retrievedModel.Length = reader.GetInt32(9);
                        //retrievedModel.Zip = reader.GetInt32(9);
                        int packageType = (int)reader.GetSqlByte(10);
                        retrievedModel.IsPackageStandard = Convert.ToBoolean(packageType);
                        retrievedModel.DeliveryOption = reader.GetString(11);

                        int residential = (int)reader.GetSqlByte(12);
                        retrievedModel.IsResidential = Convert.ToBoolean(residential);

                        retrievedShipment.shipment = retrievedModel;

                        AddressService addressService = new AddressService();
                        retrievedShipment.sender = addressService.ViewAddress(reader.GetInt32(2));
                        retrievedShipment.recipient = addressService.ViewAddress(reader.GetInt32(3));

                        Log.Information("ShipmentDAO: ShipmentID: {0} retrieved successfully.", shipmentID);
                        return retrievedShipment;
                    }
                }
            }
            catch (SqlException e)
            {
                Log.Information("ShipmentDAO: There was an SQL exception when retrieving shipmentID: {0}", shipmentID);
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }


            return null;
        }

    }
}