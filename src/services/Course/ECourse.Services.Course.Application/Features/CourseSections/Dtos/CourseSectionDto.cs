using ECourse.Services.Course.Application.Features.CourseItems.Dtos;

namespace ECourse.Services.Course.Application.Features.CourseSections.Dtos
{
    public sealed record CourseSectionDto
    {
        public required string CourseId { get; set; }
        /// <summary>
        /// each section has title
        /// </summary>
        public required string Title { get; set; }
        public List<CourseItemDto> CourseItems = null!;
    }
}
