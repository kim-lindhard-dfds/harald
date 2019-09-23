using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request.Conversation
{
    public class GetConversationsRequest : SlackRequest
    {
        public GetConversationsRequest()
        {            
            RequestUri = new System.Uri("api/conversations.list", System.UriKind.Relative);
            Method = HttpMethod.Get;
        }
    }
}
