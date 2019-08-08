namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class LookupUserResponse : GeneralResponse
    {
        public User User { get; set; }
    }
    
    public class User
    {
        public string Id { get; set; }
    }
}