using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.EventHandlers
{
    public class SlackCapabilityCreatedDomainEventHandler : IEventHandler<CapabilityCreatedDomainEvent>
    {
        private readonly ILogger<SlackCapabilityCreatedDomainEventHandler> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackService _slackService;

        public SlackCapabilityCreatedDomainEventHandler(
            ILogger<SlackCapabilityCreatedDomainEventHandler> logger,
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository,
            ISlackService slackService)
        {
            _logger = logger;
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
            _slackService = slackService;
        }

        public async Task HandleAsync(CapabilityCreatedDomainEvent domainEvent)
        {
            var createChannelResponse = await _slackFacade.CreateChannel(domainEvent.Payload.CapabilityName);

            UserGroup userGroup = null;
            try
            {
                userGroup = await _slackService.EnsureUserGroupExists(domainEvent.Payload.CapabilityName);
            }
            catch (SlackFacade.SlackFacadeException ex)
            {
                _logger.LogError($"Issue with Slack API during CreateUserGroup: {ex} : {ex.Message}");
            }


            var channelId = createChannelResponse?.Channel?.Id;
            var channelName = createChannelResponse?.Channel?.Name;

            var userGroupId = userGroup?.Id;

            if (createChannelResponse.Ok)
            {
                _logger.LogInformation($"Slack channel '{channelName}' for capability '{domainEvent.Payload.CapabilityName}' created with ID: {channelId}");

                // Save even without user group.
                var capability = Capability.Create(
                    id: domainEvent.Payload.CapabilityId,
                    name: domainEvent.Payload.CapabilityName,
                    slackChannelId: channelId,
                    slackUserGroupId: userGroupId);

                await _capabilityRepository.Add(capability);

                // Notify channel about handle.
                var sendNotificationResponse = await _slackFacade.SendNotificationToChannel(
                  channel: channelId, 
                  message: 
                  $"Thank you for creating capability '{capability.Name}'.\n" +
                  $"This channel along with handle @{userGroup.Handle} has been created.\n" + 
                  "Use the handle to notify capability members.\n" + 
                  $"If you want to define a better handle, you can do this in the '{userGroup.Name}'");

                // Pin message.
                await _slackFacade.PinMessageToChannel(channelId, sendNotificationResponse.TimeStamp);
            }
            else
            {
                _logger.LogError($"Error creating Slack channel '{channelName}', Error: '{createChannelResponse.Error}'");
            }
        }
    }
}