using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class GetUserGroupUsersRequest : SlackRequest
    {
        public GetUserGroupUsersRequest(string userGroupId, bool includeDisabled = false)
        {
            RequestUri = new System.Uri($"api/usergroups.users.list?usergroup={userGroupId}&include_disabled={includeDisabled}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}