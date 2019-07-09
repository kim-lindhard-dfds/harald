using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEventHandler : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackFacade _slackFacade;

        public SlackContextAddedToCapabilityDomainEventHandler(ICapabilityRepository capabilityRepository,
            ISlackFacade slackFacade)
        {
            _capabilityRepository = capabilityRepository;
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            var capability = await _capabilityRepository.Get(domainEvent.Payload.CapabilityId);

            var message = CreateMessage(domainEvent, capability);

            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, message);
            
            // Send message to Capability Slack channel
            await _slackFacade.SendNotificationToChannel(capability.SlackChannelId,
                $"Your context \"{domainEvent.Payload.ContextName}\" has been added. Following tasks are in progress:" +
                $"\n" +
                $"{CreateTaskTable(false, false, false)}");
        }

        public static string CreateMessage(ContextAddedToCapabilityDomainEvent domainEvent, Capability capability)
        {
            var capabilityNameMessage = capability != null
                ? $" capability : \"{capability.Name}\"."
                : $" no capability matching the id could be found in Haralds database, this could be symptom of data being out of sync. The name from event is: {domainEvent.Payload.CapabilityName}";

            var message = "Context added to capability\n" +
                          "run the following command from https://github.com/dfds/aws-account-manifests:\n" +
                          "---\n" +
                          $"CORRELATION_ID=\"{domainEvent.XCorrelationId}\" \\\n" +
                          $"CAPABILITY_NAME=\"{domainEvent.Payload.CapabilityName}\" \\\n" +
                          $"CAPABILITY_ID=\"{domainEvent.Payload.CapabilityId}\" \\\n" +
                          $"CAPABILITY_ROOT_ID=\"{domainEvent.Payload.CapabilityRootId}\" \\\n" +
                          $"ACCOUNT_NAME=\"{domainEvent.Payload.CapabilityRootId}\" \\\n" + // OBS: for now account name and capabilty root id is the same by design
                          $"CONTEXT_NAME=\"{domainEvent.Payload.ContextName}\" \\\n" +
                          $"CONTEXT_ID=\"{domainEvent.Payload.ContextId}\" \\\n" +
                          "./generate-tfvars.sh";

            return message;
        }

        public static string CreateTaskTable(bool awsAccDone, bool k8sCreatedDone, bool adsyncDone)
        {
            var awsMessage = awsAccDone ? $":heavy_check_mark: AWS Account\n" : $":black_large_square: AWS Account\n";
            var k8sMessage = k8sCreatedDone ? $":heavy_check_mark: K8s Namespace created\n" : $":black_large_square: K8s Namespace created\n";
            var adsyncMessage = adsyncDone ? $":heavy_check_mark: ADSync\n" : $":black_large_square: ADSync\n";

            return $"{awsMessage}{k8sMessage}{adsyncMessage}";
        }
    }
}