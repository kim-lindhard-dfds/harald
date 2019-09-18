using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Harald.IntegrationTests.Features.Infrastructure
{
    public static class ApiClient
    {
        public static class Connections
        {
            public static async Task<dynamic> GetAsync(
                string senderType = null,
                string senderId = null,
                string channelType = null,
                string channelId = null
            )
            {
                var parametersToAdd = new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(senderType) == false)
                {
                    parametersToAdd.Add("senderType", senderType);
                }
                
                if (string.IsNullOrWhiteSpace(senderId) == false)
                {
                    parametersToAdd.Add("senderId", senderId);
                }

                if (string.IsNullOrWhiteSpace(channelType) == false)
                {
                    parametersToAdd.Add("channelType", channelType);
                }

                if (string.IsNullOrWhiteSpace(channelId) == false)
                {
                    parametersToAdd.Add("channelId", channelId);
                }
                
                
                var uri = QueryHelpers.AddQueryString("http://localhost:5123/api/v1/connections", parametersToAdd);
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                var httpClient = new HttpClient();
                var responseMessage = await httpClient.SendAsync(httpRequestMessage);

                return await ResponseMessageToDynamicAsync(responseMessage);
            }
        }

        private static async Task<dynamic> ResponseMessageToDynamicAsync(HttpResponseMessage responseMessage)
        {
            var contentString = await responseMessage.Content.ReadAsStringAsync();

            dynamic deserializeObject = JsonConvert.DeserializeObject(contentString);

            return deserializeObject;
        }
    }
}