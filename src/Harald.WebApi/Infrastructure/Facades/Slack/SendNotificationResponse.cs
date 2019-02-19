using Newtonsoft.Json;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class SendNotificationResponse : GeneralResponse
    {
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
    }
}