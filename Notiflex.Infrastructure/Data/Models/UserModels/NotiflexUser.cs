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
        [MinLength(DbValidationConstants.NAME_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.NAME_MAX_LENGTH)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(DbValidationConstants.NAME_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.NAME_MAX_LENGTH)]
        public string LastName { get; set; } = null!;

        [Required]
        public string Age { get; set; } = null!;

        [MinLength(DbValidationConstants.DESCRIPTION_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.DESCRIPTION_MAX_LENGTH)]
        public string? Description { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [MaxLength(DbValidationConstants.PROFILE_PIC_SIZE_MAZ_LENGTH)]
        public string? ProfilePic { get; set; }

        public string? TelegramInfo { get; set; }

        public string? DefaultTime { get; set; }

        [MinLength(DbValidationConstants.TOWN_NAME_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.TOWN_NAME_MAX_LENGTH)]
        public string? HomeTown { get; set; }
        public ICollection<NotiflexTrigger> Triggers { get; set; }
    }
}
