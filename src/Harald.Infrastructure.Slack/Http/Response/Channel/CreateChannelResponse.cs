using Harald.Infrastructure.Slack.Dto;
using Harald.Infrastructure.Slack.Http.Response;
using System.Net;

namespace Harald.Infrastructure.Slack.Http.Response.Channel
{
    public class CreateChannelResponse : SlackResponse
    {
        public ChannelDto Channel { get; set; }
    }
}

