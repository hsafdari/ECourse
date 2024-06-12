using AutoMapper;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;
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
        private IMapper _mapper;
        private ResponseDto _responseDto;

        public CourseController(ICourseRepository courseLevelRepository, IMapper mapper)
        {
            _courseRepository = courseLevelRepository;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var model = await _courseRepository.GetMany();
                _responseDto.Result = _mapper.Map<List<Course>, List<CourseDto>>(model);
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
                var model = await _courseRepository.GetById(x => x.Id == ObjectId.Parse(id));
                _responseDto.Result = _mapper.Map<Course, CourseDto>(model);
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
                if (query != null)
                {
                    var model = await _courseRepository.Grid(query);
                    if (model.Item1 == null)
                    {
                        throw new Exception();
                    }
                    _responseDto.Count = model.Item2;
                    _responseDto.Result = _mapper.Map<List<Course>, List<CourseDto>>(model.Item1);
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
        public async Task<ResponseDto> Post([FromForm] CourseDto dto)
        {
            try
            {
                var model = _mapper.Map<CourseDto, Course>(dto);
                var result = await _courseRepository.Create(model);
                if (result > 0)
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "create Item was successfully!!";
                    _responseDto.Result = result;
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPut]
        public async Task<ResponseDto> Put([FromForm] CourseDto dto)
        {
            try
            {
                var model = _mapper.Map<CourseDto, Course>(dto);
                var result = await _courseRepository.Update(model);
                if (result > 0)
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "update Item was successfully!!";
                    _responseDto.Result = result;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<ResponseDto> Delete(string Id)
        {
            try
            {
                var result = await _courseRepository.Delete(x => x.Id == ObjectId.Parse(Id));
                if (result > 0)
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Delete Item was successfully!!";
                    _responseDto.Result = result;
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpDelete]
        [Route("deletemany")]
        public async Task<ResponseDto> DeleteMany([FromQuery] IEnumerable<string> ids)
        {
            try
            {
                if (ids == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "badRequest";
                    _responseDto.Result = null;
                    return _responseDto;
                }
                List<ObjectId> _ids = ids.Select(x => ObjectId.Parse(x)).ToList();
                var result = await _courseRepository.DeleteMany(x => _ids.Any(y => x.Id.Equals(y)));
                if (result > 0)
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Delete Items was successfully!!";
                    _responseDto.Result = result;
                }
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Delete Items was successfully!!";
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPut]
        [Route("Rate")]
        public async Task<int> RateInsert(string Id)
        {
            return await _courseRepository.RateInsertAsync(ObjectId.Parse(Id));
        }
    }
}
