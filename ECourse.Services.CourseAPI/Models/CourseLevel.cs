using Infrastructure.Models;
namespace ECourse.Services.CourseAPI.Models
{
    /// <summary>
    /// each course has multiple level for example Basic, Intermediate and advance
    /// </summary>
    public class CourseLevel:BaseEntity
    {
        public static readonly string DocumentName = nameof(CourseLevel);
        public required string Title { get; set; }
        public required string Icon { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileLocation { get; set; } = string.Empty;
        // public List<Course> Courses { get; init; } = null!;
    }
}
