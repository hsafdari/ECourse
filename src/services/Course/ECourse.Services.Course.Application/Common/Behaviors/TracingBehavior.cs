using System.Diagnostics;
using MediatR;

namespace ECourse.Services.Course.Application.Common.Behaviors;

public sealed class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = Observability.CourseTelemetry.ActivitySource.StartActivity(typeof(TRequest).Name);
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            activity?.SetStatus(
                ActivityStatusCode.Error,
                ex.Message);
            throw;
        }
    }
}
