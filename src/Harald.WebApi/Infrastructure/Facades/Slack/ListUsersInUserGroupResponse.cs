using System.Collections.Generic;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class ListUsersInUserGroupResponse : GeneralResponse
    {
        public List<string> Users { get; set; }

        public ListUsersInUserGroupResponse()
        {
            Users = new List<string>();
        }
    }
}