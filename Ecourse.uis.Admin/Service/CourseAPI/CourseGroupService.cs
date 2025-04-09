using ECourse.Admin.Models;
using ECourse.Admin.Models.CourseAPI.CourseGroup;
using ECourse.Admin.Utility;

namespace ECourse.Admin.Service.CourseAPI
{
    public class CourseGroupService : BaseService<CourseGroupDto>, ICourseGroupService
    {
        public CourseGroupService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : base(httpClientFactory, tokenProvider)
        {
            //set api url based on each service
            ApiUrl = SD.CourseAPIBase + "/api/CourseGroup";
        }

        public async  Task<ResponseDto?> GetChildren(string ParentId)
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl + $"/children/{ParentId}"
            });
        }

        public async Task<ResponseDto?> GetRoot()
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl + $"/root"
            });
        }
        public async Task<ResponseDto?> UpdateParent(string ParentId,string SourceId)
        {
            return await SendAsync(new RequestDto()
            {
                Url = ApiUrl + $"/updateparent/{ParentId}/{SourceId}"
            });
        }

    }
}
