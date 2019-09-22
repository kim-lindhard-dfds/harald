using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class SenderName :StringSubstitutable
    {
        public SenderName(String value) : base(value)
        {
        }
        
        public static explicit operator SenderName(String input) 
        {
            return new SenderName(input);
        }
    }
}