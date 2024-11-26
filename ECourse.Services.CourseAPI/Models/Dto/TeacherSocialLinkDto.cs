using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class TeacherSocialLinkDto : BaseDto
    {
        public ObjectId TeacherId { get; set; }
        public ObjectId SocialLinkId { get; set; }
        public required string Url { get; set; }
    }
}
