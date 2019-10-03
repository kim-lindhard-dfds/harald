using System;

namespace Harald.Infrastructure.Slack.Model
{
    public class SlackChannelName
    {
        private readonly string _value;

        
        private const int MAX_CHANNEL_NAME_LENGTH = 80;
        public SlackChannelName(string source)
        {
            if(string.IsNullOrEmpty(source)) { throw new ArgumentException($"SlackChannelName can't be: '{source}'''");}

            if (MAX_CHANNEL_NAME_LENGTH < source.Length)
            {
                source = source.Substring(0, MAX_CHANNEL_NAME_LENGTH);
            }

            var fixedChannelName = source
                .Replace('_', '-')
                .ToLowerInvariant();

            _value = fixedChannelName;
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator SlackChannelName(string input)
        {
            return new SlackChannelName(input);
        }

        public static implicit operator string(SlackChannelName input)
        {
            return input.ToString();
        }

        public override bool Equals(object obj)
        {
            if (
                obj is SlackChannelName stringSubstitutable &&
                _value.Equals(stringSubstitutable._value)
            )
            {
                return true;
            }

            if (
                obj is string text &&
                _value.Equals(text)
            )
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
