using ECourse.Services.CourseAPI.Models.Dto;
using FluentValidation;

namespace ECourse.Services.CourseAPI.Models.DtoValidators
{
    public class CourseLevelDtoValidator : AbstractValidator<CourseLevelDto>
    {
        public CourseLevelDtoValidator()
        {            
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required").MaximumLength(200);
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x=>x.FileLocation).NotEmpty().WithMessage("FileLocation is required").MaximumLength(1000);
            RuleFor(x => x.FileName).NotEmpty().WithMessage("FileName is required").MaximumLength(200);
            RuleFor(x => x.Icon).NotEmpty().WithMessage("Icon is required").MaximumLength(200);
        }
    }
}
