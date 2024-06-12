using Infrastructure.Models;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class CourseDto : BaseDto
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        /// <summary>
        /// due to seo friendly admin can put customised url
        /// </summary>
        public required string UrlLink { get; set; }
        public string? Description { get; set; }
        public IList<CoursePriceDto> CoursePrices { get; set; } = default!;
    }
}
