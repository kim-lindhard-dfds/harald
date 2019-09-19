using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ChannelType : StringSubstitutable
    {
        protected internal ChannelType(string value) : base(value)
        {
        }

              
        public static explicit operator ChannelType(String input) 
        {
            return new ChannelType(input);
        }
        
        public static ChannelType CreateFromString(string channelType)
        {
            channelType = channelType.ToLower();

            if (channelType.Equals(new ChannelTypeSlack()))
            {
                return new ChannelTypeSlack();
            }

            throw new ArgumentException($"a channelType could not be created from the string: '{channelType}'");
        }
    }

    public class ChannelTypeSlack : ChannelType
    {
        public ChannelTypeSlack() : base("slack")
        {
        }
    }
}