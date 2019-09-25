using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Harald.Infrastructure.Slack.Model
{
    public class SlackChannelName : IValidatableObject
    {
        private readonly string _value;

        public SlackChannelName(string value)
        {
            _value = value;
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (_value.Contains("_"))
            {
                results.Add(new ValidationResult(string.Format(ValidationMessages.Harald_Infrastructure_Slack_Model_SlackChannelName_Validate_Validate_Name, _value)));
            }

            return new List<ValidationResult>();
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
