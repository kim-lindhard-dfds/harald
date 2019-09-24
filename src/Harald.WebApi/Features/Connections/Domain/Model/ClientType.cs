using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ClientType : StringSubstitutable
    {
        public ClientType(string value) : base(value)
        {
        }

        public static explicit operator ClientType(string input)
        {
            return Create(input);
        }

        public static ClientType Create(string channelType)
        {
            channelType = channelType?.ToLower() ?? string.Empty;

            if (channelType.Equals(new ClientTypeCapability()))
            {
                return new ClientTypeCapability();
            }

            throw new ArgumentException($"a ClientType could not be created from the string: '{channelType}'");
        }
    }

    public class ClientTypeCapability : ClientType
    {
        public ClientTypeCapability() : base("capability")
        {
        }
    }
}