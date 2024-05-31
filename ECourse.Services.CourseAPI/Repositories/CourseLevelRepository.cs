using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseLevelRepository : BaseRepository<CourseLevel, ApplicationDataContext>, ICourseLevelRepository
    {
        public CourseLevelRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
