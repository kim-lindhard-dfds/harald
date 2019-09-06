using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public interface ISlackFacade
    {
        Task<SendNotificationResponse> SendNotificationToChannel(string channel, string message);
        Task<SendNotificationResponse> SendDelayedNotificationToChannel(string channel, string message, long delayTimeInEpoch);
        Task<SendNotificationResponse> SendNotificationToUser(string email, string message);
        Task<GeneralResponse> PinMessageToChannel(string channel, string messageTimeStamp);
        Task<CreateChannelResponse> CreateChannel(string channelName);
        Task DeleteChannel(string channelId, string token);
        Task RenameChannel(string channelId, string name);
        Task InviteToChannel(string email, string channelId);
        Task RemoveFromChannel(string email, string channelId);
        Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description);
        Task RenameUserGroup(string id, string name, string handle);
        Task AddUserGroupUser(string userGroupId, string email);
        Task RemoveUserGroupUser(string userGroupId, string email);
        Task<List<UserGroup>> GetUserGroups();
        Task<GetConversationsResponse> GetConversations();
    }
}