using Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECourse.Services.CourseAPI.Models
{
    public class CourseGroup:BaseEntity
    {
        public static readonly string DocumentName = nameof(CourseGroup);

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentId { get; set; }
        public required string Title { get; set; }
        public string? CustomCode { get; set; }       
        public bool HasChildren { get; set; }
    }
}
