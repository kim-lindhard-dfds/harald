using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly ISlackFacade _slackFacade;

        public DispatchController(ISlackFacade slackFacade)
        {
            _slackFacade = slackFacade;
        }

        [HttpPost]
        public async Task<IActionResult> DispatchMessage(DispatchMessageInput input)
        {
            // TODO: Fetch channel from DB using capibilityId.
            const string capabilityChannel = "ded-team-one";
            await _slackFacade.SendNotification(capabilityChannel, input.Message);
         
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