namespace ECourse.Admin.Utility
{
    /// <summary>
    /// this class could not be the same with Infrastructure class because we get query from Radzan
    /// then create url then send it thorough api
    /// </summary>
    public class GridQuery
    {
        public string SortColumn { get; set; } = default!;
        public string SortOrder { get; set; } = default!;
        public string FilterColumn { get; set; } = default!;
        public string FilterValue { get; set; } = default!;
        public int? Page { get; set; } = default!;
        public int PageSize { get; set; } = default!;
        public int? Skip { get; set; } = default!;
        public int? Top { get; set; } = default!;
        public string Filter { get; set; } = default!;
    }
}
