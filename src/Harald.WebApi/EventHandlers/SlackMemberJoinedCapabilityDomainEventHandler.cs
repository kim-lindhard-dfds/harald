using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class SlackMemberJoinedCapabilityDomainEventHandler : IEventHandler<MemberJoinedCapabilityDomainEvent>
    {
        public string EventType => "memberjoinedcapability";
        public Type EventTypeImplementation => typeof(MemberJoinedCapabilityDomainEvent);
        private readonly ILogger<SlackMemberJoinedCapabilityDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;

        public SlackMemberJoinedCapabilityDomainEventHandler(
            ILogger<SlackMemberJoinedCapabilityDomainEventHandler> logger,
            ISlackFacade slackFacade)
        {
            _logger = logger;
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(MemberJoinedCapabilityDomainEvent domainEvent)
        {
            // TODO: Get capability from DB.
            var capability = new Capability(Guid.NewGuid(), null, null, null); 
            
            // Invite user to Slack channel:
            await _slackFacade.InviteToChannel(
                email: domainEvent.Data.MemberEmail,
                channelId: capability.SlackChannelId);

            // Add user to Slack user group:
            await _slackFacade.AddUserGroupUser(
                email: domainEvent.Data.MemberEmail,
                userGroupId: capability.SlackUserGroupId);

            // Notify user that it has been invited.
            await _slackFacade.SendNotificationToUser(
                email: domainEvent.Data.MemberEmail, 
                message: 
                $"Thank you for joining capability {capability.Name}.\nYou have been invited to corresponding Slack channel and user group.");
        }
    }
}