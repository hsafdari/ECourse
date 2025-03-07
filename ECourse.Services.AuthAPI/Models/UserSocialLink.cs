using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.AuthAPI.Models
{
    public class UserSocialLink : BaseEntity
    {
        public static readonly string DocumentName = nameof(UserSocialLink);
        public ObjectId UserId { get; set; }
        public ObjectId SocialLinkId { get; set; }
        public required string Url { get; set; }
    }
}
