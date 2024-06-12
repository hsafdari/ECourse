using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Interfaces
{
    public interface ICourseRepository : IBaseRepository<Course>
    {        
        Task<int> RateInsertAsync(ObjectId Id);
    }
}
