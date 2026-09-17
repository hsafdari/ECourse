using MediatR;

namespace ECourse.Services.Course.Application.Abstractions.CQRS
{
    public interface ICommand : IRequest
    {
    }
    public interface ICommand<out TResponse>:IRequest<TResponse> {
    
    }
}
