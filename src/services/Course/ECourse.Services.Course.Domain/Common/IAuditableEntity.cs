namespace ECourse.Services.Course.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTimeOffset CreatedAtUtc { get; }
        DateTimeOffset? ModifiedAtUtc { get; }

    }
}
