using Infrastructure.Models;
namespace ECourse.Services.CourseAPI.Models
{

    public class CourseSection : BaseEntity
    {
        public static readonly string DocumentName = nameof(CourseSection);
        /// <summary>
        /// each course has multiple sections for example part 1, part 2, ...
        /// </summary>
        public required string CourseId { get; set; }
        /// <summary>
        /// each section has title
        /// </summary>
        public required string Title { get; set; }
        public List<CourseItem> CourseItems = null!;      
    }
}
