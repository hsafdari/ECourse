using Infrastructure.Models;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class CourseGroupDto: BaseDto
    {
        public string? ParentId { get; set; }
        public required string Title { get; set; }
        public string? CustomCode { get; set; }
        public bool HasChildren { get; set; }       
    }
}
