using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.Infrastructure.Services
{
    public interface ISlackService
    {
        Task<UserGroup> EnsureUserGroupExists(string capabilityName);
    }
}