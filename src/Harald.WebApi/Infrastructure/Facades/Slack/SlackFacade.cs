using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Serialization;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class SlackFacade : ISlackFacade
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;
        private readonly SlackHelper _slackHelper;

        public SlackFacade(HttpClient client, JsonSerializer serializer, SlackHelper slackHelper)
        {
            _client = client;
            _serializer = serializer;
            _slackHelper = slackHelper;
        }

        public async Task<CreateChannelResponse> CreateChannel(string channelName)
        {
            var validChannelName = _slackHelper.FixChannelNameForSlack(channelName);
            var payload = _serializer.GetPayload(new { Name = validChannelName, Validate = true });
            var response = await _client.PostAsync("/api/channels.create", payload);

            return await Parse<CreateChannelResponse>(response);
        }

        private async Task<T> Parse<T>(HttpResponseMessage response)  where T : GeneralResponse
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = _serializer.Deserialize<T>(content);
            if (!data.Ok)
            {
                throw new SlackFacadeException($"API error: {content}");
            }
            return data;
        }
        public async Task<SendNotificationResponse> SendNotificationToChannel(string channel, string message)
        {
            var payload = _serializer.GetPayload(new { Channel = channel, Text = message });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<SendNotificationResponse> SendDelayedNotificationToChannel(string channel, string message, long delayTimeInEpoch)
        {
            var payload = _serializer.GetPayload(new { Channel = channel, Text = message, post_at = delayTimeInEpoch });
            var response = await _client.PostAsync("/api/chat.scheduleMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = userId, Text = message, As_user = false });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<GeneralResponse> PinMessageToChannel(string channel, string messageTimeStamp)
        {
            var payload = _serializer.GetPayload(new { Channel = channel, Timestamp = messageTimeStamp });
            var response = await _client.PostAsync("/api/pins.add", payload);
            return await Parse<GeneralResponse>(response);
        }

        public async Task InviteToChannel(string email, string channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = channelId, user = userId });

            var response = await _client.PostAsync("/api/channels.invite", payload);
            await Parse<GeneralResponse>(response);
        }

        public async Task RemoveFromChannel(string email, string channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = channelId, user = userId });

            var response = await _client.PostAsync("/api/channels.kick", payload);
            await Parse<GeneralResponse>(response);
        }

        public async Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            var validHandle = _slackHelper.FixHandleNameForSlack(handle);
            var payload = _serializer.GetPayload(new { Name = name, Handle = validHandle, Description = description });
            var response = await _client.PostAsync("/api/usergroups.create", payload);

            return await Parse<CreateUserGroupResponse>(response);
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
            var response = await _client.GetAsync("/api/conversations.list");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var getConversationsResponse = _serializer.Deserialize<GetConversationsResponse>(content);
            
            return getConversationsResponse;
        }

        private async Task<string> GetUserId(string email)
        {
            var response = await _client.GetAsync($"/api/users.lookupByEmail?email={email}");

            return (await Parse<LookupUserResponse>(response))?.User?.Id;
        }

        private async Task<List<string>> GetUserGroupUsers(string userGroupId)
        {
            var response = await
             _client.GetAsync($"/api/usergroups.users.list?usergroup={userGroupId}&include_disabled=false");
            return (await Parse<ListUsersInUserGroupResponse>(response))?.Users;
        }
        
        public async Task<List<UserGroup>> GetUserGroups()
        {
            var response = await
                _client.GetAsync($"/api/usergroups.list");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            
            var userGroupsReponse = _serializer.Deserialize<ListUserGroupsResponse>(content);
            if (!userGroupsReponse.Ok)
            {
                throw new SlackFacadeException($"API error: {userGroupsReponse.Error}");
            }

            return userGroupsReponse.UserGroups;
        }

        private async Task UpdateUserGroupUsers(string userGroupId, List<string> users)
        {
            var usersList = string.Join(",", users);
            var payload = _serializer.GetPayload(new { Usergroup = userGroupId, users = usersList });

            var response = await _client.PostAsync("/api/usergroups.users.update", payload);
            await Parse<GeneralResponse>(response);
        }

        public class SlackFacadeException : Exception
        {
            public SlackFacadeException() : base() {}
            public SlackFacadeException(string message) : base(message) {}
            public SlackFacadeException(string message, Exception inner) : base(message, inner) {}
        }
    }
}