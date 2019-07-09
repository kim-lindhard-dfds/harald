using System.Text;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.EventHandlers
{
    public class SlackAWSContextAccountCreatedEventHandler : IEventHandler<AWSContextAccountCreatedDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public SlackAWSContextAccountCreatedEventHandler(ISlackFacade slackFacade, ICapabilityRepository capabilityRepository)
        {
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        public async Task HandleAsync(AWSContextAccountCreatedDomainEvent domainEvent)
        {

            var addUserCmd = $"Get-ADUser \"CN=IT BuildSource DevEx,OU=Shared Mailboxes,OU=IT,OU=DFDS AS,DC=dk,DC=dfds,DC=root\" | Set-ADUser -Add @{{proxyAddresses=\"smtp:{domainEvent.Payload.RoleEmail}\"}}";
            var installToolsCmd = $"Get-WindowsCapability -Online | ? {{$_.Name -like 'Rsat.ActiveDirectory.DS-LDS.Tools*'}} | Add-WindowsCapability -Online";


            var sb = new StringBuilder();

            sb.AppendLine($"An AWS Context account has been created for ContextId: {domainEvent.Payload.ContextId}");
            sb.AppendLine($"Please execute the following command:");
            sb.AppendLine(addUserCmd);
            sb.AppendLine($"Should you not have RSAT tools installed, please do so with command:");
            sb.AppendLine(installToolsCmd);
            sb.AppendLine("---");
            sb.AppendLine($"Please manually set Tax settings for AWS Account {domainEvent.Payload.AccountId}");
            sb.AppendLine($"\tCountry: Denmark");
            sb.AppendLine($"\tTax registration number: DK14194711");
            sb.AppendLine($"\tBusiness Legal Name: DFDS A/S");

            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, sb.ToString());
            
            // Send message to Capability Slack channel
            var capability = await _capabilityRepository.Get(domainEvent.Payload.CapabilityId);
            await _slackFacade.SendNotificationToChannel(capability.SlackChannelId, $"Task status:\n{SlackContextAddedToCapabilityDomainEventHandler.CreateTaskTable(true, false, false)}");
            
        }
    }
}