using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Enums
{
    public enum CourseType
    {
        [BsonRepresentation(BsonType.String)]
        video = 0,
        [BsonRepresentation(BsonType.String)]
        article = 1,
        [BsonRepresentation(BsonType.String)]
        content = 2
    }
}
