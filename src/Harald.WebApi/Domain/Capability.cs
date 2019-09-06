using System;

namespace Harald.WebApi.Domain
{
    public class Capability
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ChannelId ChannelId { get; private set; }
        public string SlackUserGroupId { get; private set; }

        private Capability(Guid id, string name, string slackChannelId, string slackUserGroupId)
        {
            Id = id;
            Name = name;
            ChannelId = new ChannelId(slackChannelId);
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
    }
}