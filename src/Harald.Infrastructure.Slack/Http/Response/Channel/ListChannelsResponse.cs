using Harald.Infrastructure.Slack.Dto;
using System.Collections.Generic;

namespace Harald.Infrastructure.Slack.Http.Response.Channel
{
    public class ListChannelsResponse : SlackResponse
    {
        public List<ChannelDto> Channels { get; set; }
    }
}

