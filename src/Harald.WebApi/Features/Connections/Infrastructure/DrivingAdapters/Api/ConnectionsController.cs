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
        private readonly ICapabilityRepository _capabilityRepository;
        private readonly IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;

        public ConnectionsController(
            IQueryHandler<FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler,
                ISlackFacade slackFacade,
                ICapabilityRepository capabilityRepository)
        {
            _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler =
                findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler;

            _slackFacade = slackFacade;
            _capabilityRepository = capabilityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnections(
            [FromQuery] string clientType,
            [FromQuery] string clientId,
            [FromQuery] string channelType,
            [FromQuery] string channelId
        )
        {
            var query = new FindConnectionsByClientTypeClientIdChannelTypeChannelId();

            if (!string.IsNullOrEmpty(clientType))
            {
                query.ClientType = ClientType.Create(clientType);
            }

            if (!string.IsNullOrEmpty(clientId))
            {
                query.ClientId = ClientId.Create(clientId);
            }

            if (!string.IsNullOrEmpty(channelType))
            {
                query.ChannelType = ChannelType.Create(channelType);
            }

            if (!string.IsNullOrEmpty(channelId))
            {
                query.ChannelId = ChannelId.Create(channelId);
            }

            IEnumerable<Connection> connections;

            try
            {
                connections = await _findConnectionsByClientTypeClientIdChannelTypeChannelIdQueryHandler.HandleAsync(query);
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
            if (string.IsNullOrEmpty(connection.ClientId))
            {
                return BadRequest("ClientId ID is required.");
            }
            
            if (!Guid.TryParse(connection.ClientId, out var clientIdAsGuid))
            {
                return BadRequest("ClientId ID must be a Guid.");
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

            //TODO: Talk with Kim about how to infer the user group name. Should we use the slack service or can we safely use the channelname?
            var capability = Capability.Create(clientIdAsGuid, connection.ClientName, connection.ChannelId, connection.ChannelName);

            await _capabilityRepository.Add(capability);

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
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest("ClientId ID is required.");
            }

            //TODO: Discuss this Kim. Argument: If the id isnt a Guid our capability repo will fail, maybe we should just make it a guid? :)
            if (!Guid.TryParse(clientId, out var clientIdAsGuid))
            {
                return BadRequest("ClientId ID must be a Guid.");
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

                if (allChannelConnections.All(c => c.ClientId.ToString().Equals(clientId)))
                {
                    await _slackFacade.ArchiveChannel(connection.ChannelId.ToString());
                }
            }

            return Accepted();
        }
    }
}