using System;
using System.Threading.Tasks;
using Harald.WebApi.features.connections.Domain;
using Harald.WebApi.features.connections.Domain.Model;
using Harald.WebApi.features.connections.Infrastructure.driving.api.model;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.features.connections.Infrastructure.driving.api
{
    [Route("api/v1/connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetConnections(
            [FromQuery] string senderType,
            [FromQuery] string senderId,
            [FromQuery] string channelType,
            [FromQuery] string channelId
        )
        {
            var connections = new[]
            {
                 ConnectionDto.CreateFromConnection(new Connection(
                    new SenderTypeCapability(),
                    new SenderId(senderId ?? Guid.NewGuid().ToString().Substring(0,5)),
                    new ChannelTypeSlack(),
                    new ChannelId(channelId ?? Guid.NewGuid().ToString().Substring(0,5))
                )),
                 ConnectionDto.CreateFromConnection(new Connection(
                     new SenderTypeCapability(),
                     new SenderId(senderId ?? Guid.NewGuid().ToString().Substring(0,5)),
                     new ChannelTypeSlack(),
                     new ChannelId(channelId ?? Guid.NewGuid().ToString().Substring(0,5))
                 )),
                 ConnectionDto.CreateFromConnection(new Connection(
                     new SenderTypeCapability(),
                     new SenderId(senderId ?? Guid.NewGuid().ToString().Substring(0,5)),
                     new ChannelTypeSlack(),
                     new ChannelId(channelId ?? Guid.NewGuid().ToString().Substring(0,5))
                 ))
            };


            return Ok(connections);
        }
    }
}