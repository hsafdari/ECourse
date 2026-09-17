using ECourse.Services.Course.Application.Features.CourseGroups.GetById;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.Course.Infrastructure.Persistence.Queries.CourseGroups
{
    internal class GetCourseGroupByIdReadService : IGetCourseGroupByIdReadService
    {
        private readonly CourseDbContext _DbContext;

        public GetCourseGroupByIdReadService(CourseDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<CourseGroupDetailsResponse?> GetyIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _DbContext.CourseGroups.AsNoTracking()
                            .Where(courseGroup => courseGroup.Id == id)
                           .Select(courseGroup => new CourseGroupDetailsResponse
                           (
                               courseGroup.Id,
                               courseGroup.Title,
                               courseGroup.Slug.Value,
                               courseGroup.Description,
                               courseGroup.IsActive,
                               courseGroup.ParentId,
                               courseGroup.CreatedAtUtc,
                               courseGroup.ModifiedAtUtc
                           ))
                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
