using Harald.Infrastructure.Slack;
using Harald.WebApi.Domain;
using Harald.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Guid = System.Guid;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
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

        [HttpPost()]
        [Route("[action]")]
        public async Task<IActionResult> Leave(LeaveChannelInput input)
        {
            if (input.CapabilityId.Equals(Guid.Empty))
            {
                return BadRequest("Capability ID is required.");
            }

            if (string.IsNullOrEmpty((input.ChannelId)))
            {
                return BadRequest("Channel ID is required.");
            }

            var capability = await _capabilityRepository.Get(input.CapabilityId);

            if (capability == null)
            {
                return UnprocessableEntity($"Capability ID '{input.CapabilityId}' doesn't exist.");
            }

            await _slackFacade.LeaveChannel(capability.SlackChannelId.ToString());

            var capabilities = await _capabilityRepository.GetAll();

            if (!capabilities.Any(c => c.SlackChannelId.Equals(capability.SlackChannelId) && !c.Id.Equals(capability.Id)))
            {
                await _slackFacade.ArchiveChannel(capability.SlackChannelId.ToString());
            }

            return Accepted();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Join(JoinChannelInput input)
        {
            if (input.CapabilityId.Equals(Guid.Empty))
            {
                return BadRequest("Capability ID is required.");
            }

            if (string.IsNullOrEmpty((input.ChannelId)))
            {
                return BadRequest("Channel ID is required.");
            }

            if (string.IsNullOrEmpty((input.ChannelName)))
            {
                return BadRequest("Channel name is required.");
            }

            var capability = await _capabilityRepository.Get(input.CapabilityId);

            if (capability == null)
            {
                return UnprocessableEntity($"Capability ID '{input.CapabilityId}' doesn't exist.");
            }

            //TODO: Check if connection already exists or return UnprocessableEntity.

            var response = await _slackFacade.JoinChannel(input.ChannelName);

            //TODO: Add new slack channel id (connection) to capability and update.

            return Accepted(response.Channel);
        }
    }
}