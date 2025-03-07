using Bogus;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    public class CourseLevelDataGenerator
    {
        Faker<CourseLevel> _courseLevelFaker;
        Faker<CourseLevelDto> _courseLevelDtoFaker;
        public CourseLevelDataGenerator()
        {
            Randomizer.Seed = new Random(123);
            _courseLevelFaker = new Faker<CourseLevel>()                
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.Icon, f => f.Image.PlaceImgUrl())
                .RuleFor(x => x.FileName, f =>f.Image.Random.Word())
                .RuleFor(x => x.FileLocation, f => f.Image.PlaceImgUrl());
            _courseLevelDtoFaker = new Faker<CourseLevelDto>()               
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.Icon, f => f.Image.PlaceImgUrl())
                .RuleFor(x => x.FileName, f => f.Image.Random.Word())
                .RuleFor(x => x.FileLocation, f => f.Image.PlaceImgUrl());
        }
        public CourseLevel GetCourseLevel()
        {
            return _courseLevelFaker.Generate();
        }

        internal IEnumerable<CourseLevel> GetCourseLevels(int row=10)
        {
            return _courseLevelFaker.Generate(row);
        }  
        public CourseLevelDto GetCourseLevelDto()
        {
            return _courseLevelDtoFaker.Generate();
        }
        public CourseLevelDto GetCourseLevelDtoInvalidTitle()
        {
            return _courseLevelDtoFaker.RuleFor(x=>x.Title,f=>f.Lorem.Sentence(4000)).Generate();
        }        
    }
}
