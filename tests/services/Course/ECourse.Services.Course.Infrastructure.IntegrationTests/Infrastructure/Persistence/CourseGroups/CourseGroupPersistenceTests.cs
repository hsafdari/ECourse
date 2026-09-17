using ECourse.Services.Course.Domain.Entities;
using ECourse.Services.Course.Domain.ValueObjects;
using ECourse.Services.Course.Infrastructure.Persistence;
using ECourse.Services.Course.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECourse.Services.Course.IntegrationTests.Infrastructure.Persistence.CourseGroups
{
    public sealed class CourseGroupPersistenceTests : IClassFixture<PostgreSqlFixture>
    {
        private readonly PostgreSqlFixture _fixture;

        public CourseGroupPersistenceTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }
        private static CourseGroup CreateCourseGroup(string title, string slug)
        {

            string normalizedSlug = Slug.Create(slug).ToString();
            CourseGroupDetails details = CourseGroupDetails.Create(title, normalizedSlug, null, null, true);

            return CourseGroup.Create(details, new DateTimeOffset(2026, 8, 18, 12, 0, 0, TimeSpan.Zero));
        }
        [Fact]
        public async Task SaveAndLoad_ShouldPreserveSlugValueObject()
        {
            // Arrange
            string uniqueSlug = $"test-course-group-{Guid.NewGuid():N}";
            CourseGroup courseGroup = CreateCourseGroup("Test Course Group", uniqueSlug);

            await using (CourseDbContext arrangeContext = _fixture.CreateDbContext())
            {
                arrangeContext.CourseGroups.Add(courseGroup);
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            await using CourseDbContext assertContext = _fixture.CreateDbContext();

            CourseGroup? stored = await assertContext.CourseGroups.SingleOrDefaultAsync(item => item.Id == courseGroup.Id);

            // Assert
            Assert.NotNull(stored);
            Assert.Equal(uniqueSlug, stored.Slug.Value);
        }
        [Fact]
        public async Task Repository_shouldFindPersistedCourseGroup()
        {
            // Arrange
            string uniqueSlug = $"test-course-group-{Guid.NewGuid():N}";
            CourseGroup courseGroup = CreateCourseGroup("Test Course Group", uniqueSlug);
            await using CourseDbContext dbContext = _fixture.CreateDbContext();

            CourseGroupRepository repository = new(dbContext);

            repository.Add(courseGroup);
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear(); // Clear the change tracker to ensure we fetch a fresh instance from the database

            // Act
            CourseGroup? result = await repository.GetByIdAsync(courseGroup.Id, CancellationToken.None);
            Assert.NotNull(result);
            // Assert
            Assert.Equal(uniqueSlug, result.Slug.Value);
        }
        [Fact]
        public async Task Save_WhenSlugIsDuplicated_ShouldThrowException()
        {
            //arrange
            string url =$"test-course-group-{Guid.NewGuid():N}";

            string validUrl = Slug.Create(url).Value;

            var courseGroupFirst = CreateCourseGroup("Test Course Group", validUrl);
            var courseGroupSecond = CreateCourseGroup("Test Course Group Second", validUrl);
            //act
            await using (CourseDbContext DbContextfirst = _fixture.CreateDbContext())
            {
                DbContextfirst.CourseGroups.Add(courseGroupFirst);
                await DbContextfirst.SaveChangesAsync();
            }
            await using CourseDbContext DbContextSecond = _fixture.CreateDbContext();
            
                DbContextSecond.CourseGroups.Add(courseGroupSecond);

            //assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await DbContextSecond.SaveChangesAsync());

        }
        [Fact]
        public async Task Query_WhenCourseGroupIsDeleted_ShouldHideIt()
        {
            //arrange
            string url = $"test-course-group-{Guid.NewGuid():N}";

            var courseGroup= CreateCourseGroup("Test Course Group", url);
            courseGroup.Deleted(new DateTimeOffset(2026, 8, 18, 12, 0, 0, TimeSpan.Zero));
            //act
            using (CourseDbContext DbContext=_fixture.CreateDbContext())
            {
                DbContext.CourseGroups.Add(courseGroup);
                await DbContext.SaveChangesAsync();
            }
            using CourseDbContext DbContextSecond = _fixture.CreateDbContext();
            var ResultFilter= await DbContextSecond.CourseGroups.FirstOrDefaultAsync(item => item.Id == courseGroup.Id);


            //assert
            using CourseDbContext DbContextThird = _fixture.CreateDbContext();
            var ResultIgnoreFilter=await DbContextThird.CourseGroups.IgnoreQueryFilters().FirstOrDefaultAsync(item => item.Id == courseGroup.Id);


            Assert.Null(ResultFilter);       
            Assert.NotNull(ResultIgnoreFilter);
            Assert.True(ResultIgnoreFilter.IsDeleted);
            Assert.True(ResultIgnoreFilter.DeletedAtUtc.HasValue);


        }
    }
}
