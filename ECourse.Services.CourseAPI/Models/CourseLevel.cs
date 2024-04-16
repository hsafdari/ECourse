using Middleware.Models;
namespace ECourse.Services.CourseAPI.Models
{
    /// <summary>
    /// each course has multiple level forexample Basic, intermidiate and advance
    /// </summary>
    public class CourseLevel:BaseEntity
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
        public List<Course> Courses { get; init; } = null!;
    }
}
