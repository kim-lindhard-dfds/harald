using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class GetConversationsResponse : GeneralResponse
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }

        public Channel GetChannel(string name)
        {
            var slackChannelObj = this.Channels.Where(ch => name.Equals(ch.Name));
            return slackChannelObj.Any() ? slackChannelObj.First() : null;
        }

        public bool ChannelExists(string name)
        {
            return GetChannel(name) != null;
        }
    }
}