using System.Threading.Tasks;

namespace Harald.WebApi.Application.EventHandlers
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T domainEvent);
    }
}