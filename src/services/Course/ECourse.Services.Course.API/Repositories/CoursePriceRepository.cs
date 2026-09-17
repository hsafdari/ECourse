using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CoursePriceRepository : BaseRepository<CoursePrice, ApplicationDataContext>, ICoursePriceRepository
    {
        public CoursePriceRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
