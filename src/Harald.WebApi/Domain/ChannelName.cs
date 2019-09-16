using System;

namespace Harald.WebApi.Domain
{
    public class ChannelName : StringSubstitutable
    {
        private ChannelName(string value) : base(value)
        {
        }

        public static ChannelName Create(string source)
        {
            // Max channel name length is 21.
            if (source.Length > 21)
            {
                source = source.Substring(0, 21);
            }

            var fixedChannelName = source.Replace('_', '-').ToLowerInvariant();
            
            

            return new ChannelName(fixedChannelName);
        }
        public static explicit operator ChannelName(String input) 
        {
            return ChannelName.Create(input);
        }
    }
}