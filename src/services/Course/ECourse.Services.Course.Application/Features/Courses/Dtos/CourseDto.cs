using ECourse.Services.Course.Application.Features.CoursePrices.Dtos;

namespace ECourse.Services.Course.Application.Features.Courses.Dtos
{
    public sealed record CourseDto
    {
        public required string Name { get; init; }
        public required string Title { get; init; }
        /// <summary>
        /// due to seo friendly admin can put customised url
        /// </summary>
        public required string UrlLink { get; init; }
        public string? Description { get; init; }
        public IList<CoursePriceDto> CoursePrices { get; init; } = default!;
    }
}
