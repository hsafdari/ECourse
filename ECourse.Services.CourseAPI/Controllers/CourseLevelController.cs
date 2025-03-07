using AutoMapper;
using ECourse.Services.CourseAPI.Interfaces;
using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ECourse.Services.CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLevelController : ControllerBase
    {
        private readonly ICourseLevelRepository _courseLevelRepository;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public CourseLevelController(ICourseLevelRepository courseLevelRepository, IMapper mapper)
        {
            _courseLevelRepository = courseLevelRepository;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ResponseDto> List()
        {
            try
            {
                List<CourseLevel> model = await _courseLevelRepository.GetAllAsync();
                List<CourseLevelDto> result = _mapper.Map<List<CourseLevel>, List<CourseLevelDto>>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_ListItems");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                CourseLevel model = await _courseLevelRepository.GetByIdAsync(id);
                CourseLevelDto result = _mapper.Map<CourseLevel, CourseLevelDto>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_One Row Item");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("grid")]
        public async Task<ResponseDto> GetGrid([FromQuery] GridQuery query)
        {
            if (!ModelState.IsValid)
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                if (query != null)
                {
                    (List<CourseLevel>, long) model = await _courseLevelRepository.GetGridDataAsync(query);
                    if (model.Item1 == null)
                    {
                        throw new Exception();
                    }
                    List<CourseLevelDto> result = _mapper.Map<List<CourseLevel>, List<CourseLevelDto>>(model.Item1);
                    _responseDto = _responseDto.ResultGridSuccessful(result, "Grid Successful Result", model.Item2);
                }
                else
                {
                    _responseDto = _responseDto.ResultErrorBadRequest("Bad Request");
                }

            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }

            return _responseDto;
        }
        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] CourseLevelDto dto)
        {
            if (!ModelState.IsValid)
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                CourseLevel model = _mapper.Map<CourseLevelDto, CourseLevel>(dto);
                CourseLevel result = await _courseLevelRepository.AddAsync(model);
                if (result != null)
                {
                    _responseDto = _responseDto.ResultSuccessful(result, "create Item was successfully!!");
                }
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] CourseLevelDto dto)
        {
            if (!ModelState.IsValid)
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                CourseLevel model = _mapper.Map<CourseLevelDto, CourseLevel>(dto);
                MongoDB.Driver.ReplaceOneResult result = await _courseLevelRepository.UpdateAsync(model);
                _responseDto = result.IsAcknowledged
                    ? _responseDto.ResultSuccessful(result, "update Item was successfully!!")
                    : _responseDto.ResultErrorException("Updated not completed");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<ResponseDto> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                MongoDB.Driver.UpdateResult result = await _courseLevelRepository.MarkAsDeletedAsync(id);
                _responseDto = result.IsAcknowledged
                    ? _responseDto.ResultSuccessful(result, "Delete Item was successfully!!")
                    : _responseDto.ResultErrorException("Delete did not complete!!");
            }
            catch (Exception ex)
            {

                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpDelete]
        [Route("deletemany")]
        public async Task<ResponseDto> DeleteMany([FromQuery] IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                MongoDB.Driver.UpdateResult result = await _courseLevelRepository.MarkAsDeletedAsync(ids);
                _responseDto = result.IsAcknowledged
                    ? _responseDto.ResultSuccessful(result, "Items removed successfully!!") 
                    : _responseDto = _responseDto.ResultErrorException("remove items not completed!!");
            }
            catch (Exception ex)
            {

                _responseDto = _responseDto.ResultErrorException(ex.Message);

            }
            return _responseDto;
        }
    }
}
