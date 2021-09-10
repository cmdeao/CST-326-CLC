using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace CST_326_CLC.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Log.Information("Navigating to Index View");

            return View("IndexNEW");
        }
        
        public ActionResult About()
        {
            Log.Information("Navigating to About View");

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Log.Information("Navigating to Contact View");

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}