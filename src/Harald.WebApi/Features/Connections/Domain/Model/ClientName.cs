using System;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class ClientName :StringSubstitutable
    {
        public ClientName(string value) : base(value)
        {
        }
        
        public static explicit operator ClientName(string input) 
        {
            return new ClientName(input);
        }
    }
}