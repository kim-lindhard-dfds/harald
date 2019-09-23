using System.Collections.Generic;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Infrastructure.Services;
using Harald.Infrastructure.Slack.Dto;
using Harald.Infrastructure.Slack.Http.Response.Conversation;
using Harald.Infrastructure.Slack.Http.Response.Notification;
using Harald.Infrastructure.Slack.Http.Response.UserGroup;
using Harald.Infrastructure.Slack.Response.Channel;
using Harald.Infrastructure.Slack.Http.Response;
using Harald.Infrastructure.Slack.Model;

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

        public Task RenameUserGroup(string usergroupId, string name, string handle)
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

        public Task<SlackResponse> PinMessageToChannel(SlackChannelIdentifier channelId, string messageTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateChannelResponse> CreateChannel(SlackChannelName channelName)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteChannel(SlackChannelIdentifier channelId, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameChannel(SlackChannelIdentifier channelId, SlackChannelName name)
        {
            throw new System.NotImplementedException();
        }

        public Task InviteToChannel(string email, SlackChannelIdentifier channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromChannel(string email, SlackChannelIdentifier channelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveUserGroupUser(string userGroupId, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UserGroupDto>> GetUserGroups()
        {
            throw new System.NotImplementedException();
        }
        
        public Task<GetConversationsResponse> GetConversations()
        {
            throw new System.NotImplementedException();
        }

        public Task<SendNotificationResponse> SendNotificationToChannel(SlackChannelIdentifier channelId, string message)
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

        public Task<SendNotificationResponse> SendDelayedNotificationToChannel(SlackChannelIdentifier channelId, string message, long delayTimeInEpoch)
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

        public Task LeaveChannel(SlackChannelIdentifier channelIdentifier)
        {
            throw new System.NotImplementedException();
        }

        public Task ArchiveChannel(SlackChannelIdentifier channelIdentifier)
        {
            throw new System.NotImplementedException();
        }

        public Task<JoinChannelResponse> JoinChannel(SlackChannelName channelName, bool validate = false)
        {
            throw new System.NotImplementedException();
        }
    }
}