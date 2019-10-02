using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure.Model;
using Newtonsoft.Json;

namespace Harald.IntegrationTests.Features.Infrastructure
{
    public class SlackApiServerSpyClient
    {
        public async Task<dynamic> GetNextInteractionWith5SecRetryAsync()
        {
            var endTime = DateTime.Now.AddSeconds(5);
            dynamic result = null;
            do
            {
                result = await GetNextInteractionAsync();
                if (result == null)
                {
                    Thread.Sleep(1000);
                }
                
            } while (result == null && DateTime.Now < endTime);

            return result;
        }

        public async Task<dynamic> GetNextInteractionAsync()
        {
            var uri = new Uri("http://localhost:1447/interactions/next");

            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync(uri);


            var contentString = await responseMessage.Content.ReadAsStringAsync();


            dynamic deserializeObject = JsonConvert.DeserializeObject(contentString);

            return deserializeObject;
        }

        public async Task ResetAsync()
        {
            var uri = new Uri("http://localhost:1447/interactions/reset");

            var httpClient = new HttpClient();

            await httpClient.PostAsync(uri,null);
        }

        public async Task<SlackConversationDto> ChannelsCreateAsync(string channelName)
        {
            var uri = new Uri("http://localhost:1447/api/channels.create");
            var payload = new {name = channelName };
            var json = JsonConvert.SerializeObject(payload);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            var responseMessage = await httpClient.PostAsync(
                uri,
                content
            );


            var contentString = await responseMessage.Content.ReadAsStringAsync();
            
            dynamic deserializeObject = JsonConvert.DeserializeObject(contentString);

            return new SlackConversationDto{Id = deserializeObject.Channel.Id, Name = deserializeObject.Channel.Name };
        }
    }
}