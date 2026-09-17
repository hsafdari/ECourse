
using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;

namespace ECourse.Services.Course.Application.Abstractions.Persistence
{
    public interface ICourseGroupRepository
    {
        Task<IReadOnlyList<CourseGroup>> GetAllTreeAsync();
        Task<IReadOnlyList<CourseGroup>> GetAllChildrenOfTreeAsync(Guid parentId);
        Task<CourseGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Add(CourseGroup courseGroup);        
        Task<bool> ExistsBySlugAsync(Slug slug, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid courseGroupId, CancellationToken cancellationToken = default);
        Task<bool> HasChildGroupsAsync(Guid courseGroupId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
