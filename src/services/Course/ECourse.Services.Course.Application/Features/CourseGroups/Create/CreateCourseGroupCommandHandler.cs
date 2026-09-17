using ECourse.Services.Course.Application.Abstractions.CQRS;
using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Common.Exceptions;
using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Create
{
    internal sealed class CreateCourseGroupCommandHandler : ICommandHandler<CreateCourseGroupCommand, Guid>
    {
        private readonly ICourseGroupRepository _courseGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;
        public CreateCourseGroupCommandHandler(ICourseGroupRepository courseGroupRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
        {
            _courseGroupRepository = courseGroupRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }
        public async Task<Guid> Handle(CreateCourseGroupCommand request, CancellationToken cancellationToken)
        {
            Slug slug = Slug.Create(request.Slug);
            bool isSlugExists= await _courseGroupRepository.ExistsBySlugAsync(slug, cancellationToken);
            if (isSlugExists)
            {
                throw new ConflictException($"A Course Group with the {slug.Value} slug already exists.");
            }
            //check if parentId exists
            if (request.ParentId.HasValue)
            {
                bool isParentExists = await _courseGroupRepository.ExistsByIdAsync(request.ParentId.Value, cancellationToken);
                if (!isParentExists)
                {
                    throw new NotFoundException($"Parent Course Group with Id {request.ParentId.Value} not found.");
                }
            }
            CourseGroupDetails details = CourseGroupDetails.Create(request.Title, slug.Value, request.Description,request.ParentId,request.IsActive);
            DateTimeOffset utcNow=_timeProvider.GetUtcNow();

            CourseGroup courseGroup = CourseGroup.Create(details, utcNow);

            _courseGroupRepository.Add(courseGroup);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return courseGroup.Id;
        }
    }
}
