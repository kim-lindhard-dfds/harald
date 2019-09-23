using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.Channel
{
    public class PinMessageToChannelRequest : SlackRequest
    {
        public PinMessageToChannelRequest(string channelIdentifier, string messageTimeStamp)
        {
            var serializedContent = JsonConvert.SerializeObject(new { channel = channelIdentifier, timestamp = messageTimeStamp }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/pins.add", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
