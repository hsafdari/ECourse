using ECourse.Services.CourseAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middleware.Models;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLevelController : ControllerBase
    {
        private readonly ICourseLevelRepository _courseLevelRepository;
        private ResponseDto _responseDto;

        public CourseLevelController(ICourseLevelRepository courseLevelRepository)
        {
            _courseLevelRepository = courseLevelRepository;
            _responseDto = new ResponseDto();
        }
        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                _responseDto.Result=await _courseLevelRepository.GetMany(x=>x.IsDeleted==false);
            }
            catch (Exception ex)
            {               
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(string id)
        {
            try
            {
                _responseDto.Result = await _courseLevelRepository.GetById(x=>x.Id==id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
