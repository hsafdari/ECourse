namespace ECourse.Services.Course.Application.Features.CourseGroups.GetById;

public interface IGetCourseGroupByIdReadService
{
    Task<CourseGroupDetailsResponse?> GetyIdAsync(Guid id, CancellationToken cancellationToken = default);
}
