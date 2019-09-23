using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Harald.Infrastructure.Slack.Model
{
    public class SlackChannelIdentifier : IValidatableObject
    {
        private readonly string _value;

        public SlackChannelIdentifier(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator SlackChannelIdentifier(string input) 
        {
            return new SlackChannelIdentifier(input);
        }

        public static implicit operator string(SlackChannelIdentifier input)
        {
            return input.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public override bool Equals(object obj)
        {
            if (
                obj is SlackChannelIdentifier stringSubstitutable &&
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