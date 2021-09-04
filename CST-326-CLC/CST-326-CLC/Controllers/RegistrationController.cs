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
            return View();
        }

        public ActionResult Business()
        {
            Log.Information("Navigating to Business Registration Page.");
            return View();
        }
    
        [HttpPost]
        public ActionResult Step3(PersonalRegistration model, string userState)
        {
            SecurityService service = new SecurityService();

            if(service.CheckEmail(model.userModel.email))
            {
                ModelState.AddModelError("userModel.email", "Please enter a valid email.");
                return View("Personal");
            }
            model.addressModel.state = userState;
            model.userModel.isAdmin = false;
            model.userModel.isBusinessAccount = false;
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                string modelStateJSON = JsonConvert.SerializeObject(errors, Formatting.Indented);
                Log.Information("A Model State error occured: {0}", modelStateJSON);
                return View("Error");
            }   

            UserManagement.Instance._personalRegistration = model;
            return View("Step3");
        }

        [HttpPost]
        public ActionResult Registration(PersonalCredsModel model)
        {
            Log.Information("Attempting to register a new user...");
            PersonalRegistration user = UserManagement.Instance._personalRegistration;

            if(!ModelState.IsValid || model.password != model.confirmPass)
            {
                return View("Step3");
            }

            user.userModel.username = model.username;
            user.userModel.password = model.password;
            SecurityService service = new SecurityService();
            if (service.CheckUser(model.username))
            {
                ModelState.AddModelError("username", "Please enter a valid username.");
                return View("Step3");
            }

            if (service.RegisterPersonal(user))
            {
                Log.Information("Registration: User successfully registered.");
                UserManagement.Instance.ClearRegistration();
                return Content("You've created an account!");
            }
            else
            {
                Log.Information("Registration: Registration failed.");
                UserManagement.Instance.ClearRegistration();
                return View("Error");
            }
        }
    
        [HttpPost]
        public ActionResult BusinessStep3(BusinessRegistration model, string companyState)
        {
            SecurityService service = new SecurityService();

            if(service.CheckEmail(model.businessModel.companyEmail))
            {
                ModelState.AddModelError("businessModel.email", "Please enter a valid email.");
                return View("Business");
            }

            model.addressModel.state = companyState;
            model.businessModel.isAdmin = false;
            model.businessModel.isBusinessAccount = true;

            if(!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                string businessModelJSON = JsonConvert.SerializeObject(errors, Formatting.Indented);
                Log.Information("A Model State error occurred: {0}", businessModelJSON);
                return View("Error");
            }
            UserManagement.Instance._businessRegistration = model;
            return View("BusinessStep3");
        }

        [HttpPost]
        public ActionResult BusinessRegistration(BusinessLoginModel model, string securityQuestion)
        {
            if(!ModelState.IsValid || model.password != model.confirmPass)
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
            BusinessRegistration businessRegistration = UserManagement.Instance._businessRegistration;

            if(service.RegisterBusiness(model, businessRegistration))
            {
                Log.Information("Registration: Business User successfully registered.");
                UserManagement.Instance.ClearRegistration();
                return Content("SUCCESS!");
            }
            else
            {
                Log.Information("Registration: Business Registration failed.");
                UserManagement.Instance.ClearRegistration();
                return View("Error");
            }
        }
    }

    public class PersonalRegistration
    {
        public UserModel userModel { get; set; }
        public AddressModel addressModel { get; set; }
    }

    public class BusinessRegistration
    {
        public BusinessModel businessModel { get; set; }
        public AddressModel addressModel { get; set; }
    }
}