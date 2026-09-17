namespace ECourse.Services.Course.Application.Features.CourseGroups.GetList
{
    public record CourseGroupTreeResponse
    (
       Guid Id,
       string Title,
       string Slug,
       string? Description,
       bool IsActive,
       Guid? ParentId,
       bool HasChildren,
       DateTimeOffset CreatedUtc,
       DateTimeOffset? ModifiedUtc
     );
}
