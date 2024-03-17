namespace Bdv.Cqrs.Interfaces
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}