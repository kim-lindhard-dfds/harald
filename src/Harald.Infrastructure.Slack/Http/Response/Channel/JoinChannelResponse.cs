using Harald.Infrastructure.Slack.Dto;

namespace Harald.Infrastructure.Slack.Http.Response.Channel
{
    public class JoinChannelResponse : SlackResponse
    {
        public ChannelDto Channel { get; set; }
    }
}

