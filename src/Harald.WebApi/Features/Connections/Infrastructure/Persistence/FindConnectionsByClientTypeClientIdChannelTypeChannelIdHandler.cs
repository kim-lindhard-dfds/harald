using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;
using Harald.WebApi.Features.Connections.Domain.Queries;

namespace Harald.WebApi.Features.Connections.Infrastructure.Persistence
{
    public class FindConnectionsByClientTypeClientIdChannelTypeChannelIdHandler : IQueryHandler<
        FindConnectionsByClientTypeClientIdChannelTypeChannelId, IEnumerable<Connection>>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        public FindConnectionsByClientTypeClientIdChannelTypeChannelIdHandler(
            ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        public async Task<IEnumerable<Connection>> HandleAsync(
            FindConnectionsByClientTypeClientIdChannelTypeChannelId query)
        {
            if (query.ChannelType != null && !query.ChannelType.IsEmpty && !(query.ChannelType is ChannelTypeSlack))
            {
                return Enumerable.Empty<Connection>();
            }

            if (query.ClientType != null && !query.ClientType.IsEmpty && !(query.ClientType is ClientTypeCapability))
            {
                return Enumerable.Empty<Connection>();
            }

            var capabilities = await _capabilityRepository.GetAll();

            if (query.ChannelId != null && !query.ChannelId.IsEmpty)
            {
                capabilities = capabilities
                    .Where(c =>
                        c.SlackChannelId.Equals(query.ChannelId)
                    );
            }

            if (query.ClientId != null && !query.ClientId.IsEmpty)
            {
                Guid capabilityId;
                var capabilityIdIsValid = Guid.TryParse(query.ClientId, out capabilityId);
                if(capabilityIdIsValid == false)
                {
                    throw new ValidationException($"The given capability id: '{query.ClientId}' is not valid. Expected format is Guid with 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                }
                
                capabilities = capabilities
                    .Where(c =>
                        c.Id.Equals(capabilityId));
            }

            var connections = capabilities.Select(ConvertCapabilityToConnection);

            return connections;
        }

        private Connection ConvertCapabilityToConnection(Capability capability)
        {
            return new Connection(
                new ClientTypeCapability(),
                new ClientName(capability.Name), 
                new ClientId(capability.Id.ToString()),
                new ChannelTypeSlack(),
                ChannelName.Create(capability.Name),
                new ChannelId(capability.SlackChannelId)
            );
        }
    }
}