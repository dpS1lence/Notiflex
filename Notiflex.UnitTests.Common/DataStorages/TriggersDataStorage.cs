using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Common.DataStorages
{
    public static class TriggersDataStorage
    {
        private static readonly NotiflexTrigger trigger = new()
        {
            Id = 1,
            City = "Sofia",
            Interval = 1000,
            UserId = "1",
            Hour = 2,
            Minutes = "20",
            Name = "TestTrigger",
            DaysOfWeek = "Monday, Friday"
        };

        private static readonly NotiflexTrigger triggerWrongCity = new()
        {
            Id = 1,
            City = "asdasdasdasdasd",
            Interval = 1000,
            UserId = "1",
            Hour = 2,
            Minutes = "20",
            Name = "TestTrigger",
            DaysOfWeek = "Monday, Friday"
        };

        private static readonly NotiflexTrigger triggerWrongUserId = new()
        {
            Id = 1,
            City = "Sofia",
            Interval = 1000,
            UserId = "-1",
            Hour = 2,
            Minutes = "20",
            Name = "TestTrigger",
            DaysOfWeek = "Monday, Friday"
        };

        private static readonly NotiflexTrigger triggerWrongTime = new()
        {
            Id = 1,
            City = "Sofia",
            Interval = 1000,
            UserId = "1",
            Hour = 222,
            Minutes = "222",
            Name = "TestTrigger",
            DaysOfWeek = "Monday, Friday"
        };

        public static NotiflexTrigger Trigger { get => trigger; }

        public static NotiflexTrigger TriggerWrongCity { get => triggerWrongCity; }

        public static NotiflexTrigger TriggerWrongUserId { get => triggerWrongUserId; }

        public static NotiflexTrigger TriggerWrongTime { get => triggerWrongTime; }
    }
}
