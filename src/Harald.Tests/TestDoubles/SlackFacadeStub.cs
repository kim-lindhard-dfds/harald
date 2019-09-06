using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Services;

namespace Harald.Tests.TestDoubles
{
    public class SlackFacadeStub : ISlackFacade
    {
        private readonly bool _simulateFailOnSendMessage;

        public bool SendNotificationToChannelCalled { get; private set; } = false;

        public SlackFacadeStub(bool simulateFailOnSendMessage)
        {
            _simulateFailOnSendMessage = simulateFailOnSendMessage;
        }

        public Task RenameUserGroup(string id, string name, string handle)
        {
            throw new System.NotImplementedException();
        }

        public Task AddUserGroupUser(string userGroupId, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateChannelResponse> CreateChannel(string channelName)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateChannelResponse> CreateChannel(ChannelName channelName)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteChannel(string channelId, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameChannel(string channelId, ChannelName name)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameChannel(string channelId, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task InviteToChannel(string email, string channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<GeneralResponse> PinMessageToChannel(string channel, string messageTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromChannel(string email, string channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveUserGroupUser(string userGroupId, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UserGroup>> GetUserGroups()
        {
            throw new System.NotImplementedException();
        }
        
        public Task<GetConversationsResponse> GetConversations()
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendNotificationToChannel(string channel, string message)
        {
            SendNotificationToChannelCalled = true;
            if (_simulateFailOnSendMessage)
            {
                return Task.FromResult(new SendNotificationResponse
                {
                    Ok = false,
                    Error = "Simulated error sending notification."
                });
            }

            return Task.FromResult(new SendNotificationResponse
            {
                Ok = true,
                TimeStamp = "1234"
            });
        }

        public Task<SendNotificationResponse> SendDelayedNotificationToChannel(string channel, string message, long delayTimeInEpoch)
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            if (_simulateFailOnSendMessage)
            {
                return Task.FromResult(new SendNotificationResponse
                {
                    Ok = false,
                    Error = "Simulated error sending notification."
                });
            }

            return Task.FromResult(new SendNotificationResponse
            {
                Ok = true,
                TimeStamp = "1234"
            });
        }
    }
}