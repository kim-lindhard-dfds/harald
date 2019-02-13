using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public interface IEventDispatcher
    {
        Task Send(string generalDomainEventJson);
        Task SendAsync(GeneralDomainEvent generalDomainEvent);
    }
    
}