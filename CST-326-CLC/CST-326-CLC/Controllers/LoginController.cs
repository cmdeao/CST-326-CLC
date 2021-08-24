using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;
using System.Diagnostics;

namespace CST_326_CLC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Log.Information("Navigating to Login page.");

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if(String.IsNullOrWhiteSpace(username) )
            {
                return RedirectToAction("Index", "Registration");
            }
            if(String.IsNullOrWhiteSpace(password))
            {
                return RedirectToAction("Index", "Registration");
            }

            SecurityService service = new SecurityService();

            LoginModel login = new LoginModel();
            login.username = username;
            login.password = password;
            if (service.AuthenticateUser(login))
            {
                return Content("You've logged in!");
            }
            else
            {
                return Content("Failed to login!");
            }
        }
    }
}