using System.Collections.Generic;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;

namespace Harald.WebApi.Features.Connections.Domain.Queries
{
    public class FindConnectionsByClientTypeClientIdChannelTypeChannelId : IQuery<IEnumerable<Connection>>
    {
        public ClientType ClientType { get; set; }
        public ClientId ClientId { get; set; }
        public ChannelType ChannelType { get; set; }
        public ChannelId ChannelId { get; set; }

        public FindConnectionsByClientTypeClientIdChannelTypeChannelId(){}
        public FindConnectionsByClientTypeClientIdChannelTypeChannelId(
            ClientType clientType, 
            ClientId clientId, 
            ChannelType channelType, 
            ChannelId channelId
        )
        {
            ClientType = clientType;
            ClientId = clientId;
            ChannelType = channelType;
            ChannelId = channelId;
        }
            
    }
}