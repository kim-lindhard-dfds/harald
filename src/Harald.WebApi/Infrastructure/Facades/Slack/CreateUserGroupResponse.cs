namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class CreateUserGroupResponse : GeneralResponse
    {
        public UserGroup UserGroup { get; set; }
    }

    public class UserGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Handle { get; set; }
    }
}