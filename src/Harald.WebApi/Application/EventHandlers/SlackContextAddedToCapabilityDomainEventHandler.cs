using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Infrastructure.Messaging;

namespace Harald.WebApi.Application.EventHandlers
{
    public class SlackContextAddedToCapabilityDomainEventHandler : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly ISlackFacade _slackFacade;
        private readonly ExternalEventMetaDataStore _externalEventMetaDataStore;

        public SlackContextAddedToCapabilityDomainEventHandler(
            ICapabilityRepository capabilityRepository,
            ISlackFacade slackFacade,
            ExternalEventMetaDataStore externalEventMetaDataStore
        )
        {
            _capabilityRepository = capabilityRepository;
            _slackFacade = slackFacade;
            _externalEventMetaDataStore = externalEventMetaDataStore;
        }

        public async Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            var capabilities = await _capabilityRepository.GetById(domainEvent.Payload.CapabilityId);

            var message = CreateMessage(domainEvent, _externalEventMetaDataStore.XCorrelationId);

            var hardCodedDedChannelId = new ChannelId("GFYE9B99Q");
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId.ToString(), message);

            // Send message to Capability Slack channels
            foreach (var capability in capabilities)
            {
                await _slackFacade.SendNotificationToChannel(
                    capability.SlackChannelId.ToString(),
                    $"We're working on setting up your environment. Currently the following resources are being provisioned and are awaiting status updates" +
                    $"\n" +
                    $"{CreateTaskTable(false, false, false)}");
            }
        }

        public static string CreateMessage(ContextAddedToCapabilityDomainEvent domainEvent, string xCorrelationId)
        {
            var message = "Context added to capability\n" +
                          "run the following command from https://github.com/dfds/aws-account-manifests:\n" +
                          "---\n" +
                          $"CORRELATION_ID=\"{xCorrelationId}\" \\\n" +
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
            var awsMessage = awsAccDone
                ? $":heavy_check_mark: AWS account provisioned\n"
                : $":white_check_mark: AWS account provisioned\n";
            var k8sMessage = k8sCreatedDone
                ? $":heavy_check_mark: Kubernetes namespace created\n"
                : $":white_check_mark: Kubernetes namespace created\n";
            var adsyncMessage = adsyncDone
                ? $":heavy_check_mark: AWS and Kubernetes account enrollment\n"
                : $":white_check_mark: AWS and Kubernetes account enrollment\n";

            return $"{awsMessage}{k8sMessage}{adsyncMessage}";
        }
    }
}