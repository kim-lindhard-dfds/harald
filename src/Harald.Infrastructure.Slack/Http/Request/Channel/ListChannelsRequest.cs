using System.Collections.Generic;
using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.Channel
{
    public class ListChannelsRequest : SlackRequest
    {
        public ListChannelsRequest(string token)
        {
            RequestUri = new System.Uri($"api/channels.list", System.UriKind.Relative);
            Content = new FormUrlEncodedContent(new[]{ new KeyValuePair<string, string>("token", token) });
            Method = HttpMethod.Get;
        }
    }
}
