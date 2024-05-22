using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Middleware.Models
{
    public abstract class BaseEntity
    {
        [BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public DateTime CreateDateTime { get; set; }= DateTime.Now;
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;   

    }
}
