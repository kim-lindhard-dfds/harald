using System;

namespace Harald.Infrastructure.Slack.Exceptions
{
    public class SlackFacadeException : Exception
    {
        public SlackFacadeException() : base()
        {
        }

        public SlackFacadeException(string message) : base(message)
        {
        }

        public SlackFacadeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
