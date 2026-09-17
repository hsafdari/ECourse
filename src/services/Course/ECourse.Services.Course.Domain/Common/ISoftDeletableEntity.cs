namespace ECourse.Services.Course.Domain.Common
{
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; }
        DateTimeOffset? DeletedAtUtc { get; }
    }
}
