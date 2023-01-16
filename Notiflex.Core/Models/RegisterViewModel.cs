using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        //public string HomeTown { get; set; } = null!;

        //[Required]
        //public string DefaultTime { get; set; } = null!;

        //[Required]
        //public string TelegramInfo { get; set; } = null!;

        //[Required]
        //public string ProfilePic { get; set; } = null!;

        //[Required]
        //public string Gender { get; set; } = null!;

        //[Required]
        //public string Description { get; set; } = null!;

        //[Required]
        //public string Age { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
