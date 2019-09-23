using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class UpdateUserGroupUsersRequest : SlackRequest
    {
        public UpdateUserGroupUsersRequest(string userGroupId, IEnumerable<string> users)
        {
            var usersList = string.Join(",", users);
            var serializedContent = JsonConvert.SerializeObject(new { Usergroup = userGroupId, users = usersList }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/usergroups.users.update", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
