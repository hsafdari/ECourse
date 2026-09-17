namespace ECourse.Services.Course.Application.Common.Exceptions
{
    public sealed class NotFoundException : ApplicationLayerException
    {
        public NotFoundException(
        string resourceName,
        object resourceKey)
        : base(
            $"{resourceName} with identifier " +
            $"'{resourceKey}' was not found.")
        {
        }
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
