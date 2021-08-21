using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace CST_326_CLC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Log.Information("Navigating to Login page.");

            return View("LoginTEST");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            Log.Information("Login attempted...");

            if(!ModelState.IsValid)
            {
                Log.Information("Login: Login Failed. ModelState was invalid.");

                return View("LoginTEST");
            }

            SecurityService service = new SecurityService();

            if(service.AuthenticateUser(model))
            {
                Log.Information("Login: Login Succeeded. User data was correct.");
                return Content("You've logged into the application!");
            }
            else
            {
                Log.Information("Login: Login Failed. User data was incorect.");
                return Content("You've failed to login!");
            }
        }
    }
}