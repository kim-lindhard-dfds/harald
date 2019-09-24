using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;
using Harald.WebApi.Features.Connections.Domain.Queries;
using Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly ISlackFacade _slackFacade;
        private readonly IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;

        public ConnectionsController(
            IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler,
                ISlackFacade slackFacade)
        {
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler =
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;

            _slackFacade = slackFacade;
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

        [HttpPost]
        public async Task<IActionResult> AddConnection(ConnectionDto connection)
        {
            if (connection.ClientId.Equals(Guid.Empty))
            {
                return BadRequest("ClientId ID is required.");
            }

            if (string.IsNullOrEmpty(connection.ClientType))
            {
                return BadRequest("ClientType is required.");
            }

            if (string.IsNullOrEmpty(connection.ChannelId))
            {
                return BadRequest("Channel ID is required.");
            }

            if (string.IsNullOrEmpty(connection.ChannelType))
            {
                return BadRequest("ChannelType is required.");
            }

            if (string.IsNullOrEmpty(connection.ChannelName))
            {
                return BadRequest("ChannelName is required.");
            }
            
            //TODO: Create connection (should we do this via repo or alternative?)

            var response = await _slackFacade.JoinChannel(connection.ChannelName);
            
            return Accepted(response.Channel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConnection(
            [FromQuery] string clientType,
            [FromQuery] string clientId,
            [FromQuery] string channelType,
            [FromQuery] string channelId)
        {
            if (clientId.Equals(Guid.Empty))
            {
                return BadRequest("ClientId ID is required.");
            }

            if (string.IsNullOrEmpty(clientType))
            {
                return BadRequest("ClientType is required.");
            }

            var getMatchedConnectionsQuery = new FindConnectionsByClientTypeClientIdChannelTypeChannelId(
                                            (ClientType)clientType,
                                            (ClientId)clientId,
                                            (ChannelType)channelType,
                                            (ChannelId)channelId);

            var matchedConnections = await _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler.HandleAsync(getMatchedConnectionsQuery);

            foreach (var connection in matchedConnections)
            {
                await _slackFacade.LeaveChannel(connection.ChannelId.ToString());

                var getAllChannelConnectionsQuery = new FindConnectionsByClientTypeClientIdChannelTypeChannelId(
                                            null,
                                            null,
                                            (ChannelType)channelType,
                                            (ChannelId)channelId);

                var allChannelConnections = await _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler.HandleAsync(getAllChannelConnectionsQuery);

                if (!allChannelConnections.Where(c => !c.ClientId.Equals(clientId)).Any())
                {
                    await _slackFacade.ArchiveChannel(connection.ChannelId.ToString());
                }
            }

            return Accepted();
        }
    }
}