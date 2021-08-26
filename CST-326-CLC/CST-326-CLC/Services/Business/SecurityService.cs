using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace CST_326_CLC.Services.Business
{
    public class SecurityService
    {
        SecurityDAO service = new SecurityDAO();

        //Service method to check if the passed username is already
        //registered within the application's persistent database.
        public bool CheckUser(string username)
        {
            Log.Information("SecurityService: Checking user {0}", username);
            return service.CheckUsername(username);
        }

        public bool CheckEmail(string email)
        {
            Log.Information("SecurityService: Checking email {0}", email);
            return service.CheckEmail(email);
        }

        public bool RegisterUser(PersonalUserModel model)
        {
            Log.Information("SecurityService: Registering user: {0}", model.username);
            return service.RegisterUser(model);
        }

        public bool RegisterBusiness(BusinessModel model, string securityQuestion, string securityAnswer)
        {
            return service.RegisterBusiness(model, securityQuestion, securityAnswer);
        }

        public bool AuthenticateUser(LoginModel model)
        {
            Log.Information("SecurityService: Authenticating user {0}", model.username);
            return service.AuthenticateUser(model);
        }
    }

    //Singleton to manage a logged in user.
    public class UserManagement
    {
        //Static variable for access
        private static UserManagement _instance;

        //_loggedUser is where a logged in user's information will be stored for access
        //from within the application.
        public UserModel _loggedUser { get; set; } = null;
        public PersonalUserModel _registrationUser { get; set; } = null;
        public BusinessModel _businessUser { get; set; } = null;


        public UserManagement()
        {

        }

        public static UserManagement Instance
        { 
            get
            {
                //Checking if instance is null.
                if(_instance == null)
                {
                    //Initializing the instance variable.
                    _instance = new UserManagement();
                }

                //Returning the variable.
                return _instance;
            }
        }

        //Method to log the user out of the application.
        public void LogOut()
        {
            Log.Information("UserManagement: Logged out user.");
            //Setting the Singleton instance variable to null.
            _loggedUser = null;
        }

        public void ClearRegistration()
        {
            _registrationUser = null;
        }
    }
}