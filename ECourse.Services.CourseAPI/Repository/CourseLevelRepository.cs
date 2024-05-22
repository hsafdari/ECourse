using ECourse.Services.CourseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Middleware.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Repository
{   
    public class CourseLevelRepository : BaseRepository<CourseLevel,DataContext>, ICourseLevelRepository
    {
        private readonly IDbContextFactory<DataContext> _datacontext;
        public CourseLevelRepository(IDbContextFactory<DataContext> datacontext) : base(datacontext)
        {
            _datacontext = datacontext;
        }
    }
}
