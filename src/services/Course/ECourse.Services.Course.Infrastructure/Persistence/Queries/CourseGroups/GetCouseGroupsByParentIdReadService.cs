using ECourse.Services.Course.Application.Features.CourseGroups.GetList;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.Course.Infrastructure.Persistence.Queries.CourseGroups
{
    public class GetCouseGroupsByParentIdReadService : IGetCouseGroupsByParentIdReadService
    {
        private readonly CourseDbContext _DbContext;

        public GetCouseGroupsByParentIdReadService(CourseDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IReadOnlyList<CourseGroupTreeResponse>?> GetCouseGroupsByParentIdAsync(Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await _DbContext.CourseGroups.AsNoTracking()
                 .Where(courseGroup => courseGroup.ParentId == parentId)
                .Select(courseGroup => new CourseGroupTreeResponse
                (
                    courseGroup.Id,
                    courseGroup.Title,
                    courseGroup.Slug.Value,
                    courseGroup.Description,
                    courseGroup.IsActive,
                    courseGroup.ParentId,
                    _DbContext.CourseGroups.Any(child => child.ParentId == courseGroup.Id),
                    courseGroup.CreatedAtUtc,
                    courseGroup.ModifiedAtUtc
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
