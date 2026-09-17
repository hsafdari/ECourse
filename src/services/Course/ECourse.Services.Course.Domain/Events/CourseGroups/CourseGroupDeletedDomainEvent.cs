using ECourse.Services.Course.Domain.Common;


namespace ECourse.Services.Course.Domain.Events.CourseGroups
{
    public sealed record CourseGroupDeletedDomainEvent(Guid CourseGroupId, DateTimeOffset OccuredOnUtc) : IDomainEvent;
}
