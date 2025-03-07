using Infrastructure;
using Microsoft.Extensions.Options;

namespace ECourse.Services.CourseAPI
{
    public class ApplicationDbContext : BaseDbContext
    {

        public ApplicationDbContext(IOptions<MongoDbSettings> settings) : base(settings) { }
    }
}
