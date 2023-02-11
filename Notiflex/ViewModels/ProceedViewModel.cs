using System.ComponentModel.DataAnnotations;

namespace Notiflex.ViewModels
{
    public class ProceedViewModel
    {
        [Required]
        public string TelegramId { get; set; } = null!;

        [Required]
        public string HomeTown { get; set; } = null!;
        [Required]
        
        public string PhotoUrl { get; set; } = null!;
    }
}
