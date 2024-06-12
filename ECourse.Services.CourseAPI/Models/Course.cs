using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class Course:BaseEntity
    {
        public static readonly string DocumentName = nameof(Course);
        public required string Name { get; set; }
        public required string Title { get; set; }
        /// <summary>
        /// due to seo friendly admin can put customised url
        /// </summary>
        public required string UrlLink { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// the total calculated percentage like of buyer
        /// </summary>
        public float RatePercent { get; set; } = 0;
        /// <summary>
        /// number of buyers who rate this course
        /// </summary>
        public int RateCount { get; set; } = 0;
        //bind to Teacher 
        public List<CourseTeacher> Teachers { get;set; }
        public required ObjectId LevelId { get; set; }
        public List<CourseSection> Sections { get; init; } = null!;
        public List<CoursePrice> Prices { get; init; } = null!;
        

    }
}
