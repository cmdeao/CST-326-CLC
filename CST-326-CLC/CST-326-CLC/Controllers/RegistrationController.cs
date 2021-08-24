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
            return View("Personal");
        }

        public ActionResult Business()
        {
            return View("Business");
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
                return Content("You've created an account!");
            }
            else
            {
                return View("Error");
            }
        }
    }
}