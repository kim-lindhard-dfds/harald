using System.Threading.Tasks;
using Harald.Infrastructure.Slack;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Harald.WebApi.Domain;
using Harald.WebApi.Models;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public ChannelController(
            ISlackFacade slackFacade, 
            ICapabilityRepository capabilityRepository)
        {
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Leave(LeaveChannelInput input)
        {
            if (input == null)
            {
                return BadRequest("LeaveChannelInput is required.");
            }

            var capability = await _capabilityRepository.Get(input.CapabilityId);

            if (capability == null)
            {
                return UnprocessableEntity($"Capability ID '{input.CapabilityId}' doesn't exist.");
            }

            await _slackFacade.LeaveChannel(capability.SlackChannelId.ToString());

            var capabilities = await _capabilityRepository.GetAll();

            if (!capabilities.Any(c => c.SlackChannelId == capability.SlackChannelId && c.Id != capability.Id))
            {
                await _slackFacade.ArchiveChannel(capability.SlackChannelId.ToString());
            }

            return Accepted();
        }

        [HttpPost]
        public async Task<IActionResult> Join(JoinChannelInput input)
        {
            if (input == null)
            {
                return BadRequest("LeaveChannelInput is required.");
            }

            var capability = await _capabilityRepository.Get(input.CapabilityId);

            if (capability == null)
            {
                return UnprocessableEntity($"Capability ID '{input.CapabilityId}' doesn't exist.");
            }

            var response = await _slackFacade.JoinChannel(input.ChannelName);
            
            //TODO: Cannot finish this logic before Kims PR is ready. :) (add new slack channel id (connection) to capability)

            return Accepted(response.Channel);
        }
    }
}