using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class CourseItemRepository : BaseRepository<CourseItem, ApplicationDataContext>, ICourseItemRepository
    {
        public CourseItemRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
