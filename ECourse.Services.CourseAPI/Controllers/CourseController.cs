using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        [HttpGet]
        public async Task<List<Course>> Get()
        {
            return await _courseRepository.GetMany(x => x.IsDeleted == false);
            
        }
        [HttpGet("{id}")]
        public async Task<Course> Get(ObjectId id)
        {
            return await _courseRepository.GetById(x => x.Id == id && x.IsDeleted == false);
        }
    }
}
