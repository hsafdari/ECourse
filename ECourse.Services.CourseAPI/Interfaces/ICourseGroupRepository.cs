using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;
using MongoDB.Driver;

namespace ECourse.Services.CourseAPI.Interfaces
{
    public interface ICourseGroupRepository : IBaseRepository<CourseGroup>
    {
        Task<List<CourseGroup>> GetAllTreeAsync();
        Task<List<CourseGroup>> GetAllChildrenOfTreeAsync(string parentId);
        Task<UpdateResult> SetNodeHasChildren(string parentId);
        Task<UpdateResult> UpdateParentAsync(string parentId, string sourceId);
    }
}
