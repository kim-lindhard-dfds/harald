using Harald.WebApi.Features.Connections.Domain.Model;

namespace Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api.Model
{
    public class ConnectionDto
    {
        public string SenderType { get; set; }
        
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string ChannelType { get; set; }
        
        public string ChannelName { get; set; }
        public string ChannelId { get; set; }

        public static ConnectionDto CreateFromConnection(Connection connection)
        {
            return new ConnectionDto
            {
                SenderType = connection.SenderType,
                SenderName =  connection.SenderName,
                SenderId = connection.SenderId,
                ChannelType = connection.ChannelType,
                ChannelName = connection.ChannelName,
                ChannelId = connection.ChannelId
            };
        }
    }
}