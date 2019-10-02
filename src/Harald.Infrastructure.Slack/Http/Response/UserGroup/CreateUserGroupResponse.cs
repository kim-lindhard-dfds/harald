using Harald.Infrastructure.Slack.Dto;
using System.Net;

namespace Harald.Infrastructure.Slack.Http.Response.UserGroup
{
    public class CreateUserGroupResponse : SlackResponse
    {
        public UserGroupDto UserGroup { get; set; }
    }
}