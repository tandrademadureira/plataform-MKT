using MediatR;
using Shared.Infra.Cqrs;
using Shared.Util.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plataform.MKT.Application.Commands.Queue
{
    public class SendQueueCommand
    {
        public class SendQueueContract : BaseCommand<Result>
        {
            public string Description { get; set; }
            public string Mark { get; set; }
            public bool ProductApproved { get; set; }
            public DateTimeOffset RequestDate { get; set; }
        }

        public class Handler : BaseHandler<SendQueueContract, Result>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<Result> Handle(SendQueueContract request, CancellationToken cancellationToken)
            {

                //Enviar para a fila


                return Result.Ok();
            }
        }
    }
}
