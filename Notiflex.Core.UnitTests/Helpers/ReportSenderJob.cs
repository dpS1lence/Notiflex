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
        private int _counter;
        public ReportSenderJob()
        {
            this._counter = 0;
        }
        public int Counter
        {
            get { return this._counter; }
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
