using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core.Helpers
{
    public class ReportSenderJob : IJob
    {
        private int counter;
        public ReportSenderJob()
        {
            this.counter = 0;
        }
        public int Counter
        {
            get { return this.counter; }
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
