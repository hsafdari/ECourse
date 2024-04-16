using Middleware.Models;

namespace ECourse.Services.CourseAPI.Models
{
    /// <summary>
    /// due to multi currency, each course has main price
    /// </summary>
    public class CoursePrice:BaseEntity
    {
        public decimal Price { get; set; }
        public required string  CurrencyCode { get; set;}
        public required string CourseId { get; set; }
    }
}
