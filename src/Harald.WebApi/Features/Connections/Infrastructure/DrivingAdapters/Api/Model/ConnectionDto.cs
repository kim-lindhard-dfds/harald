using Harald.WebApi.Features.Connections.Domain.Model;

namespace Harald.WebApi.Features.Connections.Infrastructure.DrivingAdapters.Api.Model
{
    public class ConnectionDto
    {
        public string ClientType { get; set; }
        
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ChannelType { get; set; }
        
        public string ChannelName { get; set; }
        public string ChannelId { get; set; }

        public static ConnectionDto CreateFromConnection(Connection connection)
        {
            return new ConnectionDto
            {
                ClientType = connection.ClientType,
                ClientName =  connection.ClientName,
                ClientId = connection.ClientId,
                ChannelType = connection.ChannelType,
                ChannelName = connection.ChannelName,
                ChannelId = connection.ChannelId
            };
        }
    }
}