using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DTOs
{
    public class ProfileDto
    {
        public string? HomeTown { get; set; }

        public string? DefaultTime { get; set; }

        public string? ProfilePic { get; set; }

        public string? Description { get; set; }
        [Required]

        public string LastName { get; set; } = null!;
        [Required]

        public string FirstName { get; set; } = null!;
        [Required]

        public string TelegramChatId { get; set; } = null!;

    }
}
