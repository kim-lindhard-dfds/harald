using Harald.Infrastructure.Slack;
using Harald.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetChannels()
        {
            //TODO: Finish this.
            await _slackFacade.GetChannels(string.Empty);

            return Accepted();
        }
    }
}