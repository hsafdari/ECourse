namespace ECourse.Services.Course.API.Contracts.CourseGroups.Requests
{
    public sealed record CreateCourseGroupRequest
    (
        string Title,
        string Slug,
        string Description,
        bool IsActive,
        Guid? ParentId
    );
}
