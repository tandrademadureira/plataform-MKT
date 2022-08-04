using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Plataform.MKT.Infra.Integrations.EventHub;
using Shared.Infra.Cqrs;
using Shared.Util.Result;
using System;
using System.Text;
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
            public bool Approved { get; set; }
            public int Amount { get; set; }
            public DateTimeOffset? DataRequisition { get; set; }
        }

        public class Handler : BaseHandler<SendQueueContract, Result>
        {
            private readonly IConfiguration _configuration;
            private IEventHubProducer _eventHubProducer;
            public Handler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<Result> Handle(SendQueueContract request, CancellationToken cancellationToken)
            {
                var eventHubConnectionString = _configuration["EventHubConnectionString"];
                var eventHubName = _configuration["EventHubName"];

                _eventHubProducer = new EventHubProducer(eventHubConnectionString, eventHubName);
                await _eventHubProducer.SendAsync(request);

                return Result.Ok();
            }
        }
    }
}
