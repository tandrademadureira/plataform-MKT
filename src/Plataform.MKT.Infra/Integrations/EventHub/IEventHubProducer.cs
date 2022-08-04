using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plataform.MKT.Infra.Integrations.EventHub
{
    public interface IEventHubProducer
    {
        Task SendAsync(object obj);
    }
}
