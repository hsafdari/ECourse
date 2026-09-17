

namespace ECourse.Services.AuthAPI.Models
{
    public class UserSocialLink 
    {
        public static readonly string DocumentName = nameof(UserSocialLink);
        public int UserId { get; set; }
        public int SocialLinkId { get; set; }
        public required string Url { get; set; }
    }
}
