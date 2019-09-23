namespace Harald.Infrastructure.Slack.Exceptions
{
    public class ChannelNotFoundException : SlackFacadeException
    {
        public ChannelNotFoundException(string message) : base(message)
        {
        }
    }
}
