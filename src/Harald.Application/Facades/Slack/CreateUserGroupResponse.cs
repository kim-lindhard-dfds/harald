namespace Harald.Application.Facades.Slack
{
    public class CreateUserGroupResponse
    {
        public bool Ok { get; set; }
        public UserGroup UserGroup { get; set; }
        public string Error { get; set; }
    }

    public class UserGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Handle { get; set; }
    }
}