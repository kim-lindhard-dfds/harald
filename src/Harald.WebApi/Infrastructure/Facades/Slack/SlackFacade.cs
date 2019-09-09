using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Serialization;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class SlackFacade : ISlackFacade
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public SlackFacade(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<CreateChannelResponse> CreateChannel(ChannelName channelName)
        {
            var validChannelName = ChannelName.Create(channelName);
            var payload = _serializer.GetPayload(new {Name = validChannelName.ToString(), Validate = true});
            var response = await _client.PostAsync("/api/channels.create", payload);

            return await Parse<CreateChannelResponse>(response);
        }


        // This uses an undocumented API, tread carefully.
        public async Task DeleteChannel(ChannelId channelId, string token)
        {
            var response = await _client.GetAsync($"/api/channels.delete?token={token}&channel={channelId}");

            await Parse<GeneralResponse>(response);
        }

        public async Task RenameChannel(ChannelId channelId, ChannelName name)
        {
            var payload = _serializer.GetPayload(new {channel = channelId.ToString(), name = name.ToString()});
            var response = await _client.PostAsync("/api/channels.rename", payload);

            await Parse<GeneralResponse>(response);
        }

        public async Task<SendNotificationResponse> SendNotificationToChannel(ChannelId channelId, string message)
        {
            var payload = _serializer.GetPayload(new {Channel = channelId.ToString(), Text = message});

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<SendNotificationResponse> SendDelayedNotificationToChannel(ChannelId channelId,
            string message, long delayTimeInEpoch)
        {
            var payload = _serializer.GetPayload(new {Channel = channelId.ToString(), Text = message, post_at = delayTimeInEpoch});
            var response = await _client.PostAsync("/api/chat.scheduleMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new {Channel = userId, Text = message, As_user = false});

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            return await Parse<SendNotificationResponse>(response);
        }

        public async Task<GeneralResponse> PinMessageToChannel(ChannelId channelId, string messageTimeStamp)
        {
            var payload = _serializer.GetPayload(new {Channel = channelId.ToString(), Timestamp = messageTimeStamp});
            var response = await _client.PostAsync("/api/pins.add", payload);
            return await Parse<GeneralResponse>(response);
        }

        public async Task InviteToChannel(string email, ChannelId channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new {Channel = channelId.ToString(), user = userId});

            var response = await _client.PostAsync("/api/channels.invite", payload);
            await Parse<GeneralResponse>(response);
        }

        public async Task RemoveFromChannel(string email, ChannelId channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new {Channel = channelId.ToString(), user = userId});

            var response = await _client.PostAsync("/api/channels.kick", payload);
            await Parse<GeneralResponse>(response);
        }

        public async Task<CreateUserGroupResponse> CreateUserGroup(string name, UserGroupHandle handle,
            string description)
        {
            var payload = _serializer.GetPayload(new {Name = name, Handle = handle.ToString(), Description = description});
            var response = await _client.PostAsync("/api/usergroups.create", payload);

            return await Parse<CreateUserGroupResponse>(response);
        }

        public async Task RenameUserGroup(string usergroupId, string name, UserGroupHandle handle)
        {
            var payload = _serializer.GetPayload(new
                {usergroup = usergroupId, name = name, handle = handle.ToString().ToLower()});
            var response = await _client.PostAsync("/api/usergroups.update", payload);

            await Parse<GeneralResponse>(response);
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

            return await Parse<GetConversationsResponse>(response);
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

            var listUsersInUserGroupResponse = await Parse<ListUsersInUserGroupResponse>(response);
            return listUsersInUserGroupResponse?.Users;
        }

        public async Task<List<UserGroup>> GetUserGroups()
        {
            var response = await
                _client.GetAsync($"/api/usergroups.list");

            var listUserGroupsResponse = await Parse<ListUserGroupsResponse>(response);

            return listUserGroupsResponse.UserGroups;
        }

        private async Task UpdateUserGroupUsers(string userGroupId, List<string> users)
        {
            var usersList = string.Join(",", users);
            var payload = _serializer.GetPayload(new {Usergroup = userGroupId, users = usersList});

            var response = await _client.PostAsync("/api/usergroups.users.update", payload);
            await Parse<GeneralResponse>(response);
        }

        private async Task<T> Parse<T>(HttpResponseMessage response) where T : GeneralResponse
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = _serializer.Deserialize<T>(content);

            if (data.Ok)
            {
                return data;
            }

            if (data.Error.Equals("not_authed"))
            {
                throw new NotAuthorizedException($"API error: {content}");
            }
            if (data.Error.Equals("channel_not_found"))
            {
                throw new ChannelNotFoundException($"API error: {content}");
            }
            throw new SlackFacadeException($"API error: {content}");
        }

        public class ChannelNotFoundException : SlackFacadeException
        {
            public ChannelNotFoundException(string message) : base(message)
            {
            }
        }
      
        public class NotAuthorizedException : SlackFacadeException
        {
            public NotAuthorizedException(string message) : base(message)
            {
            }
        }

        public class SlackFacadeException : Exception
        {
            public SlackFacadeException() : base()
            {
            }

            public SlackFacadeException(string message) : base(message)
            {
            }

            public SlackFacadeException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}