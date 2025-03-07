using ECourse.Services.AuthAPI.Interfaces;
using ECourse.Services.AuthAPI.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.AuthAPI.Repositories
{
    public class SocialLinkRepository : BaseRepository<SocialLink, ApplicationDataContext>, ISocialLinkRepository
    {
        public SocialLinkRepository(IDbContextFactory<ApplicationDataContext> datacontext) : base(datacontext)
        {
        }
    }
}
