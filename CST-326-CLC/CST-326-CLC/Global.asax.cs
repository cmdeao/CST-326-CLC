using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CST_326_CLC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var LogPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();

            Log.Information("Starting application...");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
