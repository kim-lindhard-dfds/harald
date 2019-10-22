using Harald.Infrastructure.Slack;
using Harald.WebApi.Features.Connections.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Controllers.Model;
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
                {
                    var channelDtos = await _slackFacade.GetChannels(_configuration["SLACK_API_AUTH_TOKEN"]);
                    
                    var connections = channelDtos.Select(c => 
                        new ChannelDto
                        {
                            Id = c.Id, 
                            Name = c.Name, 
                            Type = new ChannelTypeSlack()
                        });
                    
                    return Ok(new ItemsEnvelope<ChannelDto>(connections));
                    
                }

                default:
                    return UnprocessableEntity($"Unsupported channelType {channelType}");
            }
        }
    }
}