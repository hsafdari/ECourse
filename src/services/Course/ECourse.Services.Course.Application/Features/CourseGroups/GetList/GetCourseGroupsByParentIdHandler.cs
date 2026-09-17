using ECourse.Services.Course.Application.Abstractions.CQRS;
using ECourse.Services.Course.Application.Common.Exceptions;

namespace ECourse.Services.Course.Application.Features.CourseGroups.GetList;

internal class GetCourseGroupsByParentIdHandler : IQueryHandler<GetCourseGroupsByParentIdQuery, IReadOnlyList<CourseGroupTreeResponse>>
{
    private readonly IGetCouseGroupsByParentIdReadService _couseGroupsByParentIdReadService;

    public GetCourseGroupsByParentIdHandler(IGetCouseGroupsByParentIdReadService couseGroupsByParentIdReadService)
    {
        _couseGroupsByParentIdReadService = couseGroupsByParentIdReadService;
    }

    public async Task<IReadOnlyList<CourseGroupTreeResponse>> Handle(GetCourseGroupsByParentIdQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<CourseGroupTreeResponse>? courseGroups =await _couseGroupsByParentIdReadService.GetCouseGroupsByParentIdAsync(request.ParentId,cancellationToken);
        
        return courseGroups ?? throw new NotFoundException("CourseGroup", request.ParentId.Value);
    }
}
