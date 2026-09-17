using MediatR;

namespace ECourse.Services.Course.Application.Abstractions.CQRS
{
    public interface IQuery<out TResponse>:IRequest<TResponse>
    {
    }
}
