using Harald.WebApi.features.connections.Domain;
using Harald.WebApi.features.connections.Domain.Model;

namespace Harald.WebApi.features.connections.Infrastructure.driving.api.model
{
    public class ConnectionDto
    {
        public string SenderType { get; set; }
        public string SenderId { get; set; }
        public string ChannelType { get; set; }
        public string ChannelId { get; set; }

        public static ConnectionDto CreateFromConnection(Connection connection)
        {
            return new ConnectionDto
            {
                SenderType = connection.SenderType,
                SenderId = connection.SenderId,
                ChannelType = connection.ChannelType,
                ChannelId = connection.ChannelId
            };
        }
    }
}