using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TODTool.Helpers;
using TODTool.modelenums;

namespace TODTool.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            TODModelEnumerators menums = new TODModelEnumerators();
            menums.todUserModel = new FormModels.TodUserModel();
            //menums.userRoles = new UserRoles();
            return View(menums);
        }

        
    }
}