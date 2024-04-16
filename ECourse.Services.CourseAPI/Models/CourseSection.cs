using Middleware.Models;
namespace ECourse.Services.CourseAPI.Models
{
    public class CourseSection : BaseEntity
    {        
        public required string CourseId { get; set; }
        public required string Title { get; set; }
        public List<CourseItem> CourseItems = null!;      
    }
}
