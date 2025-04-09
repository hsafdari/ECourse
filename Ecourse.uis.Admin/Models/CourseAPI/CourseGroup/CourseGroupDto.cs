namespace ECourse.Admin.Models.CourseAPI.CourseGroup
{
    public class CourseGroupDto : BaseDto
    {
        public string? ParentId { get; set; }
        public string Title { get; set; } = default!;
        public string CustomCode { get; set; } = default!;
        public bool HasChildren { get; set; } = false;
        public List<CourseGroupDto>? Children { get; set; }
    }
}
