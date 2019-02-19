using Newtonsoft.Json;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class GeneralResponse
    {
        public bool Ok { get; set; }
        public string Error { get; set; }
    }
}