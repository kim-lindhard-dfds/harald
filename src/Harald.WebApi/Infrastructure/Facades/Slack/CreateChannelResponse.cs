namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class CreateChannelResponse : GeneralResponse
    {
        public Channel Channel { get; set; }
    }

    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    
}