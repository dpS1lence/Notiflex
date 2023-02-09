namespace Notiflex.ViewModels
{
    public class ProfileViewModel
    {
        public string? HomeTown { get; set; }

        public string? ProfilePic { get; set; }

        public string? Description { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string TelegramChatId { get; set; } = null!;
    }
}
