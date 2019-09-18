using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.features.connections.Domain.Model
{
    public class SenderType : StringSubstitutable
    {
        public SenderType(string value) : base(value)
        {
        }

        public static explicit operator SenderType(String input)
        {
            return new SenderType(input);
        }

        public static SenderType CreateFromString(string channelType)
        {
            channelType = channelType.ToLower();

            if (channelType.Equals(new SenderTypeCapability()))
            {
                return new SenderTypeCapability();
            }

            throw new ArgumentException($"a SenderType could not be created from the string: '{channelType}'");
        }
    }

    public class SenderTypeCapability : SenderType
    {
        public SenderTypeCapability() : base("capability")
        {
        }
    }
}