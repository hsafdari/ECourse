namespace ECourse.Services.Course.Application.Features.CourseGroups.GetById;

public sealed record CourseGroupDetailsResponse(
    Guid Id,
    string Title,
    string Slug,
    string? Description,
    bool IsActive,
    Guid? ParentId,
    DateTimeOffset CreatedUtc,
    DateTimeOffset? ModifiedUtc);
