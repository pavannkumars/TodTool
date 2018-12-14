using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TODTool.TodBL;

namespace TODTool.Controllers
{
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Thread thrd;

        public ActionResult Index()
        {
            log.Info("Invoking Home Page to display");


            //validate user role in Users Table for the user logged in

            return View();
        }

        public ActionResult About()
        {
            log.Info("Invoking About Page to display");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            log.Info("Invoking Contact Page to display");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}