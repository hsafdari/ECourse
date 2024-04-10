using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class CourseItem:BaseEntity
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
        /// <summary>
        /// the length of video or length of reading file or content
        /// </summary>
        public required string Timeduration { get; set; }
        public required string CourseId { get; set; }
        public required string SectionId { get; set; }
        public string? Description { get; set; }
        public CourseType CourseType { get; set; }     
    }

    public enum CourseType
    {
        [BsonRepresentation(BsonType.String)]
        video =0,
        [BsonRepresentation(BsonType.String)]
        article =1,
        [BsonRepresentation(BsonType.String)]
        content =2
    }
}
