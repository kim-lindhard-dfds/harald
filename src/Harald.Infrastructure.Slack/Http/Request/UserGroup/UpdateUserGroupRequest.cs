using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class UpdateUserGroupRequest : SlackRequest
    {
        public UpdateUserGroupRequest(string usergroupId, string name, string handle)
        {
            var serializedContent = JsonConvert.SerializeObject(new { Name = name, Handle = handle, usergroup = usergroupId }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/usergroups.update", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
