using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Infrastructure.Models;
using ECourse.Services.CourseAPI.Enums;

namespace ECourse.Services.CourseAPI.Models
{
    public class CourseItem:BaseEntity
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
        public required ObjectId CourseId { get; set; }
        public required ObjectId SectionId { get; set; }
        public string? Description { get; set; }
        public CourseType CourseType { get; set; }
        public List<CourseItemAttachment> CourseItemAttachments { get; init; } = null!;
    }

   
}
