using CST_326_CLC.Models;
using CST_326_CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST_326_CLC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("LoginTEST");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("LoginTEST");
            }

            SecurityService service = new SecurityService();

            if(service.AuthenticateUser(model))
            {
                return Content("You've logged into the application!");
            }
            else
            {
                return Content("You've failed to login!");
            }
        }
    }
}