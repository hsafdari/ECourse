using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseRepository : BaseRepository<Course, ApplicationDataContext>, ICourseRepository
    {
        public CourseRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }

        public Task<int> RateInsertAsync(ObjectId Id)
        {
            throw new NotImplementedException();
        }
    }
}
