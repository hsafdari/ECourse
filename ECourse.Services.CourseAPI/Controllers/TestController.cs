using ECourse.Services.CourseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using static System.Net.Mime.MediaTypeNames;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        readonly IDbContextFactory<ApplicationDataContext> _dbContext;
        public TestController(IDbContextFactory<ApplicationDataContext> dataContext)
        {
            _dbContext = dataContext;
        }
        [HttpGet]
        public async Task<List<CourseLevel>> Index()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                var items = await context.CourseLevels.ToListAsync();
                return items;
            }
               
        }
        //[HttpPost]
        //public async Task<List<CourseLevel>> post(int id)
        //{
        //    _dbContext.CourseLevels.Add(new CourseLevel()
        //    {
        //        Icon = $"test{id}",
        //        Title = $"test{id}",
        //        CreateDateTime = DateTime.Now,
        //        FileName = $"test{id}",
        //        FileLocation = $"test{id}"
        //    });
        //    _dbContext.SaveChanges();
        //    var items2 = await _dbContext.CourseLevels.ToListAsync();
        //    return items2;
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
