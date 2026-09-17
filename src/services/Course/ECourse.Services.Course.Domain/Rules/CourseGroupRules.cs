namespace ECourse.Services.Course.Domain.Rules
{
    public static class CourseGroupRules
    {
        public const int MaxTitleLength = 150;

        public const int MaxSlugLength = 250;

        public const int MaxDescriptionLength = 1_000;
    }
}
