using ECourse.Services.Course.Application.Abstractions.CQRS;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Create
{
    public sealed record CreateCourseGroupCommand
    (string Title,
    string Slug,
    string? Description,
    Guid? ParentId,
    bool IsActive
    )
    : ICommand<Guid>
    {
        
    }
}
