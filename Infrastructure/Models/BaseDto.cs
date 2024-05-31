using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class BaseDto
    {
        public ObjectId? Id { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
