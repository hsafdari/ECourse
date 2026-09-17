using ECourse.Services.Course.Application.Abstractions.CQRS;

namespace ECourse.Services.Course.Application.Features.CourseGroups.GetById
{
    public sealed record GetCourseGroupByIdQuery(Guid Id) : IQuery<CourseGroupDetailsResponse>;
}
