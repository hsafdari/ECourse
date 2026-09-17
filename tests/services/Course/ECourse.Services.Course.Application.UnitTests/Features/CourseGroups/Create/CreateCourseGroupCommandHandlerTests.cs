using ECourse.Services.Course.Application.Abstractions.Persistence;
using ECourse.Services.Course.Application.Common.Exceptions;
using ECourse.Services.Course.Application.Features.CourseGroups.Create;
using ECourse.Services.Course.Application.UnitTests.Common;
using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;
using NSubstitute;

namespace ECourse.Services.Course.Application.UnitTests.Features.CourseGroups.Create
{
    public sealed class CreateCourseGroupCommandHandlerTests
    {
        private readonly ICourseGroupRepository _courseGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FakeTimeProvider _timeProvider;
        private readonly CreateCourseGroupCommandHandler _handler;
        public CreateCourseGroupCommandHandlerTests()
        {
            _courseGroupRepository = Substitute.For<ICourseGroupRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _timeProvider = new FakeTimeProvider(new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 0, 0, 0, TimeSpan.Zero));
            _handler = new CreateCourseGroupCommandHandler(_courseGroupRepository, _unitOfWork, _timeProvider);
        }
        [Fact]
        public async Task Handle_WhenSlugIsUnique_ShouldCreateCourseGroup()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: "test-course-group",
                Description: "This is a test course group description", null, true
            );
            _courseGroupRepository
                .ExistsBySlugAsync(
                    Arg.Any<Slug>(),
                    Arg.Any<CancellationToken>())
                .Returns(false);

            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _courseGroupRepository.Received(1)
                .Add(
                Arg.Is<Domain.Entities.CourseGroup>(
                    courseGroup =>
                    courseGroup.Id == result &&
                    courseGroup.Title == command.Title &&
                    courseGroup.Slug.Value == command.Slug &&
                    courseGroup.Description == command.Description
                ));
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        }
        [Fact]
        public async Task Handle_WhenSlugAlreadyExists_ShouldThrowException()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: "test-course-group",
                Description: "This is a test course group description", null, true
            );
            _courseGroupRepository
                .ExistsBySlugAsync(
                    Arg.Is<Slug>(slug => slug.Value == command.Slug),
                    Arg.Any<CancellationToken>())
                .Returns(true);
            // Act
            Task Act() => _handler.Handle(command, CancellationToken.None);
            //Assert
            ConflictException exception = await Assert.ThrowsAsync<ConflictException>(Act);
            Assert.Contains(command.Slug, exception.Message);
            _courseGroupRepository.DidNotReceive().Add(Arg.Any<Domain.Entities.CourseGroup>());

            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_ShouldCheckUniquenessUsingNormalizedSlug()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: "Test-Course-Group",
                Description: "This is a test course group description",
                null,
                true
            );
            _courseGroupRepository
                .ExistsBySlugAsync(
                    Arg.Any<Slug>(),
                    Arg.Any<CancellationToken>())
                .Returns(false);
            // Act
            await _handler.Handle(command, CancellationToken.None);

            //Assert
            await _courseGroupRepository.Received(1)
                .ExistsBySlugAsync(
                    Arg.Is<Slug>(slug => slug.Value == command.Slug.ToLowerInvariant()),
                    Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_WhenAddChildtoParent_shouldbeCreated()
        {
            //arrage
            CreateCourseGroupCommand commandOne = new(
                Title: "Software",
              Slug: "software",
              Description: "des",
              ParentId: null,
              IsActive: true
                );
            _courseGroupRepository.Add(Arg.Any<CourseGroup>());
            //Act
            await _handler.Handle(commandOne, CancellationToken.None);
            
            //Assert
            
        }
    }
}
