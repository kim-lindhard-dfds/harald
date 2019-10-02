using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ClientId :StringSubstitutable
    {
        public ClientId(string value) : base(value)
        {
        }
        
        public static explicit operator ClientId(string input) 
        {
            return Create(input);
        }

        public static ClientId Create(string clientId)
        {
            return new ClientId(clientId);
        }
    }
}