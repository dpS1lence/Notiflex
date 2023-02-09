using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DTOs
{
    public class TriggerGetOneDto
    {
        public int Id { get; set; }

        public string Identity { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string City { get; set; } = null!;

        public int Hour { get; set; }

        public string Minutes { get; set; } = null!;

        public string DaySchedule { get; set; } = null!;
    }
}
