using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Events.CourseGroups
{
    public sealed record CourseGroupCreatedDomainEvent(Guid CourseGroupId, DateTimeOffset OccuredOnUtc) : IDomainEvent;
}
