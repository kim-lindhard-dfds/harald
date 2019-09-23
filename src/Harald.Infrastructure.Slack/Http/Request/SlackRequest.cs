using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Net.Http;

namespace Harald.Infrastructure.Slack.Http.Request
{
    public abstract class SlackRequest : HttpRequestMessage
    {
        protected readonly JsonSerializerSettings _serializerSettings;

        protected SlackRequest()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        protected SlackRequest(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }
    }
}
