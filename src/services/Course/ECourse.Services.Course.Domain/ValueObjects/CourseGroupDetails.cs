using ECourse.Services.Course.Domain.Common;
using ECourse.Services.Course.Domain.Exceptions;
using ECourse.Services.Course.Domain.Rules;

namespace ECourse.Services.Course.Domain.ValueObjects
{
    public sealed partial record CourseGroupDetails
    {
        private CourseGroupDetails(string title, string slug, string? description, Guid? parentId, bool isActive)
        {
            Title = title;
            Slug = slug;
            Description = description;
            ParentId = parentId;
            IsActive = isActive;

        }
        public string Title { get; }
        public string Slug { get; }
        public string? Description { get; }
        public Guid? ParentId { get; set; }
        public string? CustomCode { get; set; }
        public bool HasChildren { get; set; }
        public bool IsActive { get; private set; }
        public static CourseGroupDetails Create(string title, string slug, string? description, Guid? parentId, bool isActive)
        {
            string normalizedTitle = DomainGuard.RequiredText(title, CourseGroupRules.MaxTitleLength, nameof(Title));

            string normalizedslug = DomainGuard.RequiredText(slug, CourseGroupRules.MaxSlugLength, nameof(Slug));

            string? normilizedDescription = DomainGuard.OptionalText(description, CourseGroupRules.MaxDescriptionLength, nameof(Description));
            string validSlug = ValueObjects.Slug.Create(slug).Value;

            if (!ValueObjects.Slug.SlugPattern().IsMatch(normalizedslug))
            {
                throw new CourseGroupDomainException(
                "Slug can contain only lowercase letters, " +
                "numbers, and single hyphens.");
            }

            return new CourseGroupDetails(normalizedTitle, normalizedslug, normilizedDescription, parentId, isActive);
        }
    }
}
