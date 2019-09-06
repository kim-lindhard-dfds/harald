using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public interface ISlackFacade
    {
        Task<SendNotificationResponse> SendNotificationToChannel(ChannelId channelId, string message);
        Task<SendNotificationResponse> SendDelayedNotificationToChannel(ChannelId channelId, string message, long delayTimeInEpoch);
        Task<SendNotificationResponse> SendNotificationToUser(string email, string message);
        Task<GeneralResponse> PinMessageToChannel(ChannelId channelId, string messageTimeStamp);
        Task<CreateChannelResponse> CreateChannel(ChannelName channelName);
        Task DeleteChannel(ChannelId channelId, string token);
        Task RenameChannel(ChannelId channelId, ChannelName name);
        Task InviteToChannel(string email, ChannelId channelId);
        Task RemoveFromChannel(string email, ChannelId channelId);
        Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description);
        Task RenameUserGroup(string id, string name, string handle);
        Task AddUserGroupUser(string userGroupId, string email);
        Task RemoveUserGroupUser(string userGroupId, string email);
        Task<List<UserGroup>> GetUserGroups();
        Task<GetConversationsResponse> GetConversations();
    }
}