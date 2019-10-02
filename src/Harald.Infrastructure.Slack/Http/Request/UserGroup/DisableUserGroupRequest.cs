using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class DisableUserGroupRequest : SlackRequest
    {
        public DisableUserGroupRequest(string usergroupId)
        {
            var serializedContent = JsonConvert.SerializeObject(new { usergroup = usergroupId }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/usergroups.disable", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
