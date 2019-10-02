using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Harald.Infrastructure.Slack.Http.Request.UserGroup
{
    public class CreateUserGroupRequest : SlackRequest
    {
        public CreateUserGroupRequest(string name, string handle, string description)
        {
            var serializedContent = JsonConvert.SerializeObject(new { Name = name, Handle = handle, Description = description }, _serializerSettings);

            Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            RequestUri = new System.Uri("api/usergroups.create", System.UriKind.Relative);
            Method = HttpMethod.Post;
        }
    }
}
