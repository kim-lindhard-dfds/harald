using Harald.Infrastructure.Slack.Dto;
using Harald.Infrastructure.Slack.Exceptions;
using Harald.Infrastructure.Slack.Http.Request.Channel;
using Harald.Infrastructure.Slack.Http.Request.Conversation;
using Harald.Infrastructure.Slack.Http.Request.Notification;
using Harald.Infrastructure.Slack.Http.Request.User;
using Harald.Infrastructure.Slack.Http.Request.UserGroup;
using Harald.Infrastructure.Slack.Http.Response;
using Harald.Infrastructure.Slack.Http.Response.Conversation;
using Harald.Infrastructure.Slack.Http.Response.Notification;
using Harald.Infrastructure.Slack.Http.Response.User;
using Harald.Infrastructure.Slack.Http.Response.UserGroup;
using Harald.Infrastructure.Slack.Model;
using Harald.Infrastructure.Slack.Http.Response.Channel;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Harald.Infrastructure.Slack
{
    public class SlackFacade : ISlackFacade
    {
        private readonly HttpClient _client;
        private readonly IDistributedCache _cache;
        
        public SlackFacade(HttpClient client = null, IDistributedCache cache = null)
        {
            _client = client ?? new HttpClient() { BaseAddress = new System.Uri("https://slack.com", System.UriKind.Absolute) };
            _cache = cache;
        }

        public async Task<CreateChannelResponse> CreateChannel(SlackChannelName channelName)
        {
            using (var response = await _client.SendAsync(new CreateChannelRequest(channelName)))
            { 
                return await Parse<CreateChannelResponse>(response);
            }
        }

        // This uses an undocumented API, tread carefully.
        public async Task DeleteChannel(SlackChannelIdentifier channelIdentifier, string token)
        {
            using (var response = await _client.SendAsync(new DeleteChannelRequest(channelIdentifier, token)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task LeaveChannel(SlackChannelIdentifier channelIdentifier)
        {
            using (var response = await _client.SendAsync(new LeaveChannelRequest(channelIdentifier)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task ArchiveChannel(SlackChannelIdentifier channelIdentifier)
        {
            using (var response = await _client.SendAsync(new ArchiveChannelRequest(channelIdentifier)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task RenameChannel(SlackChannelIdentifier channelIdentifier, SlackChannelName channelName)
        {
            using (var response = await _client.SendAsync(new RenameChannelRequest(channelIdentifier, channelName)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task<JoinChannelResponse> JoinChannel(SlackChannelName channelName, bool validate = false)
        {
            using (var response = await _client.SendAsync(new JoinChannelRequest(channelName, validate)))
            {
                return await Parse<JoinChannelResponse>(response);
            }
        }

        public async Task<IEnumerable<ChannelDto>> GetChannels(string token)
        {
            using (var response = await _client.SendAsync(new ListChannelsRequest(token)))
            {
                var result = await Parse<ListChannelsResponse>(response);

                return result.Channels;
            }
        }

        public async Task<SendNotificationResponse> SendNotificationToChannel(SlackChannelIdentifier channelIdentifier, string message)
        {
            using (var response = await _client.SendAsync(new SendNotificationRequest(channelIdentifier, message)))
            {
                return await Parse<SendNotificationResponse>(response);
            }
        }

        public async Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            var userId = await GetUserId(email);

            return await SendNotificationToChannel(userId, message);
        }

        public async Task<SendNotificationResponse> SendDelayedNotificationToChannel(SlackChannelIdentifier channelIdentifier, string message, long delayTimeInEpoch)
        {
            using (var response = await _client.SendAsync(new SendDelayedNotificationRequest(channelIdentifier, message, delayTimeInEpoch)))
            {
                return await Parse<SendNotificationResponse>(response);
            }
        }

        public async Task<SlackResponse> PinMessageToChannel(SlackChannelIdentifier channelIdentifier, string messageTimeStamp)
        {
            using (var response = await _client.SendAsync(new PinMessageToChannelRequest(channelIdentifier, messageTimeStamp)))
            {
                return await Parse<SlackResponse>(response);
            }
        }

        public async Task InviteToChannel(string email, SlackChannelIdentifier channelIdentifier)
        {
            var userId = await GetUserId(email);

            using (var response = await _client.SendAsync(new InviteToChannelRequest(channelIdentifier, userId)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task RemoveFromChannel(string email, SlackChannelIdentifier channelIdentifier)
        {
            var userId = await GetUserId(email);
            
            using (var response = await _client.SendAsync(new RemoveFromChannelRequest(channelIdentifier, userId)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            using (var response = await _client.SendAsync(new CreateUserGroupRequest(name, handle, description)))
            {
                return await Parse<CreateUserGroupResponse>(response);
            }
        }

        public async Task RenameUserGroup(string usergroupId, string name, string handle)
        {
            using (var response = await _client.SendAsync(new UpdateUserGroupRequest(usergroupId, name, handle)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task AddUserGroupUser(string usergroupId, string name, string handle)
        {
            using (var response = await _client.SendAsync(new UpdateUserGroupRequest(usergroupId, name, handle)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        public async Task AddUserGroupUser(string userGroupId, string email)
        {
            var users = await GetUserGroupUsers(userGroupId);
            var userId = await GetUserId(email);

            users.Add(userId);

            await UpdateUserGroupUsers(userGroupId, users);
        }

        public async Task RemoveUserGroupUser(string userGroupId, string email)
        {
            var users = await GetUserGroupUsers(userGroupId);
            var userId = await GetUserId(email);

            users.Remove(userId);

            await UpdateUserGroupUsers(userGroupId, users);
        }

        public async Task<GetConversationsResponse> GetConversations()
        {
            using (var response = await _client.SendAsync(new GetConversationsRequest()))
            {
                return await Parse<GetConversationsResponse>(response);
            }
        }

        public async Task<IEnumerable<UserGroupDto>> GetUserGroups()
        {
            using (var response = await _client.SendAsync(new GetUserGroupsRequest()))
            {
                var data = await Parse<ListUserGroupsResponse>(response);

                return data.UserGroups;
            }
        }
        private async Task UpdateUserGroupUsers(string userGroupId, List<string> users)
        {
            using (var response = await _client.SendAsync(new UpdateUserGroupUsersRequest(userGroupId, users)))
            {
                await Parse<SlackResponse>(response);
            }
        }

        private async Task<List<string>> GetUserGroupUsers(string userGroupId)
        {
            using (var response = await _client.SendAsync(new GetUserGroupUsersRequest(userGroupId)))
            {
                var data = await Parse<ListUsersInUserGroupResponse>(response);

                return data.Users;
            }
        }

        private async Task<string> GetUserId(string email)
        {
            var userId = await _cache.GetStringAsync(email);

            if (userId == string.Empty)
            {
                using (var response = await _client.SendAsync(new GetUserByEmailRequest(email)))
                {
                    var lookup = await Parse<LookupUserResponse>(response);

                    userId = lookup?.User.Id;

                    if(!string.IsNullOrEmpty(userId))
                    {
                        await _cache.SetStringAsync(email, userId);
                    }
                }
            }

            return userId;
        }

        private async Task<T> Parse<T>(HttpResponseMessage response) where T : SlackResponse
        {
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            switch (data)
            {
                case T msg when msg.Ok == true:
                    return data;
                case T msg when msg.Error == "channel_not_found":
                    throw new ChannelNotFoundException($"API error: {content}");
                case T msg when msg.Error == "not_authed":
                    throw new NotAuthorizedException($"API error: {content}");
                default:
                    throw new SlackFacadeException($"API error: {content}");
            }
        }
    }
}