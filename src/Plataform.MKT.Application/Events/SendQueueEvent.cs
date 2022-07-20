using MediatR;
using Shared.Infra.Cqrs;
using System.Threading;
using System.Threading.Tasks;
using Plataform.MKT.Application.Commands.Queue;
using System;

namespace Plataform.MKT.Application.Events
{
    public class SendQueueEvent : BaseNotification
    {
        public string Description { get; set; }
        public string Mark { get; set; }
        public bool ProductApproved { get; set; }
        public DateTimeOffset RequestDate { get; set; }

        public SendQueueEvent(string description, string mark)
        {
            Description = description;
            Mark = mark;
            ProductApproved = false;
            RequestDate = DateTimeOffset.Now;
        }
    }

    public class SendQueueHandle : BaseEventHandler<SendQueueEvent>
    {
        public IMediator _mediator { get; set; }

        public SendQueueHandle(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Handle(SendQueueEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SendQueueCommand.SendQueueContract()
            {
                Description = notification.Description,
                Mark = notification.Mark,
                ProductApproved = notification.ProductApproved,
                RequestDate = notification.RequestDate
            });
        }
    }
}
