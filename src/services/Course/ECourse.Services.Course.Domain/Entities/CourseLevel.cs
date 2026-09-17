
using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Entities
{
    /// <summary>
    /// each course has multiple level for example Basic, Intermediate and advance
    /// </summary>
    public class CourseLevel:AggregateRoot<Guid>
    {    
        public static readonly string DocumentName = nameof(CourseLevel);
        /// <summary>
        /// The name of courseTitle like Basic, Intermediate, Advance and ...
        /// </summary>
        public required string Title { get; set; }
        public required string Icon { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileLocation { get; set; } = string.Empty;
        // public List<Course> Courses { get; init; } = null!;
    }
}
