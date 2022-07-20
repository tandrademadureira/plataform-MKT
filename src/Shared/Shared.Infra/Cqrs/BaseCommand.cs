using MediatR;
using Shared.Infra.Request;
using System;

namespace Shared.Infra.Cqrs
{
    /// <summary>
    /// Abstract Class BaseCommand
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    [Serializable]
    public abstract class BaseCommand<TResponse> : BaseRequest, IRequest<TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Authenticated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] Roles { get; set; }
    }
}
