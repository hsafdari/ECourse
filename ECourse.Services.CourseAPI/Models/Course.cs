using Middleware.Models;

namespace ECourse.Services.CourseAPI.Models
{
    public class Course:BaseEntity
    {
        public static readonly string DocumentName = nameof(Course);
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string UrlLink { get; set; }
        public string? Description { get; set; }
        public float RatePercent { get; set; } = 0;
        public int RateCount { get; set; } = 0;     
        public List<CourseSection> Sections { get; init; } = null!;
        public List<CoursePrice> Prices { get; init; } = null!;

    }
}
