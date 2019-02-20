using System;

namespace Harald.WebApi.Infrastructure.Messaging
{
    public class MessagingException : Exception
    {
        public MessagingException(string message) : base(message)
        {
        }
    }
}