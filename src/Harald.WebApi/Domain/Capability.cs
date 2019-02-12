using System;

namespace Harald.WebApi.Domain
{
    public class Capability
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SlackChannel { get; set; }
        public string SlackUserGroup { get; set; }
    }
}