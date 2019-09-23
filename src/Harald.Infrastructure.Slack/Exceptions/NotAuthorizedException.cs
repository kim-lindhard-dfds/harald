namespace Harald.Infrastructure.Slack.Exceptions
{
    public class NotAuthorizedException : SlackFacadeException
    {
        public NotAuthorizedException(string message) : base(message)
        {
        }
    }
}
