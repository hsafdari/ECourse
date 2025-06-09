using Bogus;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    public class CourseGroupDataGenerator
    {
        Faker<CourseGroup> _CourseGroupFaker;
        Faker<CourseGroupDto> _CourseGroupDtoFaker;
        public CourseGroupDataGenerator()
        {
            Randomizer.Seed = new Random(123);
            _CourseGroupFaker = new Faker<CourseGroup>()
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.CustomCode, f => f.Lorem.Word());
            _CourseGroupDtoFaker = new Faker<CourseGroupDto>()
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.CustomCode, f => f.Lorem.Word());
        }
        public CourseGroup GetCourseGroup()
        {
            return _CourseGroupFaker.Generate();
        }

        internal IEnumerable<CourseGroup> GetCourseGroups(int row=10)
        {
            return _CourseGroupFaker.Generate(row);
        }  
        public CourseGroupDto GetCourseGroupDto()
        {
            return _CourseGroupDtoFaker.Generate();
        }
        public CourseGroupDto GetCourseGroupDtoInvalidTitle()
        {
            return _CourseGroupDtoFaker.RuleFor(x=>x.Title,f=>f.Lorem.Sentence(4000)).Generate();
        }        
    }
}
