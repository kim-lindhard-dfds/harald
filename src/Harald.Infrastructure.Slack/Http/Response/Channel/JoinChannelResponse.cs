using Harald.Infrastructure.Slack.Dto;
using Harald.Infrastructure.Slack.Http.Response;

namespace Harald.Infrastructure.Slack.Response.Channel
{
    public class JoinChannelResponse : SlackResponse
    {
        public ChannelDto Channel { get; set; }
    }
}

