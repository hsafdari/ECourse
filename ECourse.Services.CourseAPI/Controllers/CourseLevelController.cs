using AutoMapper;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Infrastructure.Utility;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using ECourse.Services.CourseAPI.Models.Dto;

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
               var model = await _courseLevelRepository.GetMany();
                _responseDto.Result = _mapper.Map<List<CourseLevel>, List<CourseLevelDto>>(model);
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
               var model = await _courseLevelRepository.GetById(x => x.Id ==ObjectId.Parse(id));
                _responseDto.Result = _mapper.Map<CourseLevel, CourseLevelDto>(model);
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
                   var model = await _courseLevelRepository.Grid(query);
                    if (model.Item1==null)
                    {
                        throw new Exception();
                    }
                    _responseDto.Count = model.Item2;                                   
                    _responseDto.Result = _mapper.Map<List<CourseLevel>, List<CourseLevelDto>>(model.Item1);
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
                var result = await _courseLevelRepository.Create(model);
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
        public async Task<ResponseDto> Put([FromForm] CourseLevelDto dto)
        {
            try
            {
                var model = _mapper.Map<CourseLevelDto, CourseLevel>(dto);              
               var result = await _courseLevelRepository.Update(model);
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
                var result=await _courseLevelRepository.Delete(x=>x.Id==ObjectId.Parse(Id));
                if (result>0)
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Delete Item was successfully!!";                    
                    _responseDto.Result=result;
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
        public async Task<ResponseDto> DeleteMany([FromQuery]IEnumerable<string> ids)
        {
            try
            {
                if (ids==null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "badRequest";
                    _responseDto.Result = null;
                    return _responseDto;
                }             
                List<ObjectId> _ids = ids.Select(x => ObjectId.Parse(x)).ToList();
                var result = await _courseLevelRepository.DeleteMany(x => _ids.Any(y => x.Id.Equals(y)));
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
    }
}
