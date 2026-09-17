using ECourse.Services.Course.Application.Abstractions.CQRS;
using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Common.Exceptions;
using ECourse.Services.Course.Domain.ValueObjects;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Update;
internal sealed class EditCourseGroupCommandHandler : ICommandHandler<EditCourseGroupCommand>
{
    private readonly ICourseGroupRepository _courseGroupRepository;
    readonly IUnitOfWork _unitOfWork;
    public EditCourseGroupCommandHandler(ICourseGroupRepository courseGroupRepository, IUnitOfWork unitOfWork)
    {
        _courseGroupRepository = courseGroupRepository;
        _unitOfWork = unitOfWork;
    }    
    public async Task Handle(EditCourseGroupCommand request, CancellationToken cancellationToken)
    {
        var courseGroup= await _courseGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (courseGroup is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.CourseGroup), request.Id);
        }
        var CourseDetails = CourseGroupDetails.Create(request.Title, request.Slug, request.Description, request.ParentId, request.IsActive);

        courseGroup.UpdateDetails(CourseDetails,DateTimeOffset.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
