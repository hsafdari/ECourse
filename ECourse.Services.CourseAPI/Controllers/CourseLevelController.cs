using AutoMapper;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto.CourseLevel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Infrastructure.Models;
using Infrastructure.Utility;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLevelController : ControllerBase
    {
        private readonly ICourseLevelRepository _courseLevelRepository;
        private IMapper _mapper;
        private ResponseDto _responseDto;

        public CourseLevelController(ICourseLevelRepository courseLevelRepository, IMapper mapper)
        {
            _courseLevelRepository = courseLevelRepository;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]        
        public async Task<ResponseDto> Get()
        {
            try
            {
                _responseDto.Result = await _courseLevelRepository.GetMany(x => x.IsDeleted == false);
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
        public async Task<ResponseDto> Get(ObjectId id)
        {
            try
            {
                _responseDto.Result = await _courseLevelRepository.GetById(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }       
        [HttpGet]
        [Route("grid")]       
        public async Task<ResponseDto> GetGrid([FromQuery] GridQuery query)
        {
            try
            {          
                if (query!=null)
                {
                    _responseDto.Result = await _courseLevelRepository.Grid(query);
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Bad Request";
                }

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
        [HttpPost]
        public async Task<ResponseDto> Post([FromForm] CourseLevelDto dto)
        {
            try
            {
                var model = _mapper.Map<CourseLevelDto, CourseLevel>(dto);
                _responseDto.Result = await _courseLevelRepository.Create(model);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPut]
        public async Task<ResponseDto> Put(CourseLevelDto dto)
        {
            try
            {
                var model = _mapper.Map<CourseLevelDto, CourseLevel>(dto);
                //var _update = Builders<CourseLevel>.Update.Set(x => x.Title, dto.Title)
                //                                        .Set(x => x.ModifiedDateTime, DateTime.Now)
                //                                        .Set(x => x.Icon, dto.Icon);
               // _responseDto.Result = await _courseLevelRepository.Update(x => x.Id == dto.Id, _update);
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
