using System;
using System.Threading;
using System.Threading.Tasks;
using Harald.Infrastructure.Slack;
using Harald.Infrastructure.Slack.Http.Response.Conversation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Harald.WebApi.Enablers.SlackHealthCheck
{
    public class SlackHealthProbe : IHealthCheck
    {
        private ISlackFacade _slackFacade;

        public SlackHealthProbe(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            GetConversationsResponse result;
            try
            {
                result = await _slackFacade.GetConversations();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Degraded("Unable to get expected response from Slack");
            }

            if (result.Ok)
            {
                return HealthCheckResult.Healthy("Slack is capable of returning an response from their API");
            }

            return HealthCheckResult.Degraded("Unable to get expected response from Slack");
        }
    }
}