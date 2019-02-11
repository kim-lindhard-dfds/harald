using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Harald.Application.Facades.Slack;
using Harald.Infrastructure.Serialization;


namespace Harald.Infrastructure.Facades.Slack
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
            var payload = _serializer.GetPayload(new { Name = channelName, Validate = true });
            var response = await _client.PostAsync("/api/channels.create", payload);

            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var createChannelResponse = _serializer.Deserialize<CreateChannelResponse>(content);

            return createChannelResponse;
        }

        public async Task SendNotification(string recipient, string message)
        {
            var payload = _serializer.GetPayload(new { Channel = recipient, Text = message });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}