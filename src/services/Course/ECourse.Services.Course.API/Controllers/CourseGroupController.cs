using AutoMapper;
using ECourse.Services.Course.API.Contracts;
using ECourse.Services.Course.API.Interfaces;
using ECourse.Services.Course.API.Models;
using ECourse.Services.Course.API.Models.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ECourse.Services.Course.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseGroupController : ControllerBase
    {
        private readonly ICourseGroupRepository _courseGroupRepository;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public CourseGroupController(ICourseGroupRepository CourseGroupRepository, IMapper mapper)
        {
            _courseGroupRepository = CourseGroupRepository;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ResponseDto> List()
        {
            try
            {
                List<CourseGroup> model = await _courseGroupRepository.GetAllAsync();
                List<CourseGroupDto> result = _mapper.Map<List<CourseGroup>, List<CourseGroupDto>>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_ListItems");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("root")]
        public async Task<ResponseDto> RootList()
        {
            try
            {
                List<CourseGroup> model = await _courseGroupRepository.GetAllTreeAsync();
               
                List<CourseGroupDto> result = _mapper.Map<List<CourseGroup>, List<CourseGroupDto>>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_ListTreeItems");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("children/{id}")]
        public async Task<ResponseDto> ChildrenListOfRoot(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                List<CourseGroup> model = await _courseGroupRepository.GetAllChildrenOfTreeAsync(id);
                List<CourseGroupDto> result = _mapper.Map<List<CourseGroup>, List<CourseGroupDto>>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_One Row Item");
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
                CourseGroup model = await _courseGroupRepository.GetByIdAsync(id);
                CourseGroupDto result = _mapper.Map<CourseGroup, CourseGroupDto>(model);
                _responseDto = _responseDto.ResultSuccessful(result, "SuccessfulOf_One Row Item");
            }
            catch (Exception ex)
            {
                _responseDto = _responseDto.ResultErrorException(ex.Message);
            }
            return _responseDto;
        }       
        [HttpGet("grid")]
        public async Task<ActionResult<ResponseDto>> GetGrid([FromQuery] CourseGroupGridRequest query,CancellationToken cancellationToken)
        {           
            PagedResult<CourseGroup> page = await _courseGroupRepository.GetGridDataAsync(query, cancellationToken);
            if (page.Items == null)
            {
                throw new Exception();
            }
            List<CourseGroupDto> items = _mapper.Map<List<CourseGroupDto>>(page.Items);

            return _responseDto.ResultGridSuccessful(items, "Grid retrieved successfully.", page.TotalCount);
        }
        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] CourseGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                CourseGroup model = _mapper.Map<CourseGroupDto, CourseGroup>(dto);
                CourseGroup result = await _courseGroupRepository.AddAsync(model);
                if (result != null)
                {
                    if (result.ParentId != null)
                    {
                       var parentUpdated= await _courseGroupRepository.SetNodeHasChildren(result.ParentId);
                        if (parentUpdated == null)
                        {
                            throw new Exception("Parent Node not updated");
                        }
                    }
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
        public async Task<ResponseDto> Put([FromBody] CourseGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                CourseGroup model = _mapper.Map<CourseGroupDto, CourseGroup>(dto);
                MongoDB.Driver.ReplaceOneResult result = await _courseGroupRepository.UpdateAsync(model);
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
        [HttpGet]
        [Route("updateparent/{parentId}/{sourceId}")]   
        public async Task<ResponseDto> UpdateRoot(string parentId, string sourceId)
        {
            if (string.IsNullOrEmpty(parentId) || string.IsNullOrEmpty(sourceId))
            {
                return _responseDto.ResultErrorBadRequest("badRequest");
            }
            try
            {
                var result = await _courseGroupRepository.UpdateParentAsync(parentId, sourceId);
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
                MongoDB.Driver.UpdateResult result = await _courseGroupRepository.MarkAsDeletedAsync(id);
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
                MongoDB.Driver.UpdateResult result = await _courseGroupRepository.MarkAsDeletedAsync(ids);
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
