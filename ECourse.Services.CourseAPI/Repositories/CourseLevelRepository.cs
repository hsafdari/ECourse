using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseLevelRepository : BaseRepository<CourseLevel>, ICourseLevelRepository
    {
        public CourseLevelRepository(ApplicationDbContext dbContext,ILogger<CourseLevel> logger) : base(dbContext._database, logger,CourseLevel.DocumentName)
        {
        }
    }    
}
