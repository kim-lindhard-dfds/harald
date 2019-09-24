using System;

namespace Harald.WebApi.Domain
{
    public class ValidationException : Exception
    {

        public ValidationException(string messageToUser)
        {
            MessageToUser = messageToUser;
        }

        public ValidationException(string messageToUser, string message) : base(message)
        {
            MessageToUser = messageToUser;
        }

        public string MessageToUser { get; }
    }
}