using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODTool.Helpers;

namespace TODTool.TODSchedulers
{
    public class TodScheduler
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool schStatus = true;
        public static bool GetSchStatus()
        {
            return schStatus;
        }
        public static string schedulerJobMessage = "";

        public static string GetSchedulerJobMessage()
        {
            return schedulerJobMessage;
        }

        public static void StartAsync()
        {
            bool jobExecStatus = false;
            string schlogMessage = "";
            try
            {
                //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                log.Info("Scheduler instance invoked.");
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                IScheduler scheduler = schedulerFactory.GetScheduler();

                IJobDetail job = JobBuilder.Create<SchedulerJob>().Build();
                log.Info("Job instance created");

                ITrigger trigger = TriggerBuilder.Create()

                    .WithIdentity("CEExtractorJob", "CEJOB")

                    //"0 30 10 - 13 ? *WED,FRI"
                    //"0 0 11 ? *MON"
                    //.WithCronSche1dule("0 20 * 1/1 * ? *")

                    //every 15 minutes
                    //.WithCronSchedule("0 0/50 * * * ?")

                    //every mon @ 1 PM IST Run the job and extract Comment Extractor Details 
                    .WithCronSchedule("0 0 13 ? * MON")

                    //.StartAt(DateTime.UtcNow)
                    .StartNow()

                    .WithPriority(1)

                    .Build();

                log.Info("Job Trigger created");

                scheduler.ScheduleJob(job, trigger);

                schedulerJobMessage = "Comment Extraction Scheduled to run @ " + TodDateUtils.GetCurrentTimeInIST();

                log.Info("Scheduler scheduled instantiation successfully");

                //register to listeners

                // add scheduler listener
                //scheduler.ListenerManager.AddSchedulerListener();

                TodSchedulerJobListener todjoblistener = new TodSchedulerJobListener("CESchJobListener");

                JobKey jk = new JobKey("CEExtractorJob", "CEJOB");

                // add global job listener
                scheduler.ListenerManager.AddJobListener(todjoblistener, GroupMatcher<JobKey>.AnyGroup());

                // add global trigger listener
                // scheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());

                //sched.ListenerManager.AddJobListener(todjoblistener, KeyMatcher<JobKey>.KeyEquals(jk));

                log.Info("Scheduler registered successfully for job listener");

                schlogMessage = "Scheduler scheduled successfully @ " + TodDateUtils.GetCurrentTimeInIST();

                scheduler.Start();

                log.Info("scheduler instance created and started");

                jobExecStatus = true;
            }
            catch (Exception e)
            {
                log.Debug("Exception Occured. Detailed Exception is : ");
                log.Debug("--------");
                log.Debug(e.Message);
                log.Debug("---------");
                log.Debug("Stacktrace of the error is : \n" + e.StackTrace);
                schlogMessage = "Comment Extraction scheduled for " + DateTime.Now + " failed. Please check with Administrator for more details.";
                jobExecStatus = false;
            }

            //save the execution status with message to database scheduler log.

            try
            {
                using (var ctx = new TODEntities())
                {
                    TOD_TransactionLog todlog = new TOD_TransactionLog();
                    todlog.logtype = "CESCHLOG";
                    todlog.logdate = DateTime.Now.Date.ToString();
                    todlog.logMessage = schlogMessage;
                    todlog.logStatus = jobExecStatus.ToString();
                    ctx.TOD_TransactionLog.Add(todlog);

                    ctx.SaveChanges();
                }

            }
            catch (Exception e)
            {
                log.Debug("Exception Occured. Detailed Exception is : ");
                log.Debug("--------");
                log.Debug(e.Message);
                log.Debug("---------");
                log.Debug("Stacktrace of the error is : \n" + e.StackTrace);
                schlogMessage = "Database Connection lazy initialization failed. Please check with administrator for more details.";
                jobExecStatus = false;
            }
        }
    }


}