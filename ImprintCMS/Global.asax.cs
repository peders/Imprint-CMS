using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImprintCMS
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SqlCacheDependencyAdmin.EnableNotifications(ConfigurationManager.ConnectionStrings["ImprintCMSConnectionString"].ConnectionString);
            SqlCacheDependencyAdmin.EnableTableForNotifications(ConfigurationManager.ConnectionStrings["ImprintCMSConnectionString"].ConnectionString, "UploadedFile");

        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nb-NO");

        }

    }
}