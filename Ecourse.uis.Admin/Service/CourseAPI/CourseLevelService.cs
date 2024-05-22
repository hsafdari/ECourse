using ECourse.Admin.Models.CourseAPI.CourseLevel;
using ECourse.Admin.Utility;
using Radzen;

namespace ECourse.Admin.Service.CourseAPI
{
    public class CourseLevelService : BaseService<CourseLevelDto>, ICourseLevelService
    {       
        public CourseLevelService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : base(httpClientFactory, tokenProvider)
        {
            //set api url based on each service
            ApiUrl = SD.CourseAPIBase + "/api/CourseLevel";          
        }       
    }
}
