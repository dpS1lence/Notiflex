using Notiflex.Infrastructure.Data.Models.UserModels;
using Quartz;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notiflex.ViewModels
{
    public class TriggerGetOneViewModel
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
