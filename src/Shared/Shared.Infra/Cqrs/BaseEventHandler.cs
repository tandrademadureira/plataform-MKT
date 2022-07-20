using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infra.Cqrs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task Handle(TEvent notification, CancellationToken cancellationToken);
    }
}
