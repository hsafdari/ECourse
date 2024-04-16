using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CourseSectionRepository : BaseRepository<CourseSection>, ICourseSectionRepository
    {
        public CourseSectionRepository(IMongoDatabase db, string documentname) : base(db, documentname)
        {
        }
    }
}
