using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;

namespace ECourse.Services.Course.Domain.UnitTests.Entities
{
    public sealed class CourseGroupTests
    {
        [Fact]
        public void Create_WhenDetailsAreValid_ShouldCreateCourseGroup()
        {
            //Arrange
            var now = new DateTimeOffset(
                 2026, 8, 25,
                 12, 0, 0,
                 TimeSpan.Zero);
            var details = CourseGroupDetails.Create("Software", "software", "desc", null, true);
           
            //Act
            var courseGroup = CourseGroup.Create(details, now);
            //
            //Assert
            Assert.Equal("Software", courseGroup.Title);
            Assert.Equal("software", courseGroup.Slug.Value);
            Assert.Null(courseGroup.ParentId);
            Assert.True(courseGroup.IsActive);
            Assert.Equal(courseGroup.CreatedAtUtc, now);
            Assert.NotEqual(Guid.Empty, courseGroup.Id);

        }
        [Fact]
        public void Create_WhenParentIdIsProvided_ShouldSetParentId()
        {
            //Arrange
            var parentDetails = CourseGroupDetails.Create("IT", "it", "desc", null, true);
           
            var now = new DateTimeOffset(
                            2026, 8, 25,
                            12, 0, 0,
                            TimeSpan.Zero);
            
            var parent = CourseGroup.Create(parentDetails, now);
            var childDetails = CourseGroupDetails.Create("Software", "software", "desc", parent.Id, true);
            //Act

            var child = CourseGroup.Create(childDetails, now.AddMinutes(4));

            //Assert
            Assert.NotNull(child.ParentId);
            Assert.Equal(child.ParentId.Value, parent.Id);

        }
    }
}
