using ECourse.Services.CourseAPI.Enums;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class CourseItemDto
    {
        public required string Name { get; set; }
        /// <summary>
        /// if user didn't buy course it shows if enable
        /// </summary>
        public bool IsPreview { get; set; }
        /// <summary>
        /// the location of file
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// the origin file name
        /// </summary>
        public required string FileName { get; set; }
        public string FileLocation { get; set; } = string.Empty;
        /// <summary>
        /// the length of video or length of reading file or content
        /// </summary>
        public required string Timeduration { get; set; }
        public required string CourseId { get; set; }
        public required string SectionId { get; set; }
        public string? Description { get; set; }
        public CourseType CourseType { get; set; }
    }
}
