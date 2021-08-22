using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace CST_326_CLC.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            Log.Information("Navigating to Registration Page.");

            return View("TESTRegistration");
        }

        [HttpPost]
        public ActionResult RegisterAccount(UserModel model)
        {
            Log.Information("Attempting to register a new user...");

            SecurityService service = new SecurityService();
            
            if (service.CheckEmail(model.email))
            {
                Log.Information("Registration: User entered an invalid email.");
                ModelState.AddModelError("email", "Please enter a valid email.");
            }

            if(service.CheckUser(model.username))
            {
                Log.Information("Registration: User entered an invalid username.");
                ModelState.AddModelError("username", "Please enter a valid username.");
            }

            if (!ModelState.IsValid)
            {
                Log.Information("Registration: User entered an invalid registration details.");
                return View("TESTRegistration");
            }

            model.isBusinessAccount = false;
            model.isAdmin = false;

            if(service.RegisterUser(model))
            {
                Log.Information("Registration: User successfully registered.");
                return Content("You've created your account!");
            }
            else
            {
                Log.Information("Registration: Registration failed.");
                return Content("Something went wrong! Try again later!");
            }
        }
    }
}