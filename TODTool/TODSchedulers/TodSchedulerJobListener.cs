using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TODTool.TODSchedulers
{
    public class TodSchedulerJobListener : IJobListener
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string listenerName = "";

        public TodSchedulerJobListener(string name)
        {
            this.listenerName = name;
        }
        public string Name => listenerName;

        
        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Task vetoed");
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                //skip
            }
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Task to be Executed");
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                //skip 
            }
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            try
            {
                Console.WriteLine("Task was executed");
            }
            catch (Exception e)
            {
                log.Info(e.Message);
            }
        }
    }

}