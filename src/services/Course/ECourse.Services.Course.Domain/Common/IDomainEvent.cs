namespace ECourse.Services.Course.Domain.Common
{
    public interface IDomainEvent
    {
        DateTimeOffset OccuredOnUtc { get; }
    }
}
