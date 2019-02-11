using System.Threading.Tasks;

namespace Harald.Application
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T domainEvent);
    }
}
