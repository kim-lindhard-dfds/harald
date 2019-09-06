using System.Collections.Generic;
using System.Threading.Tasks;
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
        public Task<SendNotificationResponse> SendNotificationToChannel(string channel, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendDelayedNotificationToChannel(string channel, string message, long delayTimeInEpoch)
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task<GeneralResponse> PinMessageToChannel(string channel, string messageTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateChannelResponse> CreateChannel(string channelName)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteChannel(string channelId, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameChannel(string channelId, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task InviteToChannel(string email, string channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromChannel(string email, string channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            CreateUserGroupWasCalled = true;
            CreateUserGroupName = name;
            CreateUserGroupHandle = handle;
            CreateUserGroupDescription = description;

            return Task.FromResult(new CreateUserGroupResponse());
        }

        public Task RenameUserGroup(string id, string name, string handle)
        {
            throw new System.NotImplementedException();
        }

        public string CreateUserGroupDescription { get; private set; }

        public string CreateUserGroupHandle { get; private set; }

        public string CreateUserGroupName { get; private set; }

        public Task AddUserGroupUser(string userGroupId, string email)
        {
            throw new System.NotImplementedException();
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