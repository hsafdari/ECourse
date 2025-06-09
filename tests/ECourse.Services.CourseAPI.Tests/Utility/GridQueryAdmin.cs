using System.Text.Encodings.Web;

namespace ECourse.Services.CourseAPI.Tests.Utility
{
    public class GridQueryAdmin
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
        public static string GridFilterUrl(string url, GridQueryAdmin query)
        {
            Dictionary<string, object> dictionary = [];
            if (query.Page.HasValue)
            {
                dictionary.Add("page", query.Page);
            }
            if (query.Skip.HasValue)
            {
                dictionary.Add("skip", query.Skip);
            }

            if (query.Top.HasValue)
            {
                //equal page size for grid
                dictionary.Add("top", query.Top);
            }

            if (!string.IsNullOrEmpty(query.SortColumn))
            {
                dictionary.Add("sortColumn", query.SortColumn);
            }
            if (!string.IsNullOrEmpty(query.SortOrder))
            {
                dictionary.Add("sortOrder", query.SortOrder);
            }

            if (!string.IsNullOrEmpty(query.FilterColumn))
            {
                dictionary.Add("filterColumn", UrlEncoder.Default.Encode(query.FilterColumn));
            }
            if (!string.IsNullOrEmpty(query.FilterValue))
            {
                dictionary.Add("filterValue", query.FilterValue);
            }
            if (!string.IsNullOrEmpty(query.Filter))
            {
                dictionary.Add("Filter", query.Filter);
            }

            return string.Format("{0}{1}", url, dictionary.Any() ? ("?" + string.Join("&", dictionary.Select((KeyValuePair<string, object> a) => $"{a.Key}={a.Value}"))) : "");
        }
    }
}
