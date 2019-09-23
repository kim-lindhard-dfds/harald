using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.Infrastructure.Slack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly ISlackFacade _slackFacade;
        private readonly ICapabilityRepository _capabilityRepository;

        public DispatchController(
            ISlackFacade slackFacade, 
            ICapabilityRepository capabilityRepository)
        {
            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> DispatchMessage(DispatchMessageInput input)
        {
            if (!input.CapabilityId.HasValue)
            {
                return BadRequest("capabilityId is required.");
            }

            var capability = await _capabilityRepository.Get(input.CapabilityId.Value);

            if (capability == null)
            {
                return UnprocessableEntity($"Capability ID '{input.CapabilityId.Value}' doesn't exist.");
            }

            var sendNotificationToChannelResponse =
             await _slackFacade.SendNotificationToChannel(capability.SlackChannelId.ToString(), input.Message);
         
            if (!sendNotificationToChannelResponse.Ok)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    $"An error occured trying to send notification: {sendNotificationToChannelResponse.Error}");
            }

            return Accepted();
        }
    }

    public class DispatchMessageInput
    {
        [Required]

        public Guid? CapabilityId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}