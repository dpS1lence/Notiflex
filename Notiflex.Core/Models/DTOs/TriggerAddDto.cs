using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DTOs
{
    public class TriggerAddDto
    {
        public int Id { get; set; }

        [Required]
        public string Identity { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public int Hour { get; set; }

        [Required]
        public string Minutes { get; set; } = null!;

        [Required]
        public string Meridiem { get; set; } = null!;

        [Required]
        public Dictionary<DayOfWeek, bool> DaySchedule { get; set; } = null!;
    }
}
