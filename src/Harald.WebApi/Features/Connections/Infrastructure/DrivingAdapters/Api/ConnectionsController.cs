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
        private readonly IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;

        public ConnectionsController(
            IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler)
        {
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler =
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnections(
            [FromQuery] string clientType,
            [FromQuery] string clientId,
            [FromQuery] string channelType,
            [FromQuery] string channelId
        )
        {
            var query = new FindConnectionsByClientTypeClientIdChannelTypeChannelId(
            );
            if (clientType != null)
            {
                query.ClientType = ClientType.Create(clientType);
            }

            if (clientId != null)
            {
                query.ClientId = ClientId.Create(clientId);
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
                    await _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler.HandleAsync(query);
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