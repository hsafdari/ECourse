using Infrastructure.Models;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class CoursePriceDto : BaseDto
    {
        public decimal Price { get; set; }
        public required string CurrencyCode { get; set; }
        public required string CourseId { get; set; }
    }
}
