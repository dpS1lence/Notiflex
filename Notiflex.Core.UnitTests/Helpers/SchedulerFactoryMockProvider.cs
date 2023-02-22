using Moq;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.Contracts;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core.Helpers
{
    public class SchedulerFactoryMockProvider
    {
        public static Mock<ISchedulerFactory> MockSchedulerFactory()
        {
            var schedulerFactory = new Mock<ISchedulerFactory>();

            schedulerFactory.Setup(x => x.GetScheduler(It.IsAny<CancellationToken>()))
                .ReturnsAsync(SchedulerMockProvider.MockScheduler().Object);

            return schedulerFactory;
        }
    }
}
