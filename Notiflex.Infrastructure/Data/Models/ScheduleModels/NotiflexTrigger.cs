﻿using Notiflex.Infrastructure.Data.Models.UserModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Infrastructure.Data.Models.ScheduleModels
{
    public class NotiflexTrigger
    {
        [Key]
        public int Id { get; set; }

        public string Identity { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Minutes { get; set; } = null!;

        public int Hour { get; set; }

        public int Interval { get; set; }

        public string DaysOfWeek { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public NotiflexUser? User { get; set; }

    }
}
