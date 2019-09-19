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
    public class FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler : IQueryHandler<
        FindConnectionsBySenderTypeSenderIdChannelTypeChannelId, IEnumerable<Connection>>
    {
        private readonly ICapabilityRepository _capabilityRepository;

        public FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(
            ICapabilityRepository capabilityRepository)
        {
            _capabilityRepository = capabilityRepository;
        }

        public async Task<IEnumerable<Connection>> HandleAsync(
            FindConnectionsBySenderTypeSenderIdChannelTypeChannelId query)
        {
            if (query.ChannelType != null && query.ChannelType.GetType() != typeof(ChannelTypeSlack) )
            {
                return Enumerable.Empty<Connection>();
            }

            if (query.SenderType != null && query.SenderType.GetType() != typeof(SenderTypeCapability))
            {
                return Enumerable.Empty<Connection>();
            }

            var capabilities = await _capabilityRepository.GetAll();

            if (query.ChannelId != null)
            {
                capabilities = capabilities
                    .Where(c =>
                        c.SlackChannelId.Equals(query.ChannelId)
                    );
            }

            if (query.SenderId != null)
            {
                capabilities = capabilities
                    .Where(c =>
                        c.Id.Equals(new Guid(query.SenderId)));
            }

            var connections = capabilities.Select(ConvertCapabilityToConnection);

            return connections;
        }

        private Connection ConvertCapabilityToConnection(Capability capability)
        {
            return new Connection(
                new SenderTypeCapability(),
                new SenderId(capability.Id.ToString()),
                new ChannelTypeSlack(),
                new ChannelId(capability.SlackChannelId)
            );
        }
    }
}