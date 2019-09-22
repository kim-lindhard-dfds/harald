using Harald.WebApi.Domain;

namespace Harald.WebApi.Features.Connections.Domain.Model
{
    public class Connection
    {
        public Connection(
            SenderType senderType,
            SenderName senderName,
            SenderId senderId, 
            ChannelType channelType,
            ChannelName channelName,
            ChannelId channelId
        )
        {
            SenderType = senderType;
            SenderId = senderId;
            SenderName = senderName;
            ChannelType = channelType;
            ChannelName = channelName;
            ChannelId = channelId;
        }

        public SenderType SenderType { get; }
        public SenderId SenderId { get; }
        public SenderName SenderName { get; }
        public ChannelType ChannelType { get; }
        public ChannelName ChannelName { get; }
        public ChannelId ChannelId { get; }
    }
}