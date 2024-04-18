using ECourse.Admin.Models;

namespace ECourse.Admin.Models.CourseAPI.CourseLevel
{
    public class CourseLevelDto : BaseDto
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
    }
}
