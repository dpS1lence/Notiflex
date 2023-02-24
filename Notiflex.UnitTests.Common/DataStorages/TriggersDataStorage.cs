using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Common.DataStorages
{
    public class TriggersDataStorage
    {
        public TriggersDataStorage() 
        {
            Triggers = new List<NotiflexTrigger>();

            InstantiateTriggers();
        }

        public List<NotiflexTrigger> Triggers { get; private set; }

        public NotiflexTrigger Trigger { get; private set; }

        public NotiflexTrigger TriggerWrongCity { get; private set; }

        public NotiflexTrigger TriggerWrongUserId { get; private set; }

        public NotiflexTrigger TriggerWrongTime { get; private set; }

        private void InstantiateTriggers()
        {
            NotiflexTrigger trigger = new()
            {
                Identity = "Identity",
                Id = 1,
                City = "Sofia",
                Interval = 1000,
                UserId = "1",
                Hour = 2,
                Minutes = "20",
                Name = "TestTrigger",
                DaysOfWeek = "Monday, Friday"
            };
            Trigger = trigger;
            Triggers.Add(trigger);

            NotiflexTrigger triggerWrongCity = new()
            {
                Id = 1,
                City = null,
                Interval = 1000,
                UserId = "1",
                Hour = 2,
                Minutes = "20",
                Name = "TestTrigger",
                DaysOfWeek = "Monday, Friday"
            };
            TriggerWrongCity = triggerWrongCity;
            Triggers.Add(triggerWrongCity);

            NotiflexTrigger triggerWrongUserId = new()
            {
                Id = 1,
                City = "Sofia",
                Interval = 1000,
                UserId = null,
                Hour = 2,
                Minutes = "20",
                Name = "TestTrigger",
                DaysOfWeek = "Monday, Friday"
            };
            TriggerWrongUserId = triggerWrongUserId;
            Triggers.Add(triggerWrongUserId);

            NotiflexTrigger triggerWrongTime = new()
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
            TriggerWrongTime = triggerWrongTime;
            Triggers.Add(TriggerWrongTime);
        }
    }
}
