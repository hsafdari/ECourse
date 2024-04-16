using Middleware.Models;
namespace ECourse.Services.CourseAPI.Models
{
    public class CourseLevel:BaseEntity
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
    }
}
