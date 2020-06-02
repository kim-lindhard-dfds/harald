using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Harald.Infrastructure.Slack.Exceptions;

namespace Harald.WebApi.Application.EventHandlers
{
    public class SlackMemberJoinedCapabilityDomainEventHandler : IEventHandler<MemberJoinedCapabilityDomainEvent>
    {
        private readonly ILogger<SlackMemberJoinedCapabilityDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackService _slackService;

        public SlackMemberJoinedCapabilityDomainEventHandler(
            ILogger<SlackMemberJoinedCapabilityDomainEventHandler> logger,
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository,
            ISlackService slackService)
        {
            _logger = logger;
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
            _slackService = slackService;
        }

        public async Task HandleAsync(MemberJoinedCapabilityDomainEvent domainEvent)
        {
            var capabilities = await _capabilityRepository.GetById(domainEvent.Payload.CapabilityId);
            
            foreach (var capability in capabilities)
            {
                if (capability == null)
                {
                    _logger.LogError(
                        $"Couldn't get capability with ID {domainEvent.Payload.CapabilityId}. Can't add member {domainEvent.Payload.MemberEmail} to Slack.");

                    return;
                }

                try
                {
                    await _slackFacade.InviteToChannel(
                        email: domainEvent.Payload.MemberEmail,
                        channelIdentifier: capability.SlackChannelId.ToString());
                }
                catch (SlackFacadeException ex)
                {
                    _logger.LogError($"Issue with Slack API during InviteToChannel: {ex} : {ex.Message}");
                }

                try
                {
                    //TODO: Should we remove this?
                    if (string.IsNullOrEmpty(capability.SlackUserGroupId))
                    {
                        var userGroup = await _slackService.EnsureUserGroupExists(capability.Name);

                        capability.SetUserGroupID(userGroup.Id);
                    }
                                       
                    await capability.AddMember(domainEvent.Payload.MemberEmail);
                    await _capabilityRepository.Update(capability);

                    // Add user to Slack user group:    
                    await _slackFacade.AddUserGroupUser(email: domainEvent.Payload.MemberEmail,
                        userGroupId: capability.SlackUserGroupId);
                }
                catch (SlackFacadeException ex)
                {
                    _logger.LogError($"Issue with Slack API during AddUserGroupUser: {ex} : {ex.Message}");
                }
            }
        }
    }
}