using Notiflex.Infrastructure.Data.Models.UserModels;
using Quartz;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notiflex.ViewModels
{
    public class TriggerAddViewModel
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
