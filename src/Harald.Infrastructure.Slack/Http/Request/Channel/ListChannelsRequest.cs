using System.Collections.Generic;
using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.Channel
{
    public class ListChannelsRequest : SlackRequest
    {
        public ListChannelsRequest(string token)
        {
            RequestUri = new System.Uri($"api/conversations.list?token={token}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}
