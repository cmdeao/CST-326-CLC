using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;
using System.Globalization;
using Newtonsoft.Json;

namespace CST_326_CLC.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            Log.Information("Navigating to Registration Page.");
            return View();
        }

        public ActionResult Personal()
        {
            Log.Information("Navigating to Personal Registration Page.");
            return View("Personal");
        }

        public ActionResult Business()
        {
            Log.Information("Navigating to Business Registration Page.");
            return View("Business");
        }

        [HttpPost]
        public ActionResult BusinessStep3(BusinessModel model, string companyState)
        {
            Log.Information("Beginning Step 3 of Business Registration.");
            model.state = companyState;
            model.isBusinessAccount = true;
            model.isAdmin = false;

            SecurityService service = new SecurityService();

            if(service.CheckEmail(model.companyEmail))
            {
                ModelState.AddModelError("companyEmail", "Please enter a valid email.");
            }

            if(!ModelState.IsValid)
            {
                return View("Business");
            }

            UserManagement.Instance._businessUser = model;

            return View("BusinessStep3");
        }

        [HttpPost]
        public ActionResult BusinessRegistration(BusinessLoginModel model, string securityQuestion)
        {
            Log.Information("Attempting to register a new business user.");
            if (!ModelState.IsValid || model.password != model.confirmPass)
            {
                ModelState.AddModelError("password", "Please ensure both password inputs match.");
                return View("BusinessStep3");
            }

            SecurityService service = new SecurityService();

            if(service.CheckUser(model.username))
            {
                ModelState.AddModelError("username", "Please enter a valid username.");
                return View("BusinessStep3");
            }

            model.securityQuestion = securityQuestion;
            BusinessModel regBusiness = UserManagement.Instance._businessUser;
            regBusiness.username = model.username;
            regBusiness.password = model.password;

            if (service.RegisterBusiness(regBusiness, securityQuestion, model.securityAnswer))
            {
                Log.Information("Registration: Business User successfully registered.");
                return Content("SUCCESS!");
            }
            else
            {
                Log.Information("Registration: Business Registration failed.");
                return View("Error");
            }
                   
        }

        [HttpPost]
        public ActionResult Step3(PersonalUserModel model, string userState)
        {
            SecurityService service = new SecurityService();

            if(service.CheckEmail(model.email))
            {
                ModelState.AddModelError("email", "Please enter a valid email.");
                return View("Personal");
            }

            model.state = userState;
            model.isAdmin = false;
            model.isBusinessAccount = false;
            if (!ModelState.IsValid)
            {
                return View("Personal");
            }

            UserManagement.Instance._registrationUser = model;

            return View("Step3");
        }

        [HttpPost]
        public ActionResult Registration(PersonalCredsModel creds)
        {
            Log.Information("Attempting to register a new user...");
            PersonalUserModel regModel = UserManagement.Instance._registrationUser;

            if(!ModelState.IsValid || creds.password != creds.confirmPass)
            {
                ModelState.AddModelError("password", "Please ensure both password inputs match.");
                return View("Step3");
            }

            regModel.username = creds.username;
            regModel.password = creds.password;

            SecurityService service = new SecurityService();
            
            if(service.CheckUser(regModel.username))
            {
                ModelState.AddModelError("username", "Please enter a valid username.");
                return View("Step3");
            }

            if (service.RegisterUser(regModel))
            {
                Log.Information("Registration: User successfully registered.");
                return Content("You've created an account!");
            }
            else
            {
                Log.Information("Registration: Registration failed.");
                return View("Error");
            }
        }
    }
}