using CST_326_CLC.Models;
using CST_326_CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST_326_CLC.Services.Business
{
    public class SecurityService
    {
        SecurityDAO service = new SecurityDAO();

        //Service method to check if the passed username is already
        //registered within the application's persistent database.
        public bool CheckUser(string username)
        {
            return service.CheckUsername(username);
        }

        public bool CheckEmail(string email)
        {
            return service.CheckEmail(email);
        }

        public bool RegisterUser(UserModel model)
        {
            return service.RegisterUser(model);
        }

        public bool AuthenticateUser(LoginModel model)
        {
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
            //Setting the Singleton instance variable to null.
            _loggedUser = null;
        }
    }
}