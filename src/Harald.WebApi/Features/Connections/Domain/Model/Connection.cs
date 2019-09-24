using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class Connection
    {
        public Connection(
            ClientType clientType,
            ClientName clientName,
            ClientId clientId, 
            ChannelType channelType,
            ChannelName channelName,
            ChannelId channelId
        )
        {
            ClientType = clientType;
            ClientId = clientId;
            ClientName = clientName;
            ChannelType = channelType;
            ChannelName = channelName;
            ChannelId = channelId;
        }

        public ClientType ClientType { get; }
        public ClientId ClientId { get; }
        public ClientName ClientName { get; }
        public ChannelType ChannelType { get; }
        public ChannelName ChannelName { get; }
        public ChannelId ChannelId { get; }
    }
}