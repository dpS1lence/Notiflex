using Moq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core.Helpers
{
    public class SchedulerMockProvider
    {
        public static Mock<IScheduler> MockScheduler()
        {
            var scheduler = new Mock<IScheduler>();

            scheduler.Setup(x => x.ScheduleJob(It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()));

            scheduler.Setup(x => x.UnscheduleJob(It.IsAny<TriggerKey>(), It.IsAny<CancellationToken>()));

            return scheduler;
        }
    }
}
