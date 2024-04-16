using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(IMongoDatabase db, string documentname) : base(db, documentname)
        {            
        }
    }
}
