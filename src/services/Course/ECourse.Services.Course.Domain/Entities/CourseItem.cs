
using ECourse.Services.Course.Domain.Common;
using ECourse.Services.Course.Domain.Enums;

namespace ECourse.Services.Course.Domain.Entities
{
    public class CourseItem:AggregateRoot<Guid>
    {
        public static readonly string DocumentName = nameof(CourseItem);
        public required string Name { get; set; }
        /// <summary>
        /// enable for users, if it is free.
        /// </summary>
        public bool IsPreview { get; set; } 
        /// <summary>
        /// the origin file name
        /// </summary>
        public required string FileName { get; set; }
        //Relative file location
        public string FileLocation { get; set; } = string.Empty;
        //Link of file which can be shown or downloaded
        public string FileUrl { get; set; } = string.Empty;
        /// <summary>
        /// file type extensions,auto filled
        /// </summary>
        public string FileExtension { get; set; } = string.Empty;
        /// <summary>
        /// the length of video or length of reading file or content
        /// </summary>
        public required string TimeDuration { get; set; }
        public required int CourseId { get; set; }
        public required int SectionId { get; set; }
        public string? Description { get; set; }
        public CourseType CourseType { get; set; }
        public List<CourseItemAttachment> CourseItemAttachments { get; init; } = null!;
    }

   
}
