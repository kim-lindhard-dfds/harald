using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.User
{
    public class GetUserByEmailRequest : SlackRequest
    {
        public GetUserByEmailRequest(string email)
        {
            RequestUri = new System.Uri($"api/users.lookupByEmail?email={email}", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}
