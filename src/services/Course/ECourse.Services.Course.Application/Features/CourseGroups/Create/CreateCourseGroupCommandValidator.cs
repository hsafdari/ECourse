using ECourse.Services.Course.Domain.Rules;
using FluentValidation;

namespace ECourse.Services.Course.Application.Features.CourseGroups.Create
{
    public class CreateCourseGroupCommandValidator:AbstractValidator<CreateCourseGroupCommand>
    {
        public CreateCourseGroupCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(CourseGroupRules.MaxTitleLength)
                .WithMessage($"Title cannot exceed {CourseGroupRules.MaxTitleLength} characters.");
            RuleFor(command=>command.Slug)
                .NotEmpty()
                .WithMessage("Slug is required.")
                .MaximumLength(CourseGroupRules.MaxSlugLength)
                .WithMessage($"Slug cannot exceed {CourseGroupRules.MaxSlugLength} characters.");
            RuleFor(x => x.Description)
                .MaximumLength(CourseGroupRules.MaxDescriptionLength).WithMessage($"Description cannot exceed {CourseGroupRules.MaxDescriptionLength} characters.");
            RuleFor(x => x.ParentId)
                .NotEqual(Guid.Empty)
                .When(x => x.ParentId.HasValue);

        }
    }
}
