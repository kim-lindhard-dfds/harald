using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.Notification
{
    public class SendNotificationRequest : SlackRequest
    {
        public SendNotificationRequest(string channelIdentifier, string message, bool asUser = false)
        {
            var serializedContent = JsonConvert.SerializeObject(new { channel = channelIdentifier, text = message, as_user = asUser }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/chat.postMessage", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
