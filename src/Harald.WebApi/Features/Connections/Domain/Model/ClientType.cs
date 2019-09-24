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

        public static ClientType Create(string clientType)
        {
            switch (clientType)
            {
                case string str when str.Equals(new ClientTypeCapability(), StringComparison.OrdinalIgnoreCase):
                    return new ClientTypeCapability();
                default:
                    return null;
            }
        }
    }

    public class ClientTypeCapability : ClientType
    {
        public ClientTypeCapability() : base("capability")
        {
        }
    }
}