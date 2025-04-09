using ECourse.Admin.Models;
using ECourse.Admin.Models.CourseAPI.CourseGroup;

namespace ECourse.Admin.Service.CourseAPI
{
    public interface ICourseGroupService : IBaseService<CourseGroupDto>
    {
        public Task<ResponseDto?> GetRoot();
        public Task<ResponseDto?> GetChildren(string ParentId);
        public Task<ResponseDto?> UpdateParent(string ParentId, string SourceId);
    }
}
