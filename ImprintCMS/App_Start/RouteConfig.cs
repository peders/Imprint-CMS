using System.Web.Mvc;
using System.Web.Routing;

namespace ImprintCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DisplayFile",
                url: "uploads/{category}/{fileName}",
                defaults: new { controller = "Upload", action = "Display", category = UrlParameter.Optional, fileName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DisplayPersonThumbnail",
                url: "thumbnails/{side}/{fileName}",
                defaults: new { controller = "Upload", action = "Thumbnail" },
                constraints: new { side = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}