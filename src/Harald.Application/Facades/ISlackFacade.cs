using System.Threading.Tasks;

namespace Harald.Application.Facades
{
    public interface ISlackFacade
    {
        Task SendNotification(string recipient, string message);
        Task CreateChannel(string channelName);
    }
}