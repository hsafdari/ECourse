using Middleware.Models;

namespace ECourse.Services.CourseAPI.Models
{
    public class CoursePrice:BaseEntity
    {
        public decimal Price { get; set; }
        public required string  CurrencyCode { get; set;}
        public required string CourseId { get; set; }
    }
}
