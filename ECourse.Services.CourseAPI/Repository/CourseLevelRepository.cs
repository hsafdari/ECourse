using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CourseLevelRepository : BaseRepository<CourseLevel>, ICourseLevelRepository
    {
        public CourseLevelRepository(IMongoDatabase db, string documentname) : base(db, documentname)
        {
        }
    }
}
