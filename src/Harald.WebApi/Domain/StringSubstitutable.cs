using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace Harald.WebApi.Domain
{
    public abstract class StringSubstitutable
    {
        private readonly string _value;

        public bool IsEmpty => string.IsNullOrEmpty(_value);

        protected StringSubstitutable(string value)
        {
            _value = value;
        }

        public static implicit operator string(StringSubstitutable stringSubstitutable)
        {
            return stringSubstitutable?._value;
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            if (
                obj is StringSubstitutable stringSubstitutable && 
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