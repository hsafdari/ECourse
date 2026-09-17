using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Events.CourseGroups
{
    public sealed record CourseGroupDetailsUpdatedDomainEvent(Guid CourseGroupId, DateTimeOffset OccuredOnUtc) : IDomainEvent;

}
