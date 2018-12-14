using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TODTool.TODSchedulers
{
    public class TodTriggerListener : ITriggerListener
    {
        public string Name => throw new NotImplementedException();

        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            throw new NotImplementedException();
        }

        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public void TriggerMisfired(ITrigger trigger)
        {
            throw new NotImplementedException();
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}