using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Entities
{
    /// <summary>
    /// due to multi currency, each course has main price
    /// </summary>
    public class CoursePrice:AggregateRoot<Guid>
    {
        public static readonly string DocumentName = nameof(CoursePrice);
        public decimal Price { get; set; }
        public required string  CurrencyCode { get; set;}
        public required int CourseId { get; set; }
    }
}
