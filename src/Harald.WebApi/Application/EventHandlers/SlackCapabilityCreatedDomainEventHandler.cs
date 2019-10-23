using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dafda.Messaging;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Harald.Infrastructure.Slack.Dto;
using Harald.Infrastructure.Slack.Exceptions;

namespace Harald.WebApi.Application.EventHandlers
{
    public class SlackCapabilityCreatedDomainEventHandler : IMessageHandler<CapabilityCreatedDomainEvent>
    {
        private readonly ILogger<CapabilityCreatedDomainEvent> _logger;
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackService _slackService;

        public SlackCapabilityCreatedDomainEventHandler(ILogger<CapabilityCreatedDomainEvent> logger,
            ISlackFacade slackFacade,
            ICapabilityRepository capabilityRepository,
            ISlackService slackService)
        {
            _logger = logger;
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
            _slackService = slackService;
        }

        async Task Reee()
        {
            Console.WriteLine("RERERERERER");
            await Task.Run(() => { Task.Delay(1000); });
            Console.WriteLine("ERERERERERE");
        }

        public async Task Handle(CapabilityCreatedDomainEvent message)
        {
            _logger.LogInformation(@"Handling: {@Message}", message);

            var x = "";
            var xx = new List<string>();
            await Reee();
            var convs = await _slackFacade.GetConversations();
            
            // _dbContext seems to get disposed here.
            var createChannelResponse = await _slackFacade.CreateChannel(message.CapabilityName);

            UserGroupDto userGroup = null;
            try
            {
                userGroup = await _slackService.EnsureUserGroupExists(message.CapabilityName);
            }
            catch (SlackFacadeException ex)
            {
                _logger.LogError($"Issue with Slack API during CreateUserGroup: {ex} : {ex.Message}");
            }


            var channelName = createChannelResponse?.Channel?.Name;


            if (createChannelResponse.Ok)
            {
                var channelId = new ChannelId(createChannelResponse?.Channel?.Id);
                _logger.LogInformation($"Slack channel '{channelName}' for capability '{message.CapabilityName}' created with ID: {channelId}");

                var userGroupId = userGroup?.Id;
                // Save even without user group.
                var capability = Capability.Create(
                    id: message.CapabilityId,
                    name: message.CapabilityName,
                    slackChannelId: channelId,
                    slackUserGroupId: userGroupId);
                _logger.LogInformation($"Capability id: '{capability.Id}'  name: '{capability.Name}' slackChannelId: '{capability.SlackChannelId}', userGroupId: '{capability.SlackUserGroupId}'");

                await _capabilityRepository.Add(capability);

                // Notify channel about handle.
                var sendNotificationResponse = await _slackFacade.SendNotificationToChannel(
                    channelIdentifier: channelId.ToString(), 
                  message: 
                  $"Thank you for creating capability '{capability.Name}'.\n" +
                  $"This channel along with handle @{userGroup.Handle} has been created.\n" + 
                  "Use the handle to notify capability members.\n" + 
                  $"If you want to define a better handle, you can do this in the '{userGroup.Name}'");

                // Pin message.
                await _slackFacade.PinMessageToChannel(channelId.ToString(), sendNotificationResponse.TimeStamp);
            }
            else
            {
                _logger.LogError($"Error creating Slack channel '{channelName}', Error: '{createChannelResponse.Error}'");
            }
        }
    }
}