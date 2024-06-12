using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class TeacherRepository : BaseRepository<CourseTeacher, ApplicationDataContext>, ITeacherRepository
    {
        public TeacherRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
