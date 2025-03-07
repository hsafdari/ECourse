using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseTeacherRepository : BaseRepository<CourseTeacher, ApplicationDataContext>, ICourseTeacherRepository
    {
        public CourseTeacherRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
