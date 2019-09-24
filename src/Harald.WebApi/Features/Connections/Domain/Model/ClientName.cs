using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ClientName :StringSubstitutable
    {
        public ClientName(String value) : base(value)
        {
        }
        
        public static explicit operator ClientName(String input) 
        {
            return new ClientName(input);
        }
    }
}