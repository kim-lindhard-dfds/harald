using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harald.WebApi.Domain
{
    public class Capability
    {
        private List<string> _members = new List<string>();
        public IEnumerable<string> Members => _members.AsReadOnly();
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ChannelId SlackChannelId { get; private set; }
        public string SlackUserGroupId { get; private set; }
        
        // For Entity Framework
        private Capability()
        {
        }

        private Capability(Guid id, string name, string slackChannelId, string slackUserGroupId)
        {
            Id = id;
            Name = name;
            SlackChannelId = new ChannelId(slackChannelId);
            SlackUserGroupId = slackUserGroupId;
        }

        public static Capability Create(Guid id, string name, string slackChannelId, string slackUserGroupId)
        {
            var capability = new Capability(
                id: id,
                name: name,
                slackChannelId: slackChannelId,
                slackUserGroupId: slackUserGroupId
            );

            return capability;
        }

        public ValueTask AddMember(string email) 
        {
            if(!_members.Contains(email))
            { 
                _members.Add(email);
            }

            return new ValueTask();
        }

        public ValueTask RemoveMember(string email)
        {
            _members.Remove(email);

            return new ValueTask();
        }
    }
}