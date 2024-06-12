using AutoMapper;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;

namespace ECourse.Services.CourseAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CourseLevelDto, CourseLevel>().ReverseMap();
                config.CreateMap<CourseDto,Course>().ReverseMap();
                config.CreateMap<CoursePriceDto, CoursePrice>().ReverseMap();
                config.CreateMap<CourseSectionDto, CourseSection>().ReverseMap();
                config.CreateMap<CourseItemDto, CourseItem>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
