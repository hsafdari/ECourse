using ECourse.Services.Course.Application.Features.CourseGroups.Create;
using ECourse.Services.Course.Domain.Rules;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECourse.Services.Course.Application.UnitTests.Features.CourseGroups.Create
{
    public sealed class CreateCourseGroupCommandValidatorTests
    {
        private readonly CreateCourseGroupCommandValidator _validator=new();
        [Fact]
        public async Task Validate_WhenCommandIsValid_ShouldHaveNoErrors()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: "test-course-group",
                Description: "This is a test course group description",
                null,
                true
            );
            // Act
            TestValidationResult<CreateCourseGroupCommand> result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact]
        public async Task Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "",
                Slug: "test-course-group",
                Description: "This is a test course group description",
                null,
                true
            );
            // Act
            TestValidationResult<CreateCourseGroupCommand> result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }
        [Fact]
        public async Task Validate_WhenSlugIsEmpty_ShouldHaveSlugError()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: string.Empty,
                null,
                null,
                true
            );
            // Act
            TestValidationResult<CreateCourseGroupCommand> result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Slug);
        }
        [Fact]
        public async Task Validate_WhenTitleExceedsMaximumLength_ShouldHaveError()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: new string('a', CourseGroupRules.MaxTitleLength + 1),
                Slug: "test-course-group",
                Description: "This is a test course group description",
                null,
                true
            );
            // Act
            TestValidationResult<CreateCourseGroupCommand> result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Title);
        }
        [Fact]
        public async Task Validate_WhenSlugExceedsMaximumLength_ShouldHaveError()
        {
            // Arrange
            CreateCourseGroupCommand command = new(
                Title: "Test Course Group",
                Slug: new string('a', CourseGroupRules.MaxSlugLength + 1),
                Description: "This is a test course group description",
                null,
                true
            );
            // Act
            TestValidationResult<CreateCourseGroupCommand> result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Slug);
        }
    }
}
