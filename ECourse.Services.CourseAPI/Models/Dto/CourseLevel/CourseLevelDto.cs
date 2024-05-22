using Microsoft.AspNetCore.Components.Forms;
using Middleware.Models;

namespace ECourse.Services.CourseAPI.Models.Dto.CourseLevel
{
    public class CourseLevelDto:BaseDto
    {
        public string Title { get; set; } = default!;
        public string Icon { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string FileLocation { get; set; } = default!;
    }
}
