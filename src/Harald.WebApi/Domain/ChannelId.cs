using System;

namespace Harald.WebApi.Domain
{
    public class ChannelId : StringSubstitutable
    {
        public ChannelId(string value) : base(value)
        {
        }

        public static explicit operator ChannelId(String input) 
        {
            return new ChannelId(input);
        }
    }
}