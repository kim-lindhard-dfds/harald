using System;

namespace Harald.WebApi.Domain
{
    public class CapabilityMember
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        // For Entity Framework
        private CapabilityMember()
        {
        }

        public CapabilityMember(string email)
        {
            Email = email;
        }
    }
}