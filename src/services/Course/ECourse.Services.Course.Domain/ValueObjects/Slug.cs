using System.Text.RegularExpressions;
using ECourse.Services.Course.Domain.Exceptions;

namespace ECourse.Services.Course.Domain.ValueObjects
{
    public sealed partial record Slug
    {
        public const int MaxLength = 250;

        private Slug(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Slug Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Slug is required.");
            }

            string normalizedValue = value
                .Trim()
                .ToLowerInvariant();

            if (normalizedValue.Length > MaxLength)
            {
                throw new DomainException(
                    $"Slug cannot exceed {MaxLength} characters.");
            }

            if (!SlugPattern().IsMatch(normalizedValue))
            {
                throw new DomainException(
                    "Slug can contain only lowercase letters, " +
                    "numbers, and single hyphens.");
            }

            return new Slug(normalizedValue);
        }

        public override string ToString() => Value;

        [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
        public static partial Regex SlugPattern();
    }
}
