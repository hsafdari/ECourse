using ECourse.Services.Course.Application.Abstractions.CQRS;

namespace ECourse.Services.Course.Application.Features.CourseGroups.GetList;

public sealed record GetCourseGroupsByParentIdQuery(Guid? ParentId) : IQuery<IReadOnlyList<CourseGroupTreeResponse>>;
