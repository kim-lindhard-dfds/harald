using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Harald.IntegrationTests.Features.Infrastructure.Model;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Harald.IntegrationTests.Features.Infrastructure
{
    public static class ApiClient
    {
        public static class Connections
        {
            private const string ConnectionsUrl = "http://localhost:5123/api/v1/connections";

            public static async Task<ItemsEnvelope<ConnectionDto>> GetAsync(
                string senderType = null,
                string senderId = null,
                string channelType = null,
                string channelId = null
            )
            {
                var parametersToAdd = new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(senderType) == false)
                {
                    parametersToAdd.Add("clientType", senderType);
                }

                if (string.IsNullOrWhiteSpace(senderId) == false)
                {
                    parametersToAdd.Add("clientId", senderId);
                }

                if (string.IsNullOrWhiteSpace(channelType) == false)
                {
                    parametersToAdd.Add("channelType", channelType);
                }

                if (string.IsNullOrWhiteSpace(channelId) == false)
                {
                    parametersToAdd.Add("channelId", channelId);
                }


                var uri = QueryHelpers.AddQueryString(ConnectionsUrl, parametersToAdd);
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                var httpClient = new HttpClient();
                var responseMessage = await httpClient.SendAsync(httpRequestMessage);

                var contentString = await responseMessage.Content.ReadAsStringAsync();

                var deserializeObject = JsonConvert.DeserializeObject<ItemsEnvelope<ConnectionDto>>(contentString);

                return deserializeObject;
            }

            public static async Task AddAsync(
                string clientName,
                string clientId,
                string channelName,
                string channelId
            )
            {
                var payload = new
                {
                    ClientType = "capability",
                    ClientName = clientName,
                    ClientId = clientId,
                    ChannelType = "slack",
                    ChannelName = channelName,
                    ChannelId = channelId
                };


                var uri = new Uri(ConnectionsUrl);
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

                httpRequestMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );


                var httpClient = new HttpClient();
                var responseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode == false)
                {
                    throw new Exception(responseMessage.ReasonPhrase);
                }
            }
        }
    }
}