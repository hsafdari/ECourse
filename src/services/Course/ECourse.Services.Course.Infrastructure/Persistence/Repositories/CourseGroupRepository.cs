using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.Course.Infrastructure.Persistence.Repositories
{
    internal class CourseGroupRepository : ICourseGroupRepository
    {
        private readonly CourseDbContext _dbContext;
        public CourseGroupRepository(CourseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(CourseGroup courseGroup)
        {
            _dbContext.CourseGroups.Add(courseGroup);
        }

        public async Task DeleteAsync(Guid courseGroupId, CancellationToken cancellationToken = default)
        {
            var courseGroup = await _dbContext.CourseGroups
                                    .FirstOrDefaultAsync(cg => cg.Id == courseGroupId, cancellationToken);
            if (courseGroup != null)
            {
                courseGroup.MarkAsDeleted(DateTimeOffset.UtcNow);
            }
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return _dbContext.CourseGroups
                .AsNoTracking()
                .AnyAsync(cg => cg.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CourseGroups
                .AsNoTracking()
                .AnyAsync(cg => cg.Slug == slug, cancellationToken);
        }

        public async Task<IReadOnlyList<CourseGroup>> GetAllChildrenOfTreeAsync(Guid parentId)
        {
            return await _dbContext.CourseGroups
                .AsNoTracking()
                .Where(cg => cg.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CourseGroup>> GetAllTreeAsync()
        {
            return await _dbContext.CourseGroups
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CourseGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CourseGroups
                .FirstOrDefaultAsync(cg => cg.Id == id, cancellationToken);
        }
        public async Task<bool> HasChildGroupsAsync(Guid courseGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CourseGroups
                .AsNoTracking()
                .AnyAsync(cg => cg.ParentId == courseGroupId, cancellationToken);
        }
    }
}
