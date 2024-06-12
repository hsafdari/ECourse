using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    /// <summary>
    /// due to multi currency, each course has main price
    /// </summary>
    public class CoursePrice:BaseEntity
    {
        public static readonly string DocumentName = nameof(CoursePrice);
        public decimal Price { get; set; }
        public required string  CurrencyCode { get; set;}
        public required ObjectId CourseId { get; set; }
    }
}
