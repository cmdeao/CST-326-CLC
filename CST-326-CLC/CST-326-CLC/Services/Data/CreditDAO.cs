using CST_326_CLC.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;

namespace CST_326_CLC.Services.Data
{
    public class CreditDAO
    {
        /* DISCLOSURE!!!
         * This class does not showcase how to properly store payment information.
         * This class is intended for learning functionality purposes within a classroom environment.
         * The Payment Card Industry Data Security Standard (PCI DSS) has outlined as set of security 
         * standards and protocols to be utilized when storing ACTUAL CREDIT CARD INFORMATION!
         * For information regarding PCI DSS please visit: https://www.pcicomplianceguide.org/faq/
         */

        public bool StoreCredit(CreditCardModel model, int userID)
        {
            Log.Information("CreditDAO: Inserting new CreditModel into the database");

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string insertQuery = "INSERT INTO dbo.Credit(USER_ID, USER_NAME, CARD_NUMBER, EXPIRATION_MONTH, EXPIRATION_YEAR, SECURITY, CARD_TYPE) " +
                "VALUES (@ID, @Username, @Number, @Month, @Year, @Security, @Type)";
            SqlCommand command = new SqlCommand(insertQuery, conn);

            try
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = userID;
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 100)).Value = model.cardHolderName;
                command.Parameters.Add(new SqlParameter("@Number", SqlDbType.BigInt)).Value = model.cardNumber;
                command.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int)).Value = model.expirationMonth;
                command.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = model.expirationYear;
                command.Parameters.Add(new SqlParameter("@Security", SqlDbType.Int)).Value = model.cvvCode;
                command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = model.cardType.ToString();

                int operationResult = command.ExecuteNonQuery();

                return Convert.ToBoolean(operationResult);
            }
            catch (SqlException e)
            {
                Log.Information("CreditDAO: There was an SQL exception when inserting a card in the database.");
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public List<CreditCardModel> RetrieveCards(int userID)
        {
            Log.Information("CreditDAO: Retrieving cards for {0}", userID);

            List<CreditCardModel> cards = new List<CreditCardModel>();
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string query = "SELECT * FROM dbo.Credit WHERE USER_ID = @UserID";
            SqlCommand command = new SqlCommand(query, conn);

            try 
            {
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int)).Value = userID;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        CreditCardModel card = new CreditCardModel();
                        card.cardHolderName = reader.GetString(4);
                        card.cardNumber = reader.GetInt64(5);
                        card.expirationMonth = reader.GetInt32(6);
                        card.expirationYear = reader.GetInt32(7);
                        card.cvvCode = reader.GetInt32(8);
                        card.cardType = (CreditCardModel.CardType)Enum.Parse(typeof(CreditCardModel.CardType), reader.GetString(9));

                        cards.Add(card);
                    }
                }

                return cards;
            }
            catch (SqlException e)
            {
                Log.Information("CreditDAO: There was an SQL exception when retrieving cards from the database.");
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public bool CheckCard(long cardNumber)
        {
            Log.Information("CreditDAO: Checking card for {0}", cardNumber);

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            string query = "SELECT * FROM dbo.Credit WHERE CARD_NUMBER = @Number";
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                int expirationMonth = 0;
                int expirationYear = 0;
                command.Parameters.Add(new SqlParameter("@Number", SqlDbType.BigInt)).Value = cardNumber;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                { 
                    while(reader.Read())
                    {
                        expirationMonth = reader.GetInt32(6);
                        expirationYear = reader.GetInt32(7);
                    }
                }

                DateTime currentDate = DateTime.Today;
                int currentMonth = currentDate.Month;
                int currentYear = currentDate.Year;

                if(expirationYear >= currentYear && expirationMonth > currentMonth)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException e)
            {
                Log.Information("CreditDAO: There was an SQL exception when checking card {0}.", cardNumber);
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