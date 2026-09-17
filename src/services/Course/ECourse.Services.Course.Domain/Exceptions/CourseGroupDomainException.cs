namespace ECourse.Services.Course.Domain.Exceptions
{
    public sealed class CourseGroupDomainException : DomainException
    {
        public CourseGroupDomainException(string message)
            : base(message)
        {

        }
    }
}
