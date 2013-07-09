using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Caching;
using System.Configuration;

namespace ImprintCMS
{

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "DisplayFile", // Route name
                "uploads/{category}/{fileName}", // URL with parameters
                new { controller = "Upload", action = "Display", category = UrlParameter.Optional, fileName = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SqlCacheDependencyAdmin.EnableNotifications(ConfigurationManager.ConnectionStrings["ImprintCMSConnectionString"].ConnectionString);
            SqlCacheDependencyAdmin.EnableTableForNotifications(ConfigurationManager.ConnectionStrings["ImprintCMSConnectionString"].ConnectionString, "UploadedFile");

        }
    }
}