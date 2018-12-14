using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TODTool.Controllers
{
    public class UploadFilesController : Controller
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: UploadFiles
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files)
        {

            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        // var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        //Save file to server folder  
                        // file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " with " + InputFileName + " files uploaded successfully.";
                        log.Info(InputFileName + " uploaded successfully.");
                    }

                }
            }
            return PartialView("~/Views/Shared/UpLoadFiles.cshtml");
        }
    }
}