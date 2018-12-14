using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TODTool.Helpers;
using TODTool.Models;

namespace TODTool.TodBL
{
    public class TODDataService
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public List<string> GetCEWKList()
        {
            CEDataModel cedam = new CEDataModel();
            using (var ctx = new TODEntities())
            {
                var result = (from ta in ctx.FW_CALENDAR select ta.Fiscal_Week).Distinct();
                cedam.CeWkDataList = result.ToList();
                /*List<FW_CALENDAR> fwdataList = ctx.FW_CALENDAR.Distinct().ToList();
                foreach(var cedata in fwdataList)
                {
                     cedam.CeWkDataList.Add(cedata.Fiscal_Week);
                }
                /*var result = (from ta in ctx.CE_DATA select ta.FW_ID).Distinct();
                
                cedam.ceWkDataList = result.ToList();*/
            }
            return cedam.CeWkDataList;
        }

        private class TempP
        {
            public string Name { get; set; }
            public string Code { get; set; }

            public string Brand { get; set; }
            public string category { get; set; }

           public string parentproduct { get; set; }
        }
        public List<string> GetProductList()
        {
            List<string> prdnameList = new List<string>();

            using (var ctx = new TODEntities())
            {
                var qresult =
                    (from om in ctx.Products
                     join fd in ctx.Product_Variant
                     on new { om.Name, om.Code } equals new { fd.Name, fd.Code }
                     select new TempP() {
                           Name = om.Name,
                           Code = om.Code,
                           Brand = om.Brand,
                           category = om.Category,
                           parentproduct = fd.Parent_Product

                     }).Distinct().ToList();
                /*var qresult = (from om in ctx.Products
                              join fd in ctx.Product_Variant
                              on om.Code equals fd.Code
                              select om).Distinct().ToList();
                /*var qresult = (from om in ctx.Products select om.Name).ToList();*/
                List<TempP> omDataList = qresult;
                foreach(var item in omDataList)
                {
                    if(!prdnameList.Contains(item.Name))
                    {
                        prdnameList.Add(item.Name);
                    }
                }
           }
            return prdnameList;
       }

       public IEnumerable<SelectListItem> GetEnumFWKList()
       {
           CEDataModel cedam = new CEDataModel();
           using (var ctx = new TODEntities())
           {
               /*var result = (from ta in ctx.FW_CALENDAR select ta.Fiscal_Week).Distinct();
               cedam.CeWkDataList = result.ToList();

               List<CE_DATA> cedatalist = ctx.CE_DATA.Distinct().ToList();
               cedam.CeDATAList = cedatalist;
               cedam.CeFwList = cedatalist.Select(m =>
                               new SelectListItem()
                               {
                                   Text = m.FW_ID,
                                   Value = m.FW_ID
                               });*/
            }

            return cedam.CeFwList;
        }

        public List<CE_DATA> GetCommentExtractDataFor(string fiscalweek)
        {
            return null;
        }


        public OMNITURE_DATA AppendFileData(OMNITURE_DATA omData, HttpPostedFileBase uploadedFile, string fweek)
        {
            bool isFileSaved = false;
            try
            {


                /*var qresult = (from om in ctx.OMNITURE_DATA
                               join fd in ctx.FW_CALENDAR
                               on om.FW_ID equals fd.Fiscal_Week
                               where om.FW_ID == fweek
                               select om).ToList();
                List<OMNITURE_DATA> omDataList = qresult.Distinct().ToList();

               if(omDataList.Count == 0)
                {
                    log.Info("Omniture Reports Records not found for the Week : (" + fweek + ")");
                }
                */

                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);
                string FileName = uploadedFile.FileName;



                if (FileName.Contains("c58") || FileName.Contains("TOD")) // TOD Report file
                {
                    omData.TOD_REPORT = FileByteArray;
                }
                else if (FileName.Contains("ProductCD")) // Product Code File
                {
                    omData.PRODUCT_CODE = FileByteArray;
                }
                else if (FileName.Contains("Language")) // Language Report
                {
                    omData.LANGUAGE_REPORT = FileByteArray;
                }
                else if (FileName.Contains("Product Name")) // product Name Report file
                {
                    omData.PRODUCT_NAME = FileByteArray;
                }
                else if (FileName.Contains("Publication ID")) // publication id Report File
                {
                    omData.PUBLICATION_ID = FileByteArray;
                }
                else if (FileName.Contains("GUID")) // topic guid report file
                {
                    omData.TOPIC_GUID = FileByteArray;
                }
                else if (FileName.Contains("Topic Title")) // topic title report file
                {
                    omData.TOPIC_TITLE = FileByteArray;
                }
                else if(FileName.Contains("Manual Title"))
                {
                    omData.MANUAL_TITLE = FileByteArray;
                }
                
                isFileSaved = true;
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                isFileSaved = false;
            }
            return omData;
        }

        public bool SaveOrUpdateOMRecords(OMNITURE_DATA omData, string fweek)
        {
            bool isFileSaved = false;
            try
            {
                using (var ctx = new TODEntities())
                {
                    DateTime currDate = TodDateUtils.GetCurrentTimeInIST();
                    string fiscalweek = TodDateUtils.GetWeekOfYearFor(TodDateUtils.GetCurrentTimeInIST());
                    DateTime ceStartDate = currDate.AddDays(-7).Date;
                    DateTime ceEndDate = currDate.AddDays(-1).Date;


                    omData.FW_ID = fweek;
                    omData.FW_START_DATE = ceStartDate;
                    omData.FW_END_DATE = ceEndDate;
                    omData.REP_GEN_DATE = currDate;

                    omData.OMNITURE_STATUS = "Success";
                    ctx.OMNITURE_DATA.Attach(omData);
                    ctx.Entry<OMNITURE_DATA>(omData).State = omData.SEQUENCE_ID == 0 ? EntityState.Added : EntityState.Unchanged;
                    ctx.SaveChanges();
                }
                isFileSaved = true;
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                isFileSaved = false;
            }
            return isFileSaved;
        }
    }
}
 