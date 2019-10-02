using System.Threading.Tasks;
using Harald.Infrastructure.Slack.Dto;

namespace Harald.WebApi.Infrastructure.Services
{
    public interface ISlackService
    {
        Task<UserGroupDto> EnsureUserGroupExists(string capabilityName);
    }
}