using ECourse.Services.Course.Application.Abstractions.CQRS;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Update;
public sealed record EditCourseGroupCommand(Guid Id, string Title, string Slug, string Description, Guid? ParentId, bool IsActive) : ICommand { }
