namespace ECourse.Services.Course.Application.Common.Exceptions
{
    public abstract class ApplicationLayerException : Exception
    {
        protected ApplicationLayerException(string message) : base(message)
        {

        }
        protected ApplicationLayerException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
