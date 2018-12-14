using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NSoup;
using NSoup.Nodes;
using System.IO;
using TODTool.TODSchedulers;

namespace TODTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string signInURL = "https://dellcms.sdlproducts.com/ISHSTS/account/signin";
        protected void Application_Start()
        {
            log.Info("Starting application");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //start the Scheduler
            TodScheduler.StartAsync();
        }
    }

}
