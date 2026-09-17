using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ECourse.Services.Course.Application.Common.Behaviors
{
    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            ValidationContext<TRequest> context = new(request);

            ValidationResult[] validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            ValidationFailure[] failures = validationResults.SelectMany(result => result.Errors).Where(failure => failure is not null).ToArray();

            if (failures.Length > 0)
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
