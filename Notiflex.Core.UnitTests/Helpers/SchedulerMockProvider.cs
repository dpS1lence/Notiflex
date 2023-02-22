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
            var schedulerFactory = new Mock<IScheduler>();

            schedulerFactory.Setup(x => x.ScheduleJob(It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()));

            return schedulerFactory;
        }
    }
}
