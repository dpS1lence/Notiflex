using Notiflex.Infrastructure.Data.Models.UserModels;
using Quartz;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notiflex.ViewModels
{
    public class TriggerAddViewModel
    {
        [Required]
        public string Identity { get; set; } = null!;
        [Required]

        public string City { get; set; } = null!;
        [Required]

        public int Interval { get; set; }
        [Required]

        public IntervalUnit IntervalUnit { get; set; }       
    }
}
