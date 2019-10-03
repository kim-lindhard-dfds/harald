using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.Channel
{
    public class ListChannelsRequest : SlackRequest
    {
        public ListChannelsRequest(string token, bool excludeArchived = true)
        {
            RequestUri = new System.Uri($"api/conversations.list?token={token}&limit=10000&exclude_archived={excludeArchived}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}
