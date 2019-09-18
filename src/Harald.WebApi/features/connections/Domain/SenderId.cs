using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.features.connections.Domain
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