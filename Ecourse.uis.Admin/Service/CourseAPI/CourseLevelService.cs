using ECourse.Admin.Models.CourseAPI.CourseLevel;
using ECourse.Admin.Utility;

namespace ECourse.Admin.Service.CourseAPI
{
    public class CourseLevelService : BaseService<CourseLevelDto>, ICourseLevelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public CourseLevelService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : base(httpClientFactory, tokenProvider)
        {
            //SD.CourseAPIBase = WebApplication.
            _apiUrl = SD.CourseAPIBase + "/api/CourseLevel";
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
    }
}
