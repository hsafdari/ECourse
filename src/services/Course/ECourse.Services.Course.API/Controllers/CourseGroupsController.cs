using ECourse.Services.Course.API.Contracts.CourseGroups.Requests;
using ECourse.Services.Course.API.Contracts.CourseGroups.Responses;
using ECourse.Services.Course.Application.Features.CourseGroups.Create;
using ECourse.Services.Course.Application.Features.CourseGroups.Delete;
using ECourse.Services.Course.Application.Features.CourseGroups.GetById;
using ECourse.Services.Course.Application.Features.CourseGroups.GetList;
using ECourse.Services.Course.Application.Features.CourseGroups.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECourse.Services.Course.API.Controllers;

[ApiController]
[Route("api/course-groups")]
public sealed class CourseGroupsController : ControllerBase
{
    private readonly ISender _sender;

    public CourseGroupsController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CourseGroupDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseGroupDetailsResponse>> GetByIdAsync(Guid id, CancellationToken cancellation)
    {
        GetCourseGroupByIdQuery query = new(id);
        CourseGroupDetailsResponse courseGroup = await _sender.Send(query, cancellation);

        return Ok(courseGroup);
    }
    [HttpPost]
    [ProducesResponseType(typeof(CreateCourseGroupResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseGroupDetailsResponse>> CreateAsync([FromBody] CreateCourseGroupRequest request, CancellationToken cancellationToken)
    {
        CreateCourseGroupCommand command = new(request.Title, request.Slug, request.Description, request.ParentId, request.IsActive);

        Guid courseGroupId = await _sender.Send(command, cancellationToken);

       var query= new GetCourseGroupByIdQuery(courseGroupId);
       CourseGroupDetailsResponse response = await _sender.Send(query, cancellationToken);

       return CreatedAtAction("GetById", new { id = response.Id }, response);
   
    }
    [HttpGet("tree")]
    [ProducesResponseType(typeof(CourseGroupTreeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<CourseGroupTreeResponse>>> GetTreeAsync(Guid? parentId =null, CancellationToken cancellationToken=default)
    {
        GetCourseGroupsByParentIdQuery query = new(parentId);
        IReadOnlyList<CourseGroupTreeResponse> courseGroups = await _sender.Send(query, cancellationToken);

        return Ok(courseGroups);
    }
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EditAsync(Guid id, [FromBody] UpdateCourseGroupRequest request, CancellationToken cancellationToken)
    {
        EditCourseGroupCommand command = new(id, request.Title, request.Slug, request.Description, request.ParentId, request.IsActive);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        DeleteCourseGroupCommand command = new(id);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}
