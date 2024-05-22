using ECourse.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ECourse.Admin.Models.CourseAPI.CourseLevel
{
    public class CourseLevelDto : BaseDto
    {
        public string Title { get; set; } = default!;
        public string Icon { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string FileLocation { get; set; } = default!;
    }
}
