using ECourse.Services.Course.Application.Abstractions.CQRS;
using ECourse.Services.Course.Application.Common.Exceptions;

namespace ECourse.Services.Course.Application.Features.CourseGroups.GetById
{
    internal class GetCourseGroupByIdQueryHandler : IQueryHandler<GetCourseGroupByIdQuery, CourseGroupDetailsResponse>
    {
        private readonly IGetCourseGroupByIdReadService _readService;

        public GetCourseGroupByIdQueryHandler(IGetCourseGroupByIdReadService readService)
        {
            _readService = readService;
        }
        public async Task<CourseGroupDetailsResponse> Handle(GetCourseGroupByIdQuery query, CancellationToken cancellationToken)
        {
            var courseGroup = await _readService.GetyIdAsync(query.Id, cancellationToken);

            return courseGroup
                ?? throw new NotFoundException("CourseGroup", query.Id);
        }
    }
}
