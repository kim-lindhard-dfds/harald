using System;

namespace Harald.WebApi.Domain
{
    public class ChannelName : StringSubstitutable
    {
        private ChannelName(string value) : base(value)
        {
        }

        private const int MAX_CHANNEL_NAME_LENGTH = 80;

        public static ChannelName Create(string source)
        {
            if (MAX_CHANNEL_NAME_LENGTH < source.Length)
            {
                source = source.Substring(0, MAX_CHANNEL_NAME_LENGTH);
            }

            var fixedChannelName = source
                .Replace('_', '-')
                .ToLowerInvariant();


            return new ChannelName(fixedChannelName);
        }

        public static explicit operator ChannelName(String input)
        {
            return ChannelName.Create(input);
        }
    }
}