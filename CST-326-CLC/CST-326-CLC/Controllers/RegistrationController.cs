using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST_326_CLC.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAccount(UserModel model)
        {
            SecurityService service = new SecurityService();
            
            if (service.CheckEmail(model.email))
            {
                ModelState.AddModelError("email", "Please enter a valid email.");
            }

            if(service.CheckUser(model.username))
            {
                ModelState.AddModelError("username", "Please enter a valid username.");
            }

            if (!ModelState.IsValid)
            {
                return View("TESTRegistration");
            }

            model.isBusinessAccount = false;
            model.isAdmin = false;

            if(service.RegisterUser(model))
            {
                return Content("You've created your account!");
            }
            else
            {
                return Content("Something went wrong! Try again later!");
            }
        }
    }
}