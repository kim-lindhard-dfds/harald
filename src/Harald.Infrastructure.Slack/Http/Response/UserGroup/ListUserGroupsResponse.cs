using Harald.Infrastructure.Slack.Dto;
using System.Collections.Generic;

namespace Harald.Infrastructure.Slack.Http.Response.UserGroup
{
    public class ListUserGroupsResponse : SlackResponse
    {
        public List<UserGroupDto> UserGroups { get; set; }
    }
}