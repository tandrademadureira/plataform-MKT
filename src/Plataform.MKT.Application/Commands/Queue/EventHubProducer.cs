using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using Plataform.MKT.Infra.Integrations.EventHub;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Plataform.MKT.Application.Commands.Queue
{
    public class EventHubProducer : IEventHubProducer
    {
        private readonly EventHubProducerClient _eventHubProducerClient;
        public EventHubProducer(string connectionString, string name)
        {
            _eventHubProducerClient = new EventHubProducerClient(connectionString, name);
        }

        public async Task SendAsync(object obj)
        {
            using EventDataBatch eventBatch = await _eventHubProducerClient.CreateBatchAsync();
            var eventObject = JsonConvert.SerializeObject(obj, Formatting.Indented);

            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventObject))))
            {
                throw new Exception($"Event {eventObject} is too large for the batch and cannot be sent.");
            }

            await _eventHubProducerClient.SendAsync(eventBatch);
            await _eventHubProducerClient.DisposeAsync();
        }
    }
}
