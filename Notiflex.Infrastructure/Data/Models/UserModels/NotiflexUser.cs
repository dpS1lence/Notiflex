using Microsoft.AspNetCore.Identity;
using Notiflex.Common.Constants;
using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Infrastructure.Data.Models.UserModels
{
    public class NotiflexUser : IdentityUser
    {
        public NotiflexUser()
        {
            Triggers = new HashSet<NotiflexTrigger>();
        }

        [Required]
        [MinLength(DbValidationConstants.NameMinLength)]
        [MaxLength(DbValidationConstants.NameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(DbValidationConstants.NameMinLength)]
        [MaxLength(DbValidationConstants.NameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string Age { get; set; } = null!;

        [MinLength(DbValidationConstants.DescriptionMinLength)]
        [MaxLength(DbValidationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [MaxLength(DbValidationConstants.ProfilePicSizeMazLength)]
        public string? ProfilePic { get; set; }

        public string? TelegramInfo { get; set; }

        public string? DefaultTime { get; set; }

        [MinLength(DbValidationConstants.TownNameMinLength)]
        [MaxLength(DbValidationConstants.TownNameMaxLength)]
        public string? HomeTown { get; set; }
        public ICollection<NotiflexTrigger> Triggers { get; set; }
    }
}
