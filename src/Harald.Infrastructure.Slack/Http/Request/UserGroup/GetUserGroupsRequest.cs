using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class GetUserGroupsRequest : SlackRequest
    {
        public GetUserGroupsRequest(bool includeDisabled = false)
        {
            RequestUri = new System.Uri($"api/usergroups.list?include_disabled={includeDisabled}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}