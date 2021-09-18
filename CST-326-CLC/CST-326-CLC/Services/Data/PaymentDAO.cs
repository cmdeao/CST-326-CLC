using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Diagnostics;

namespace CST_326_CLC.Services.Data
{
    public class PaymentDAO
    {
        public bool CreateTransaction(int shipmentID, decimal amount)
        {
            Log.Information("PaymentDAO: Inserting new payment transaction into the database");

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string transactionQuery = "INSERT INTO dbo.Payment (SHIPMENT_ID, AMOUNT) VALUES (@ShipmentID, @Amount)";
            SqlCommand command = new SqlCommand(transactionQuery, conn);

            try
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("ShipmentID", SqlDbType.Int)).Value = shipmentID;
                command.Parameters.Add(new SqlParameter("Amount", SqlDbType.Decimal)).Value = amount;

                int operationResult = command.ExecuteNonQuery();

                return Convert.ToBoolean(operationResult);
            }
            catch (SqlException e)
            {
                Log.Information("PaymentDAO: There was an SQL exception when inserting a transaction in the database.");
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public Dictionary<int, decimal> RetrieveTransaction()
        {
            Log.Information("PaymentDAO: Retrieving transactions from the database");

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string retrieveQuery = "SELECT * FROM dbo.Payment";
            SqlCommand command = new SqlCommand(retrieveQuery, conn);

            Dictionary<int, decimal> transactions = new Dictionary<int, decimal>();

            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        transactions.Add(reader.GetInt32(1), reader.GetDecimal(2));
                    }
                }

                return transactions;
            }
            catch (SqlException e)
            {
                Log.Information("PaymentDAO: There was an SQL exception when retrieving transactions from the database.");
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