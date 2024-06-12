using Infrastructure.Models;

namespace ECourse.Services.CourseAPI.Models.Dto
{
    public class SocialLinkDto:BaseDto
    {
        public string Name { get; set; }
        public required string Icon { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileLocation { get; set; } = string.Empty;
    }
}
