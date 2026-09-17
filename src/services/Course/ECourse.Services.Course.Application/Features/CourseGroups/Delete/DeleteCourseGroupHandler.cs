using ECourse.Services.Course.Application.Abstractions.CQRS;
using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Common.Exceptions;


namespace ECourse.Services.Course.Application.Features.CourseGroups.Delete;
internal class DeleteCourseGroupHandler : ICommandHandler<DeleteCourseGroupCommand>
{
    private readonly ICourseGroupRepository _courseGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseGroupHandler(ICourseGroupRepository courseGroupRepository, IUnitOfWork unitOfWork)
    {
        _courseGroupRepository = courseGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCourseGroupCommand request, CancellationToken cancellationToken)
    {
        var courseGroup = await _courseGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (courseGroup is null)
        {
            throw new NotFoundException("CourseGroup", request.Id);
        }
        //check if the course group has any child groups or courses associated with it before deleting
        var hasChildGroups = await _courseGroupRepository.HasChildGroupsAsync(request.Id, cancellationToken);
        if (hasChildGroups)
        {
            throw new ConflictException("Cannot delete course group with child groups.");
        }
        await _courseGroupRepository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
