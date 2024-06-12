using Infrastructure.Models;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class CourseSectionDto : BaseDto
    {
        public required string CourseId { get; set; }
        /// <summary>
        /// each section has title
        /// </summary>
        public required string Title { get; set; }
        public List<CourseItemDto> CourseItems = null!;
    }
}
