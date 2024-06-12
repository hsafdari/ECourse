using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    public class CourseLevelData
    {
        public CourseLevel FakeGetSingleRow()
        {
            var entity = new CourseLevel
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Title = "Beginner1",
                FileLocation = "/Uploads/Test/file.jpg",
                Icon = "/Uploads/test1.jpg",
                CreateDateTime = DateTime.Now
            };
            return entity;
        }
        public CourseLevelDto FakeGetSingleRowDTO()
        {
            var entity = new CourseLevelDto
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                Title = "Beginner1",
                FileLocation = "/Uploads/Test/file.jpg",
                Icon = "/Uploads/test1.jpg",
                CreateDateTime = DateTime.Now
            };
            return entity;
        }
    }
}
