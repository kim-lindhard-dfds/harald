using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
    }
}