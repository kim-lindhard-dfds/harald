using System.Collections.Generic;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;

namespace Harald.WebApi.Features.Connections.Domain.Queries
{
    public class FindConnectionsBySenderTypeSenderIdChannelTypeChannelId : IQuery<IEnumerable<Connection>>
    {
        public SenderType SenderType { get; }
        public SenderId SenderId { get; }
        public ChannelType ChannelType { get; }
        public ChannelId ChannelId { get; }
        
        public FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
            SenderType senderType, 
            SenderId senderId, 
            ChannelType channelType, 
            ChannelId channelId
        )
        {
            SenderType = senderType;
            SenderId = senderId;
            ChannelType = channelType;
            ChannelId = channelId;
        }
            
    }
}