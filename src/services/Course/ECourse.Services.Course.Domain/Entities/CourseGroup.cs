using ECourse.Services.Course.Domain.Common;
using ECourse.Services.Course.Domain.Events.CourseGroups;
using ECourse.Services.Course.Domain.ValueObjects;

namespace ECourse.Services.Course.Domain.Entities
{
    public sealed class CourseGroup : SoftDeletableAggrateRoot<Guid>
    {
        private CourseGroup() { }
        private CourseGroup(Guid id, CourseGroupDetails courseGroupDetails, DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
        {
            Id = id;
            ApplyDetails(courseGroupDetails);
        }

        private void ApplyDetails(CourseGroupDetails courseGroupDetails)
        {
            Title = courseGroupDetails.Title;
            Description = courseGroupDetails.Description;
            Slug = Slug.Create(courseGroupDetails.Slug);
            ParentId = courseGroupDetails.ParentId;
            IsActive = courseGroupDetails.IsActive;
        }

        public Guid? ParentId { get; private set; }
        public string Title { get; private set; }
        public Slug Slug { get; private set; }
        public string? Description { get; private set; } = default!;
        public bool IsActive { get; private set; }
        public CourseGroup? Parent { get; private set; } = default!;
        public static CourseGroup Create(CourseGroupDetails details, DateTimeOffset utcnow)
        {
            ArgumentNullException.ThrowIfNull(details);

            var courseGroup = new CourseGroup(Guid.NewGuid(), details, utcnow);

            courseGroup.RaiseDomainEvent(new CourseGroupCreatedDomainEvent(courseGroup.Id, utcnow));
            return courseGroup;
        }
        public void UpdateDetails(CourseGroupDetails details, DateTimeOffset modifiedAtUtc)
        {
            ArgumentNullException.ThrowIfNull(details);
            EnsureNotDeleted();
            if (hasSomeDetails(details))
            {
                return;
            }
            ApplyDetails(details);
            MarkAsModified(modifiedAtUtc);
            RaiseDomainEvent(new CourseGroupDetailsUpdatedDomainEvent(Id, modifiedAtUtc));
        }
        public void DeActive(DateTimeOffset modifiedAtUtc)
        {
            EnsureNotDeleted();
            if (!IsActive)
            {
                return;
            }
            IsActive = false;
            MarkAsModified(modifiedAtUtc);

            RaiseDomainEvent(new CourseGroupDeactivedDomainEvent(Id, modifiedAtUtc));
        }
        public void Activate(DateTimeOffset modifiedAtUtc)
        {
            EnsureNotDeleted();
            if (IsActive)
            {
                return;
            }
            IsActive = true;
            MarkAsModified(modifiedAtUtc);
            RaiseDomainEvent(new CourseGroupActivedDomainEvent(Id, modifiedAtUtc));
        }
        public void Deleted(DateTimeOffset deletedAtUtc)
        {
            if (!MarkAsDeleted(deletedAtUtc))
            {
                return;
            }
            IsActive = false;

            RaiseDomainEvent(new CourseGroupDeletedDomainEvent(Id, deletedAtUtc));
        }
        private bool hasSomeDetails(CourseGroupDetails details)
        {
            return Title == details.Title &&
               Slug.Value == details.Slug &&
               IsActive == details.IsActive &&
               ParentId== details.ParentId &&
               Description == details.Description;
        }
    }
}
