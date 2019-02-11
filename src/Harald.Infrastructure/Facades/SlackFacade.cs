using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Harald.Application.Facades;


namespace Harald.Infrastructure.Facades
{
    public class SlackFacade : ISlackFacade
    {
        private readonly HttpClient _client;

        public SlackFacade(HttpClient client)
        {
            _client = client;
        }

        public async Task CreateChannel(string channelName)
        {
            var payload = GetPayload(new { Name = channelName, Validate = true });
            var response = await _client.PostAsync("/api/channels.create", payload);

            response.EnsureSuccessStatusCode();
        }

        public async Task SendNotification(string recipient, string message)
        {
            var payload = GetPayload(new { Channel = recipient, Text = message });

            var response = await _client.PostAsync("/api/chat.postMessage", payload);
            response.EnsureSuccessStatusCode();
        }

        private StringContent GetPayload(object objectToSerialize)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var payload = new StringContent(
                content: JsonConvert.SerializeObject(objectToSerialize, serializerSettings),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            return payload;
        }
    }
}