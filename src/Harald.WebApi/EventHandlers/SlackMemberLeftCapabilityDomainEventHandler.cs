using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class SlackMemberLeftCapabilityDomainEventHandler : IEventHandler<MemberLeftCapabilityDomainEvent>
    {
        public string EventType => "member_left_capability";
        public Type EventTypeImplementation => typeof(MemberLeftCapabilityDomainEvent);
        private readonly ILogger<SlackMemberLeftCapabilityDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public SlackMemberLeftCapabilityDomainEventHandler(
            ILogger<SlackMemberLeftCapabilityDomainEventHandler> logger,
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository)
        {
            _logger = logger;
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        public async Task HandleAsync(MemberLeftCapabilityDomainEvent domainEvent)
        {
            var capability = await _capabilityRepository.Get(domainEvent.Data.CapabilityId);
            
            if (capability == null)
            {
                _logger.LogError(
                    $"Couldn't get capability with ID {domainEvent.Data.CapabilityId}. Can't remove member {domainEvent.Data.MemberEmail} from Slack.");
                return;    
            }

            // Remove user from Slack channel:
            await _slackFacade.RemoveFromChannel(
                email: domainEvent.Data.MemberEmail,
                channelId: capability.SlackChannelId);

            // Remove user from Slack user group:
            await _slackFacade.RemoveUserGroupUser(
                email: domainEvent.Data.MemberEmail,
                userGroupId: capability.SlackUserGroupId);

            // Notify user that it has been invited.
            await _slackFacade.SendNotificationToUser(
                email: domainEvent.Data.MemberEmail, 
                message: 
                $"Thank you for your contributions to capability {capability.Name}.\nYou have been removed from corresponding Slack channel and user group.");
        }
    }
}