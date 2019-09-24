using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;
using Harald.WebApi.Features.Connections.Domain.Queries;
using Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api
{
    [Route("api/v1/connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly IQueryHandler<FindConnectionsBySenderTypeSenderIdChannelTypeChannelId, IEnumerable<Connection>>
            _findConnectionsBySenderTypeSenderIdChannelTypeChannelIdQueryHandler;

        public ConnectionsController(
            IQueryHandler<FindConnectionsBySenderTypeSenderIdChannelTypeChannelId, IEnumerable<Connection>>
                findConnectionsBySenderTypeSenderIdChannelTypeChannelIdQueryHandler)
        {
            _findConnectionsBySenderTypeSenderIdChannelTypeChannelIdQueryHandler =
                findConnectionsBySenderTypeSenderIdChannelTypeChannelIdQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnections(
            [FromQuery] string senderType,
            [FromQuery] string senderId,
            [FromQuery] string channelType,
            [FromQuery] string channelId
        )
        {
            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
            );
            if (senderType != null)
            {
                query.SenderType = SenderType.Create(senderType);
            }

            if (senderId != null)
            {
                query.SenderId = SenderId.Create(senderId);
            }

            if (channelType != null)
            {
                query.ChannelType = ChannelType.Create(channelType);
            }

            if (channelId != null)
            {
                query.ChannelId = ChannelId.Create(channelId);
            }

            IEnumerable<Connection> connections;
            try
            {
                connections =
                    await _findConnectionsBySenderTypeSenderIdChannelTypeChannelIdQueryHandler.HandleAsync(query);
            }
            catch (ValidationException validationException)
            {
                return StatusCode(
                    (int)HttpStatusCode.UnprocessableEntity, 
                    new {message = validationException.MessageToUser}
                );
            }


            var connectionDtos = connections.Select(ConnectionDto.CreateFromConnection);

            return Ok(new ItemsEnvelope<ConnectionDto>(connectionDtos));
        }
    }
}