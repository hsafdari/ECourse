using ECourse.Services.Course.Application.Abstractions.CQRS;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Delete;
public sealed record DeleteCourseGroupCommand(Guid Id) : ICommand { }
