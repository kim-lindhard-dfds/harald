using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.Tests.TestDoubles
{
    public class SlackFacadeSpy : ISlackFacade
    {
        public List<UserGroup> UserGroups;

        public SlackFacadeSpy()
        {
            UserGroups = new List<UserGroup>();
        }

        public Task<SendNotificationResponse> SendNotificationToChannel(ChannelId channelId, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendDelayedNotificationToChannel(ChannelId channelId, string message,
            long delayTimeInEpoch)
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task<GeneralResponse> PinMessageToChannel(ChannelId channelId, string messageTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateChannelResponse> CreateChannel(ChannelName channelName)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteChannel(ChannelId channelId, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameChannel(ChannelId channelId, ChannelName name)
        {
            throw new System.NotImplementedException();
        }

        public readonly Dictionary<ChannelId, List<string>> InvitedToChannel = new Dictionary<ChannelId, List<string>>();

        public Task InviteToChannel(string email, ChannelId channelId)
        {
            if (InvitedToChannel.ContainsKey(channelId) == false)
            {
                InvitedToChannel.Add(channelId, new List<string>());
            }

            InvitedToChannel[channelId].Add(email);

            return Task.CompletedTask;
        }

        public Task RemoveFromChannel(string email, ChannelId channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateUserGroupResponse> CreateUserGroup(string name, UserGroupHandle handle, string description)
        {
            CreateUserGroupWasCalled = true;
            CreateUserGroupName = name;
            CreateUserGroupHandle = handle;
            CreateUserGroupDescription = description;

            return Task.FromResult(new CreateUserGroupResponse());
        }

        public Task RenameUserGroup(string usergroupId, string name, UserGroupHandle handle)
        {
            throw new System.NotImplementedException();
        }

        public string CreateUserGroupDescription { get; private set; }

        public string CreateUserGroupHandle { get; private set; }

        public string CreateUserGroupName { get; private set; }
        public readonly Dictionary<string, List<string>> UserGroupsUsers = new Dictionary<string, List<string>>();

        public Task AddUserGroupUser(string userGroupId, string email)
        {
            if (UserGroupsUsers.ContainsKey(userGroupId) == false)
            {
                UserGroupsUsers.Add(userGroupId, new List<string>());
            }

            UserGroupsUsers[userGroupId].Add(email);

            return Task.CompletedTask;
        }

        public Task RemoveUserGroupUser(string userGroupId, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UserGroup>> GetUserGroups()
        {
            return Task.FromResult(UserGroups);
        }

        public Task<GetConversationsResponse> GetConversations()
        {
            throw new System.NotImplementedException();
        }

        public bool CreateUserGroupWasCalled { get; private set; }
    }
}