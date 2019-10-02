using Harald.Infrastructure.Slack;
using Harald.WebApi.Features.Connections.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Harald.WebApi.Domain;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly ISlackFacade _slackFacade;
        private readonly IConfiguration _configuration;

        public ChannelsController(
            ISlackFacade slackFacade, 
            IConfiguration configuration)
        {
            _slackFacade = slackFacade;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetChannels([FromQuery] string channelType)
        {
            object convertedChannelType;

            try
            {
                convertedChannelType = ChannelType.Create(channelType);
            }
            catch(ValidationException exp)
            {
                return UnprocessableEntity(new {Message = exp.MessageToUser});
            }

            switch (convertedChannelType)
            {
                case ChannelTypeSlack _:
                    return Accepted(await _slackFacade.GetChannels(_configuration["SLACK_API_AUTH_TOKEN"]));

                default:
                    return UnprocessableEntity($"Unsupported channelType {channelType}");
            }
        }
    }
}