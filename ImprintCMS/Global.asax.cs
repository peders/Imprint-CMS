using System.Configuration;
using System.Web.Caching;
using System.Web.Http;
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
    }
}