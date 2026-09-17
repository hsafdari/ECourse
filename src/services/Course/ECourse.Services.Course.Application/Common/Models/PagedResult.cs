namespace ECourse.Services.Course.Application.Common.Models
{
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount)
    {
        public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling(
                TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
