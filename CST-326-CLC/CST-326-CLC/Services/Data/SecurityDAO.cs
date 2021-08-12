﻿using System;
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

namespace CST_326_CLC.Services.Data
{
    public class SecurityDAO
    {
        public bool CheckUsername(string username)
        {
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
                    reader.Close();
                    conn.Close();
                    return true;
                }
            }
            catch(SqlException e)
            {
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
                    reader.Close();
                    conn.Close();
                    return true;
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
            return false;
        }

        public bool RegisterUser(UserModel user)
        {
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

            command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 50)).Value = Hash(user.password);

            command.Parameters.Add(new SqlParameter("@business", SqlDbType.TinyInt)).Value = user.isBusinessAccount;
            command.Parameters.Add(new SqlParameter("@admin", SqlDbType.TinyInt)).Value = user.isAdmin;

            command.Prepare();

            retValue = command.ExecuteNonQuery();

            if (retValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string Hash(string password)
        {
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
            return true;
        }
    }
}