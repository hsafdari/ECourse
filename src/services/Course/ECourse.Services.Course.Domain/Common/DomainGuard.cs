using ECourse.Services.Course.Domain.Exceptions;

namespace ECourse.Services.Course.Domain.Common
{
    public static class DomainGuard
    {
        public static DateTimeOffset AgainstNonUtc(
        DateTimeOffset value,
        string parameterName)
        {
            if (value.Offset != TimeSpan.Zero)
            {
                throw new DomainException(
                    $"{parameterName} must use the UTC offset.");
            }

            return value;
        }

        public static string RequiredText(
            string? value,
            int maximumLength,
            string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(
                    $"{propertyName} is required.");
            }

            string normalizedValue = value.Trim();

            if (normalizedValue.Length > maximumLength)
            {
                throw new DomainException(
                    $"{propertyName} cannot exceed " +
                    $"{maximumLength} characters.");
            }

            return normalizedValue;
        }

        public static string? OptionalText(
            string? value,
            int maximumLength,
            string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string normalizedValue = value.Trim();

            if (normalizedValue.Length > maximumLength)
            {
                throw new DomainException(
                    $"{propertyName} cannot exceed " +
                    $"{maximumLength} characters.");
            }

            return normalizedValue;
        }
    }
}
