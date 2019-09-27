using System;
using System.Linq;
using System.Reflection;
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
            var validClientTypes = Assembly
                .GetAssembly(typeof(ClientType))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ClientType)))
                .Select(t => (ClientType) Activator.CreateInstance(t))
                .ToList();
            
            var returnChannelType = validClientTypes.SingleOrDefault(c =>
                clientType.Equals(c, StringComparison.OrdinalIgnoreCase));
                
            if (returnChannelType == null)
            {
                throw new ValidationException(
                    $"Invalid client type: '{clientType}'. Your options are: '{String.Join("', '", validClientTypes)}'");
            }

            return returnChannelType;
        }
    }

    public class ClientTypeCapability : ClientType
    {
        public ClientTypeCapability() : base("capability")
        {
        }
    }
}