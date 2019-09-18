namespace Harald.WebApi.features.connections.Domain.Model
{
    public class Connection
    {
        public Connection(
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

        public SenderType SenderType { get; }
        public SenderId SenderId { get; }
        public ChannelType ChannelType { get; }
        public ChannelId ChannelId { get; }
    }
}