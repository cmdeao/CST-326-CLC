using System.Web;
using System.Web.Mvc;
using Serilog;

namespace CST_326_CLC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            Log.Information("Registering Filters...");

            filters.Add(new HandleErrorAttribute());

            Log.Information("Filters registered.");
        }
    }
}
