using Harald.Infrastructure.Slack.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Harald.Infrastructure.Slack.Http.Response.Conversation
{
    public class GetConversationsResponse : SlackResponse
    {
        [JsonProperty("channels")]
        public List<ChannelDto> Channels { get; set; }

        public ChannelDto GetChannel(string name)
        {
            var slackChannelObj = this.Channels.Where(ch => name.Equals(ch.Name));

            return slackChannelObj?.FirstOrDefault();
        }

        public bool ChannelExists(string name)
        {
            return GetChannel(name) != null;
        }
    }
}