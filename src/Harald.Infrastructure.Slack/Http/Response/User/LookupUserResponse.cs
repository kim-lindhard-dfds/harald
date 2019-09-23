using Harald.Infrastructure.Slack.Dto;

namespace Harald.Infrastructure.Slack.Http.Response.User
{
    public class LookupUserResponse : SlackResponse
    {
        public UserDto User { get; set; }
    }
    
}