using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infra.Cqrs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Contract"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseHandler<Contract, TResponse> : IRequestHandler<Contract, TResponse>
        where Contract : BaseCommand<TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<TResponse> Handle(Contract request, CancellationToken cancellationToken);

    }
}
