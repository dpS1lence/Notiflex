using Microsoft.AspNetCore.Identity;
using Notiflex.Common.Constants;
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

        [Required]
        [MinLength(DbValidationConstants.DESCRIPTION_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        [MaxLength(DbValidationConstants.PROFILE_PIC_SIZE_MAZ_LENGTH)]
        public string ProfilePic { get; set; } = null!;

        [Required]
        public string TelegramInfo { get; set; } = null!;

        [Required]
        public string DefaultTime { get; set; } = null!;

        [Required]
        [MinLength(DbValidationConstants.TOWN_NAME_MIN_LENGTH)]
        [MaxLength(DbValidationConstants.TOWN_NAME_MAX_LENGTH)]
        public string HomeTown { get; set; } = null!;
    }
}
