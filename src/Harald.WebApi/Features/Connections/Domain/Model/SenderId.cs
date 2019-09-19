using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class SenderId :StringSubstitutable
    {
        public SenderId(string value) : base(value)
        {
        }
        
        public static explicit operator SenderId(String input) 
        {
            return new SenderId(input);
        }
    }
}