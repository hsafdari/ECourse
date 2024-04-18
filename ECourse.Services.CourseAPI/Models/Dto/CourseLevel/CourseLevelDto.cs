using Middleware.Models;

namespace ECourse.Services.CourseAPI.Models.Dto.CourseLevel
{
    public class CourseLevelDto:BaseDto
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
    }
}
