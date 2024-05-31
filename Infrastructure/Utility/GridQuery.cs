namespace Infrastructure.Utility
{
    public class GridQuery
    {
        //string filter, int? top, int? skip, string orderby
        public string? filter { get; set; } = default!;
        public int top { get; set; } = default!;
        public int skip { get; set; } = default!;
        public string? orderby { get; set; } = default!;
        public string? select { get; set; } = default!;
    }
}
