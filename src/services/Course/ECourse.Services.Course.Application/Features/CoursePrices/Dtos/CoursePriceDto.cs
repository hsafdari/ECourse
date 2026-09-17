namespace ECourse.Services.Course.Application.Features.CoursePrices.Dtos
{
    public sealed record CoursePriceDto
    {
        public decimal Price { get; init; }
        public required string CurrencyCode { get; init; }
        public required string CourseId { get; init; }
    }
}
