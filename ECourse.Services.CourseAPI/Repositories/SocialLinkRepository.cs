using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.CourseAPI.Repositories
{
    public class SocialLinkRepository : BaseRepository<SocialLink, ApplicationDataContext>, ISocialLinkRepository
    {
        public SocialLinkRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
