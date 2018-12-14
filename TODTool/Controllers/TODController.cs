using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TODTool.Helpers;
using TODTool.TodBL;
using TODTool.TODSchedulers;

namespace TODTool.Controllers
{
    public class TODController : Controller
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TODDataService tODDataService;

        // GET: TOD
        public ActionResult Index()
        {
            //get the scheduler execution status message to display in Scheduler notification area in TOD Page
            ViewData["schMessage"] = TodScheduler.GetSchedulerJobMessage();
            tODDataService = new TODDataService();
            ViewData["fwklist"] = tODDataService.GetCEWKList();
            ViewData["PRDList"] = tODDataService.GetProductList();
            return View();
        }

        [HttpGet]
        public ActionResult GetSchedulerResultInfo()
        {
            bool schStat = TodScheduler.GetSchStatus();
            ViewData["schStat"] = schStat;
            string schMessage = TodScheduler.GetSchedulerJobMessage();
            ViewData["schMessage"] = schMessage;
            return Json(new { success = schStat, oldval = schMessage },
                JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetCEFWList()
        {
            //List<string> fwklist = TODDataService.GetCEWKList();
            /*foreach (var wk in fwklist)
            {
                log.Info("Week is : " + wk);
            }
            ViewData["fwklist"] = fwklist;*/

            return PartialView(tODDataService.GetEnumFWKList());
        }


        [HttpPost]
        public JsonResult UploadMultiple(HttpPostedFileBase[] uploadedFiles, string fweek)
        {
            bool success = true;
            OMNITURE_DATA omData = new OMNITURE_DATA();
            tODDataService = new TODDataService();
            for (var i = 0; i < uploadedFiles.Length; i++)
            {
                var uploadedFile = uploadedFiles[i];
                if (uploadedFile != null && uploadedFile.ContentLength > 0)
                {
                    byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                    uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);

                    //save the file to database 
                    
                    omData = tODDataService.AppendFileData(omData, uploadedFile, fweek);
                    
                }
            }

            success = tODDataService.SaveOrUpdateOMRecords(omData, fweek);
           
            if (success)
            {
                log.Info("Files successfully uploaded to database.");
                ViewData["UploadStatus"] = Json(new
                {
                    statusCode = 200,
                    status = "Files Uploaded Successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                log.Info("Files uploading to database failed.");
                ViewData["UploadStatus"] = Json(new
                {
                    statusCode = 400,
                    status = "Bad Request! Upload Failed",
                    file = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(ViewData);
        }

        [HttpPost]
        public ActionResult LoadCEData(string fweek)
        {
            try
            {
                //fiscal week
                var fiscalWeek = fweek;

                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != NewMethod() ? Convert.ToInt32(length) : 0;

                int skip = start != NewMethod() ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                List<CE_DATA> cedataList;
                JArray[] jArrays;
                List<string> jsonlist = new List<string>();
                List<string[]> jsonresult;
                using (var context = new TODEntities())
                {
                    

                    // getting all Customer data  
                    /*var customerData = (from tempcustomer in _context.CustomerTB
                                        select tempcustomer);*/

                    var ce_data = (from t in context.CE_DATA select t);
                    var ce_datafiltered = (from s in ce_data
                                           where s.FW_ID == fiscalWeek
                                           select s);
                    var ce_dataOrderByDescResult = (from s in ce_datafiltered
                                                    orderby s.REP_GEN_DATE descending
                                                    select s);

                    /*var commentData = (from cd in context.CE_DATA
                                       .Where(cd => cd.FW_ID == fiscalWeek)
                                       .GroupBy(cd => cd.REP_GEN_DATE)
                                       .Select(cd => cd.OrderByDescending(cd => cd.CE_SEQUENCE_ID).FirstOrDefault())
                                       .Select(cd => new {
                                           c.CE_SEQUENCE_ID,
                                           c.FW_ID,
                                           c.REP_GEN_DATE,
                                           c.FW_START_DATE,
                                           c.FW_END_DATE,
                                           c.CREATED_DATE,
                                           c.CREATED_TIME,
                                           c.PUBLICATION,
                                           c.LANGUAGE,
                                           c.GUID,
                                           c.ACCURATE_RATING,
                                           c.USEFUL_RATING,
                                           c.EASY_TO_UNDERSTAND_RATING,
                                           c.ARTICLE_SOLVED_ISSUE,
                                           c.FEEDBACK
                                       }));

                    var cedata = from cd in context.CE_DATA
                                  .Where(c => c.FW_ID == fiscalWeek)
                                  .GroupBy(c => c.REP_GEN_DATE)
                                  .Select(g => g.OrderByDescending(c => c.REP_GEN_DATE).First())
                                  .Select(c => new {
                                      c.CE_SEQUENCE_ID,
                                      c.FW_ID,
                                      c.REP_GEN_DATE,
                                      c.FW_START_DATE,
                                      c.FW_END_DATE,
                                      c.CREATED_DATE,
                                      c.CREATED_TIME,
                                      c.PUBLICATION,
                                      c.LANGUAGE,
                                      c.GUID,
                                      c.ACCURATE_RATING,
                                      c.USEFUL_RATING,
                                      c.EASY_TO_UNDERSTAND_RATING,
                                      c.ARTICLE_SOLVED_ISSUE,
                                      c.FEEDBACK });*/
                    //Sorting  
                    /*if (!(string.IsNullOrEmpty(sortColumn.ToString()) && string.IsNullOrEmpty(sortColumnDirection.ToString())))
                    {
                        //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                        ce_dataOrderByDescResult = ce_dataOrderByDescResult.OrderBy(s = > ssortColumn.ToString() + " " + sortColumnDirection.ToString());
                    }
                    //Search  
                    if (!string.IsNullOrEmpty(searchValue.ToString()))
                    {
                        ce_dataOrderByDescResult = ce_dataOrderByDescResult.Where(m => m.Name == searchValue);
                    }*/

                    //total number of rows counts   
                    recordsTotal = ce_dataOrderByDescResult.Count();
                    //Paging   
                    var data = ce_dataOrderByDescResult.Skip(skip).Take(pageSize).ToList();

                    /*for (int i = 0; i < data.Count; i++)
                    {
                        CE_DATA ced = data[i];
                        List<string> jsonList = new List<string>();
                        jsonList.Add(Json(new {
                            REP_GEN_DATE = ced.REP_GEN_DATE,
                            FW_START_DATE = ced.FW_START_DATE,
                            FW_END_DATE = ced.FW_END_DATE ,
                            CREATED_DATE = ced.CREATED_DATE,
                            CREATED_TIME = ced.CREATED_TIME,
                            PUBLICATION = ced.PUBLICATION ,
                            LANGUAGE = ced.LANGUAGE ,
                            GUID = ced.GUID ,
                            Title = ced.Title ,
                            ACCURATE_RATING = ced.ACCURATE_RATING ,
                            USEFUL_RATING = ced.USEFUL_RATING ,
                            EASY_TO_UNDERSTAND_RATING = ced.EASY_TO_UNDERSTAND_RATING ,
                            ARTICLE_SOLVED_ISSUE = ced.ARTICLE_SOLVED_ISSUE ,
                            FEEDBACK = ced.FEEDBACK }).ToString());
                        jsonlist.Add(JsonConvert.SerializeObject(jsonList));
                    }*/

                    var result = (from ced in data select new [] {
                            ced.REP_GEN_DATE.ToString(),
                            ced.FW_START_DATE.ToString(),
                            ced.FW_END_DATE.ToString() ,
                            ced.CREATED_DATE.ToString(),
                            ced.CREATED_TIME.ToString(),
                            ced.PUBLICATION ,
                            ced.LANGUAGE ,
                            ced.GUID ,
                            ced.Title ,
                            ced.ACCURATE_RATING.ToString() ,
                            ced.USEFUL_RATING.ToString() ,
                            ced.EASY_TO_UNDERSTAND_RATING.ToString() ,
                            ced.ARTICLE_SOLVED_ISSUE ,
                            ced.FEEDBACK
                    });
                    log.Info("data : \n" + result.ToString());
                    jsonresult = result.ToList();
                }
               
                
                
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = jsonresult });
            }
            catch (Exception e)
            {
                log.Info("Exception Occurred while fetching data." + e.Message);
                return Json(new { error = e.Message });
            }
        }

        private static int? NewMethod()
        {
            return null;
        }
    }
}