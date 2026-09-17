namespace ECourse.Services.Course.API.Contracts.CourseGroups.Responses;

public sealed record EditCourseGroupResponse
(
 Guid Id,
 string Title,
 string Slug,
 string Description,
 bool IsActive,
 Guid? ParentId,
 DateTimeOffset CreatedAt,
 DateTimeOffset UpdatedAt
 );

