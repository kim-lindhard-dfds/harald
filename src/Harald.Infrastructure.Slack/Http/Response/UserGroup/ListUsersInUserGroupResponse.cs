using System.Collections.Generic;

namespace Harald.Infrastructure.Slack.Http.Response.UserGroup
{
    public class ListUsersInUserGroupResponse : SlackResponse
    {
        public List<string> Users { get; set; }

        public ListUsersInUserGroupResponse()
        {
            Users = new List<string>();
        }
    }
}