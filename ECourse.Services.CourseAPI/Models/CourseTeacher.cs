using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class CourseTeacher:BaseEntity
    {
        //map with user Id
        public ObjectId UserId { get; set; }
        public ObjectId CourseId { get; set; }
    }
}
