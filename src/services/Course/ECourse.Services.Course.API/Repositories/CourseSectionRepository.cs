using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;


namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseSectionRepository : BaseRepository<CourseSection, ApplicationDataContext>, ICourseSectionRepository
    {
        public CourseSectionRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
