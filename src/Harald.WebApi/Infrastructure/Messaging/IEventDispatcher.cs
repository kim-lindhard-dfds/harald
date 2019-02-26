using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public interface IEventDispatcher
    {
        Task Send(string generalDomainEventJson, IServiceScope serviceScope);
        Task SendAsync(GeneralDomainEvent generalDomainEvent, IServiceScope serviceScope);
    }
    
}