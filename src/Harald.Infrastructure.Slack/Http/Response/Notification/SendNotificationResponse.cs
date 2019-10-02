using Newtonsoft.Json;
using System.Net;

namespace Harald.Infrastructure.Slack.Http.Response.Notification
{
    public class SendNotificationResponse : SlackResponse
    {
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
    }
}