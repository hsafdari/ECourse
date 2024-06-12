using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class CourseTeacher:BaseEntity
    {
        public ObjectId TeacherId { get; set; }
        public ObjectId CourseId { get; set; }
    }
}
