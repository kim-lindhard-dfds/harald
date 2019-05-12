using System.Text;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Facades.Slack;

namespace Harald.WebApi.EventHandlers
{
    public class SlackAWSContextAccountCreatedEventHandler : IEventHandler<AWSContextAccountCreatedDomainEvent>
    {
        private readonly ISlackFacade _slackFacade;

        public SlackAWSContextAccountCreatedEventHandler(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }

        public async Task HandleAsync(AWSContextAccountCreatedDomainEvent domainEvent)
        {

            var addUserCmd = $"Get-ADUser \"CN=IT BuildSource DevEx,OU=Shared Mailboxes,OU=IT,OU=DFDS AS,DC=dk,DC=dfds,DC=root\" | Set-ADUser -Add @{{proxyAddresses=\"smtp:${domainEvent.Data.RoleEmail}\"}}";
            var installToolsCmd = $"Get-WindowsCapability -Online | ? {{$_.Name -like 'Rsat.ActiveDirectory.DS-LDS.Tools*'}} | Add-WindowsCapability -Online";


            var sb = new StringBuilder();

            sb.AppendLine($"An AWS Context account has been created for ContextId: {domainEvent.Data.ContextId}");
            sb.AppendLine($"Please execute the following command:");
            sb.AppendLine(addUserCmd);
            sb.AppendLine($"Should you not have RSAT tools installed, please do so with command:");
            sb.AppendLine(installToolsCmd);
            
            var hardCodedDedChannelId = "GFYE9B99Q";
            await _slackFacade.SendNotificationToChannel(hardCodedDedChannelId, sb.ToString());
        }
    }
}