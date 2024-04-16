using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CourseItemRepository : BaseRepository<CourseItem>, ICourseItemRepository
    {
        public CourseItemRepository(IMongoDatabase db, string documentname) : base(db, documentname)
        {
        }
    }
}
