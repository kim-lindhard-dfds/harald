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

        public SlackFacade(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<CreateChannelResponse> CreateChannel(string channelName)
        {
            var validChannelName = FixChannelNameForSlack(channelName);
            var payload = _serializer.GetPayload(new { Name = validChannelName, Validate = true });
            var response = await _client.PostAsync("/api/channels.create", payload);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createChannelResponse = _serializer.Deserialize<CreateChannelResponse>(content);

            return createChannelResponse;
        }

        public async Task<SendNotificationResponse> SendNotificationToChannel(string channel, string message)
        {
            var payload = _serializer.GetPayload(new { Channel = channel, Text = message });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var sendNotificationResponse = _serializer.Deserialize<SendNotificationResponse>(content);

            return sendNotificationResponse;
        }

        public async Task<SendNotificationResponse> SendNotificationToUser(string email, string message)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = userId, Text = message, As_user = true });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var sendNotificationResponse = _serializer.Deserialize<SendNotificationResponse>(content);

            return sendNotificationResponse;
        }

        public async Task<GeneralResponse> PinMessageToChannel(string channel, string messageTimeStamp)
        {
            var payload = _serializer.GetPayload(new { Channel = channel, Timestamp = messageTimeStamp });

            var response = await _client.PostAsync("/api/pins.add", payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var generalResponse = _serializer.Deserialize<GeneralResponse>(content);

            return generalResponse;
        }

        public async Task InviteToChannel(string email, string channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = channelId, user = userId });

            var response = await _client.PostAsync("/api/channels.invite", payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFromChannel(string email, string channelId)
        {
            var userId = await GetUserId(email);
            var payload = _serializer.GetPayload(new { Channel = channelId, user = userId });

            var response = await _client.PostAsync("/api/channels.kick", payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task<CreateUserGroupResponse> CreateUserGroup(string name, string handle, string description)
        {
            var validHandle = FixHandleNameForSlack(handle);
            var payload = _serializer.GetPayload(new { Name = name, Handle = validHandle, Description = description });
            var response = await _client.PostAsync("/api/usergroups.create", payload);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createUserGroupResponse = _serializer.Deserialize<CreateUserGroupResponse>(content);

            return createUserGroupResponse;
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

        private async Task<string> GetUserId(string email)
        {
            var response = await _client.GetAsync($"/api/users.lookupByEmail?email={email}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            string userId = _serializer.GetTokenValue<string>(content, "['user']['id']");

            return userId;
        }

        private async Task<List<string>> GetUserGroupUsers(string userGroupId)
        {
            var response = await
             _client.GetAsync($"/api/usergroups.users.list?usergroup={userGroupId}&include_disabled=false");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = _serializer.GetTokenValue<List<string>>(content, "['users']");

            return users;
        }

        private async Task UpdateUserGroupUsers(string userGroupId, List<string> users)
        {
            var usersList = string.Join(",", users);
            var payload = _serializer.GetPayload(new { Usergroup = userGroupId, users = usersList });

            var response = await _client.PostAsync("/api/usergroups.users.update", payload);
            response.EnsureSuccessStatusCode();
        }

        private string FixChannelNameForSlack(string channelName)
        {
            // Max channel name length is 21.
            if (channelName.Length > 21)
            {
                channelName = channelName.Substring(0, 21);
            }

            // Make sure camel-casing is used, in order to split words.
            channelName = UppercaseFirstCharacter(channelName);

            var lowerCaseWords = Regex.Matches(channelName, @"([A-Z][a-z]+)")
            .Cast<Match>()
            .Select(m => m.Value.ToLower());

            var lowerCaseChannelNameWithHypens = string.Join("-", lowerCaseWords);

            return lowerCaseChannelNameWithHypens;
        }

        private string FixHandleNameForSlack(string handleName)
        {
            const string handleSuffix = "Members";
            handleName = handleName + handleSuffix;

            var lowerCaseWords = Regex.Matches(handleName, @"([A-Z][a-z]+)")
            .Cast<Match>()
            .Select(m => m.Value.ToLower());

            var lowerCaseHandleNameWithHypens = string.Join("-", lowerCaseWords);

            return lowerCaseHandleNameWithHypens;
        }

        private string UppercaseFirstCharacter(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}