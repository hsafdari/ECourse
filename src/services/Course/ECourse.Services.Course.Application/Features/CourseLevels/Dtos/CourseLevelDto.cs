namespace ECourse.Services.Course.Application.Features.CourseLevels.Dtos
{
    public sealed record CourseLevelDto
    {
        public string Title { get; init; } = default!;
        public string Icon { get; init; } = default!;
        public string FileName { get; init; } = default!;
        public string FileLocation { get; init; } = default!;
    }
}
