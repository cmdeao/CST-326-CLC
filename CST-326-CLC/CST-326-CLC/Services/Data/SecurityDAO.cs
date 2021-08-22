using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using CST_326_CLC.Models;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Diagnostics;
using System.Security.Cryptography;
using CST_326_CLC.Services.Business;
using Serilog;

namespace CST_326_CLC.Services.Data
{
    public class SecurityDAO
    {
        public bool CheckUsername(string username)
        {
            Log.Information("SecurityDAO: Checking username: {0} against the database", username);
            string query = "SELECT * FROM dbo.Users WHERE username = @Username";

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    Log.Information("SecurityDAO: Successfully found username: {0}", username);
                    reader.Close();
                    return true;
                }
            }
            catch(SqlException e)
            {
                Log.Information("SecurityDAO: There was an SQL excecption when checking username: {0}", username);
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool CheckEmail(string email)
        {
            Log.Information("SecurityDAO: Checking user email: {0} against the database", email);

            string query = "SELECT * FROM dbo.Users WHERE email = @Email";
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = email;
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    Log.Information("SecurityDAO: Successfully found user email: {0}", email);
                    reader.Close();
                    return true;
                }
            }
            catch (SqlException e)
            {
                Log.Information("SecurityDAO: There was an SQL excecption when checking for user email: {0}", email);
                Debug.WriteLine(String.Format("Error generated: {0} - {1}", e.GetType(), e.Message));
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool RegisterUser(UserModel user)
        {
            Log.Information("SecurityDAO: Registering new user to database");

            int retValue = 0;
            
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);

            conn.Open();

            string query = "INSERT INTO dbo.Users(first_name, last_name, phone, email, username, password, isBusinessAccount, isAdmin) " +
                "VALUES(@fName, @lName, @phone, @email, @username, @password, @business, @admin)";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.Add(new SqlParameter("@fName", SqlDbType.VarChar, 25)).Value = user.firstName;
            command.Parameters.Add(new SqlParameter("@lName", SqlDbType.VarChar, 25)).Value = user.lastName;
            command.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar, 15)).Value = user.phone;
            command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 50)).Value = user.email;
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 50)).Value = user.username;
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 100)).Value = Hash(user.password);
            command.Parameters.Add(new SqlParameter("@business", SqlDbType.TinyInt)).Value = user.isBusinessAccount;
            command.Parameters.Add(new SqlParameter("@admin", SqlDbType.TinyInt)).Value = user.isAdmin;

            command.Prepare();

            retValue = command.ExecuteNonQuery();

            return Convert.ToBoolean(retValue);
        }

        public static string Hash(string password)
        {
            Log.Information("SecurityDAO: Hashing password");
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[20]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[40];
            Array.Copy(salt, 0, hashBytes, 0, 20);
            Array.Copy(hash, 0, hashBytes, 20, 20);
            string hashPass = Convert.ToBase64String(hashBytes);
            return hashPass;
        }

        public bool VerifyHash(string hashPass, string password)
        {
            Log.Information("SecurityDAO: Verifing hashed password");
            byte[] hashBytes = Convert.FromBase64String(hashPass);
            byte[] salt = new byte[20];
            Array.Copy(hashBytes, 0, salt, 0, 20);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 20] != hash[i])
                {
                    return false;
                    throw new UnauthorizedAccessException();
                }
            }
            Log.Information("SecurityDAO: Hashed password verified.");
            return true;
        }
    
        public bool AuthenticateUser(LoginModel user)
        {
            Log.Information("SecurityDAO: Authenticating user against database");

            string query = "SELECT * FROM dbo.Users WHERE username = @Username";
            bool autenticatedUser = false;

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["myConn"].ConnectionString);
            SqlCommand command = new SqlCommand(query, conn);

            try
            {
                command.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = user.username;

                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        if(!VerifyHash(reader.GetString(6), user.password))
                        {
                            break;
                        }
                        else
                        {
                            UserModel loggedUser = new UserModel();
                            loggedUser.userID = reader.GetInt32(0);
                            loggedUser.firstName = reader.GetString(1);
                            loggedUser.lastName = reader.GetString(2);
                            loggedUser.phone = reader.GetString(3);
                            loggedUser.email = reader.GetString(4);
                            loggedUser.username = reader.GetString(5);
                            int business = (int)reader.GetSqlByte(7);
                            int admin = (int)reader.GetSqlByte(8);
                            loggedUser.isBusinessAccount = Convert.ToBoolean(business);
                            loggedUser.isAdmin = Convert.ToBoolean(admin);
                            UserManagement.Instance._loggedUser = loggedUser;

                            autenticatedUser = true;
                        }
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
            return autenticatedUser;
        }
    }
}