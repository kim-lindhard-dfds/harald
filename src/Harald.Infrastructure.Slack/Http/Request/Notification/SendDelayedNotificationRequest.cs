using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.Notification
{
    public class SendDelayedNotificationRequest : SlackRequest
    {
        public SendDelayedNotificationRequest(string channelIdentifier, string message, long delayTimeInEpoch)
        {
            var serializedContent = JsonConvert.SerializeObject(new { channel = channelIdentifier, text = message, post_at = delayTimeInEpoch }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/chat.scheduleMessage", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
