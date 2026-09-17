namespace ECourse.Services.Course.API.Contracts.CourseGroups.Requests
{
    public class CourseGroupGridRequest
    {
        //string filter, int? top, int? skip, string orderby

        public string sortColumn { get; init; }
        public string sortOrder { get; init; }
        public string filterColumn { get; init; }
        public string filterValue { get; init; }
        public int page { get; init; } = 1;
        public int skip { get; init; } = 0;
        public int top { get; init; } = 0;
        public string Filter { get; init; }
    }
}
