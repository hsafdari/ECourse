using ECourse.Services.CourseAPI.Models;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{
    public class CourseItemRepository : BaseRepository<CourseItem>, ICourseItemRepository
    {
        readonly DataContext _dbContext;
        public CourseItemRepository(DataContext db, string documentname) : base(db, documentname)
        {
            _dbContext = db;
        }
    }
}
