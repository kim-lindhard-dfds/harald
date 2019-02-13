namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class CreateChannelResponse
    {
        public bool Ok { get; set; }
        public Channel Channel { get; set; }
        public string Error { get; set; }
    }

    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}