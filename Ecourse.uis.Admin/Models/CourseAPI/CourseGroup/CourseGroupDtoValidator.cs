using FluentValidation;

namespace ECourse.Admin.Models.CourseAPI.CourseGroup
{
    public class CourseGroupDtoValidator : AbstractValidator<CourseGroupDto>
    {
        public CourseGroupDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required").MaximumLength(200);
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CustomCode).MaximumLength(20);           
        }
    }
}
