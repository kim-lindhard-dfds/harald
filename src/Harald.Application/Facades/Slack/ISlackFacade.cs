using System.Threading.Tasks;

namespace Harald.Application.Facades.Slack
{
    public interface ISlackFacade
    {
        Task SendNotification(string recipient, string message);
        Task<CreateChannelResponse> CreateChannel(string channelName);
        Task InviteToChannel(string email, string channelId);
        Task RemoveFromChannel(string email, string channelId);
        Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description);
        Task AddUserGroupUser(string userGroupId, string email);
        Task RemoveUserGroupUser(string userGroupId, string email);
    }
}