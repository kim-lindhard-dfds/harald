using System.Collections.Generic;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;

namespace Harald.WebApi.Features.Connections.Domain.Queries
{
    public class FindConnectionsBySenderTypeSenderIdChannelTypeChannelId : IQuery<IEnumerable<Connection>>
    {
        public SenderType SenderType { get; set; }
        public SenderId SenderId { get; set; }
        public ChannelType ChannelType { get; set; }
        public ChannelId ChannelId { get; set; }

        public FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(){}
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