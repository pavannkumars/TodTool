using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TODTool.App_Start;
using TODTool.Helpers;

namespace TODTool.TODSchedulers
{
    public class SchedulerJob : IJob,IDisposable
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IEBrowser browser;
        bool visible = true;


        public void Dispose()
        {
            //result = browser.HtmlResult;
            //if (!visible) browser.Dispose();
            browser.DisposeBrowser(true);
        }

        public void Execute(IJobExecutionContext context)
        {
            

            log.Info("Comment Extractor Execution started at : " + TodDateUtils.GetCurrentTimeInIST().Date);
            string userID = "delltools";
            string password = "g9eDx5IWlseQEeSvknxz";
            AutoResetEvent resultEvent = new AutoResetEvent(false);
            //string result = null;

            
            browser = new IEBrowser(visible, userID, password, resultEvent);
            //browser = new IEBrowser(visible, "delltools", "g9eDx5IWlseQEeSvknxz", resultEvent);

            // wait for the third thread getting result and setting result event
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });
            // the result is ready later than the result event setting somtimes 
            //while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            //result = browser.HtmlResult;
            //if (!visible) browser.Dispose();
            Dispose();
            //throw new NotImplementedException();

            log.Info("Comment Extractor Execution completed @ " + TodDateUtils.GetCurrentTimeInIST().Date);
            //return (Task)context.Result;
        }
    }
}