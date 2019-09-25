using System;
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
            switch (channelType)
            {
                case string str when str.Equals(new ChannelTypeSlack(), StringComparison.OrdinalIgnoreCase):
                    return new ChannelTypeSlack();
                default:
                    throw new ArgumentException($"Invalid channel type {channelType}");
            }
        }
    }

    public class ChannelTypeSlack : ChannelType
    {
        public ChannelTypeSlack() : base("slack")
        {
        }
    }
}