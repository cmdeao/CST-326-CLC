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
            Log.Information("AddressDAO: Inserting new Address into the database");
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string insertQuery = "INSERT INTO dbo.Address(USER_ID, ADDRESS, APT_SUITE, CITY, STATE, ZIP, COUNTRY) " +
                    "VALUES (@ID, @Address, @Suite, @City, @State, @Zip, @Country)";
            SqlCommand command = new SqlCommand(insertQuery, conn);
            try
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = 8;
                command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 100)).Value = model.address;

                if (model.aptSuite != null)
                {
                    command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = model.aptSuite;
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@Suite", SqlDbType.NVarChar, 25)).Value = DBNull.Value;
                }
                command.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 50)).Value = model.city;
                command.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 25)).Value = model.state;
                command.Parameters.Add(new SqlParameter("@Zip", SqlDbType.Int)).Value = model.zip;
                command.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 50)).Value = model.country;

                int result = command.ExecuteNonQuery();

                return Convert.ToBoolean(result);
            }
            catch (SqlException e)
            {
                Log.Information("AddressDAO: There was an SQL exception when inserting an address in the database.");
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
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

                Log.Information("AddressDAO: Successfully deleted Address: {0} from the database.", addressID);

                return true;
            }
            catch (SqlException e)
            {
                Log.Information("AddressDAO: There was an SQL exception when deleting an address in the database.");
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
            Log.Information("AddressDAO: Viewing Address: {0} in the database", addressID);
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string addressQuery = "SELECT * FROM dbo.Address WHERE ADDRESS_ID = @ID";
            SqlCommand command = new SqlCommand(addressQuery, conn);

            try
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = addressID;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        AddressModel model = new AddressModel();
                        model.addressID = reader.GetInt32(0);
                        model.address = reader.GetString(2);
                        if(!reader.IsDBNull(3))
                        {
                            model.aptSuite = reader.GetString(3);
                        }
                        model.city = reader.GetString(4);
                        model.state = reader.GetString(5);
                        model.zip = reader.GetInt32(6);
                        model.country = reader.GetString(7);

                        return model;
                    }
                }
            }
            catch (SqlException e)
            {
                Log.Information("AddressDAO: There was an SQL exception when retrieving Address ID: {0}", addressID);
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public bool UpdateAddress(AddressModel newModel, int oldAddress)
        {
            Log.Information("AddressDAO: Updating Address: {0} in the database", oldAddress);
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string updateQuery = "UPDATE dbo.Address SET ADDRESS = @address, APT_SUITE = @apt, CITY = @city, STATE = @state, ZIP = @zip, COUNTRY = @country" +
                " WHERE ADDRESS_ID = @ID";
            SqlCommand command = new SqlCommand(updateQuery, conn);

            try
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = oldAddress;
                command.Parameters.Add(new SqlParameter("@address", SqlDbType.NVarChar, 100)).Value = newModel.address;

                if (newModel.aptSuite != null)
                {
                    command.Parameters.Add(new SqlParameter("@apt", SqlDbType.NVarChar, 25)).Value = newModel.aptSuite;
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@apt", SqlDbType.NVarChar, 25)).Value = DBNull.Value;
                }
                command.Parameters.Add(new SqlParameter("@city", SqlDbType.NVarChar, 50)).Value = newModel.city;
                command.Parameters.Add(new SqlParameter("@state", SqlDbType.NVarChar, 25)).Value = newModel.state;
                command.Parameters.Add(new SqlParameter("@zip", SqlDbType.Int)).Value = newModel.zip;
                command.Parameters.Add(new SqlParameter("@country", SqlDbType.NVarChar, 50)).Value = newModel.country;

                int result = command.ExecuteNonQuery();

                return Convert.ToBoolean(result);
            }
            catch (SqlException e)
            {
                Log.Information("AddressDAO: There was an SQL exception when updating Address ID: {0}", oldAddress);
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public List<AddressModel> ViewAllAddresses()
        {
            Log.Information("AddressDAO: Retrieving all Addresses in the database");
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string query = "SELECT * FROM dbo.Address";
            SqlCommand command = new SqlCommand(query, conn);
            List<AddressModel> retrievedAddresses = new List<AddressModel>();

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        AddressModel address = new AddressModel();
                        address.addressID = reader.GetInt32(0);
                        address.userID = reader.GetInt32(1);
                        address.address = reader.GetString(2);
                        if(!reader.IsDBNull(3))
                        {
                            address.aptSuite = reader.GetString(3);
                        }
                        address.city = reader.GetString(4);
                        address.state = reader.GetString(5);
                        address.zip = reader.GetInt32(6);
                        address.country = reader.GetString(7);

                        retrievedAddresses.Add(address);
                    }
                }

                return retrievedAddresses;
            }
            catch (SqlException e)
            {
                Log.Information("AddressDAO: There was an SQL exception when retrieving addresses");
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