using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class Teacher:BaseEntity
    {
        public static readonly string DocumentName = nameof(Teacher);
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Photo { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileLocation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<TeacherSocialLink> TeacherSocialLinks { get; set; } = null!;
        //connect UserId to teacher's Id
        public ObjectId? UserId { get; set; }
    }
}
