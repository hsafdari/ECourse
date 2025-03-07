namespace Infrastructure.Utility
{
    public class GridQuery
    {
        //string filter, int? top, int? skip, string orderby
        public string sortColumn { get; set; } = "";
        public string sortOrder { get; set; } = "";
        public string filterColumn { get; set; } = "";
        public string filterValue { get; set; } = "";
        public int page { get; set; } = 1;
        public int skip { get; set; } = 0;
        public int top { get; set; } = 0;
        public string Filter { get; set; } = "";
    }
}
