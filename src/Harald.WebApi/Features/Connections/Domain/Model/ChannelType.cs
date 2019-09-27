using System;
using System.Linq;
using System.Reflection;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ChannelType : StringSubstitutable
    {
        protected internal ChannelType(string value) : base(value)
        {
        }


        public static explicit operator ChannelType(string input)
        {
            return Create(input);
        }

        public static ChannelType Create(string channelType)
        {
            var validChannelTypes = Assembly
                .GetAssembly(typeof(ChannelType))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ChannelType)))
                .Select(t => (ChannelType) Activator.CreateInstance(t))
                .ToList();

            var returnChannelType = validChannelTypes.SingleOrDefault(c =>
                channelType.Equals(c, StringComparison.OrdinalIgnoreCase));
                
            if (returnChannelType == null)
            {
                throw new ValidationException(
                    $"Invalid channel type: '{channelType}'. Your options are: '{String.Join("', '", validChannelTypes)}'");
            }

            return returnChannelType;
        }
    }

    public class ChannelTypeSlack : ChannelType
    {
        public ChannelTypeSlack() : base("slack")
        {
        }
    }
}