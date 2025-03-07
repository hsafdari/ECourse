using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICourseLevelRepository _dataContext;
        public TestController(ICourseLevelRepository dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<List<CourseLevel>> Index()
        {
            return await _dataContext.GetAllAsync();
        }
        //[HttpPost]
        //public async Task post(int id)
        //{

        //   await _dataContext.InsertOneAsync(new CourseLevel()
        //    {
        //        Icon = $"test{id}",
        //        Title = $"test{id}",
        //        //CreateDateTime = DateTime.Now,
        //        FileName = $"test{id}",
        //        FileLocation = $"test{id}"
        //    });
        //}
        //[HttpGet]
        //[Route("Index")]
        //public CourseLevel Index(string id)
        //{
        //    var item = _dbContext.CourseLevels.FirstOrDefault(x => x.Id == ObjectId.Parse(id));
        //    return item;
        //    //return new CourseLevel();
        //}
    }
}
