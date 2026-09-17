using ECourse.Services.Course.Domain.Exceptions;

namespace ECourse.Services.Course.Domain.Common
{
    public class SoftDeletableAggrateRoot<TId> : AggregateRoot<TId>, IAuditableEntity, ISoftDeletableEntity where TId : notnull
    {
        public SoftDeletableAggrateRoot()
        {

        }
        public SoftDeletableAggrateRoot(TId id, DateTimeOffset createdAtUtc) : base(id)
        {
            CreatedAtUtc = createdAtUtc;
        }
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? ModifiedAtUtc { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTimeOffset? DeletedAtUtc { get; private set; }
        public void MarkAsModified(DateTimeOffset modifiedAtUtc)
        {
            EnsureNotDeleted();
            ModifiedAtUtc = DomainGuard.AgainstNonUtc(modifiedAtUtc, nameof(modifiedAtUtc));
        }
        public bool MarkAsDeleted(DateTimeOffset deletedAtUtc)
        {
            if (IsDeleted)
            {
                return false; // Already deleted, no action needed
            }
            IsDeleted = true;
            DeletedAtUtc = DomainGuard.AgainstNonUtc(deletedAtUtc, nameof(deletedAtUtc));
            ModifiedAtUtc = DeletedAtUtc;

            return true;
        }
        protected void EnsureNotDeleted()
        {
            if (IsDeleted)
            {
                throw new DomainException(
                    $"{GetType().Name} has been deleted " +
                    "and cannot be modified.");
            }
        }
    }
}
