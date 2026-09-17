using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Entities
{
    public class CourseTeacher:AggregateRoot<Guid>
    {
        //map with user Id
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
