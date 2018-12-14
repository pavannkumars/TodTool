using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TODTool
{
    public class RouteConfig
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void RegisterRoutes(RouteCollection routes)
        {
            log.Info("Registering routes and Url Paths");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "Index" }
            );*/

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
