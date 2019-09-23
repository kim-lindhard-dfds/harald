using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class GetUserGroupsRequest : SlackRequest
    {
        public GetUserGroupsRequest()
        {
            RequestUri = new System.Uri($"api/usergroups.list", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}