using ECourse.Services.CourseAPI.Models.Dto;
using FluentValidation;

namespace ECourse.Services.CourseAPI.Models.DtoValidators
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
