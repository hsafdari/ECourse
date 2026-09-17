namespace ECourse.Services.Course.Application.Common.Exceptions
{
    public sealed class ConflictException : ApplicationLayerException
    {
        public ConflictException(string message) : base(message)
        {

        }
        public ConflictException(
        string message,
        Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
