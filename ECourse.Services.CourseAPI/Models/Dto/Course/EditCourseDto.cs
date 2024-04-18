namespace ECourse.Services.CourseAPI.Models.Dto.Course
{
    public class EditCourseDto
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        /// <summary>
        /// due to seo friendly admin can put customised url
        /// </summary>
        public required string UrlLink { get; set; }
        public string? Description { get; set; }
        
    }
}
