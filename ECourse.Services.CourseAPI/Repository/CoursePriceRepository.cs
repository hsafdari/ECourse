using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CoursePriceRepository : BaseRepository<CoursePrice>, ICoursePriceRepository
    {
        public CoursePriceRepository(IMongoDatabase db, string documentname) : base(db, documentname)
        {
        }
    }
}
