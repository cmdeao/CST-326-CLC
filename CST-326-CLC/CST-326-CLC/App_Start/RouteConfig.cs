using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Serilog;

namespace CST_326_CLC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Log.Information("Registering Routes...");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("defaults", "{controller}/{action}/{id}", new { id = UrlParameter.Optional });

            Log.Information("Routes registered.");
        }
    }
}
