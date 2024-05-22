using AutoMapper;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto.CourseLevel;

namespace ECourse.Services.CourseAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CourseLevelDto, CourseLevel>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
