using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.Channel
{
    public class DeleteChannelRequest : SlackRequest
    {
        public DeleteChannelRequest(string channelIdentifier, string token)
        {
            RequestUri = new System.Uri($"api/channels.delete?token={token}&channel={channelIdentifier}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}
