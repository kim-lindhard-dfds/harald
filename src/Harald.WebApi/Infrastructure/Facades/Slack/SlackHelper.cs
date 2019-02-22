using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Harald.WebApi.Infrastructure.Serialization;

namespace Harald.WebApi.Infrastructure.Facades.Slack
{
    public class SlackHelper
    {
        public string FixChannelNameForSlack(string channelName)
        {
            // Max channel name length is 21.
            if (channelName.Length > 21)
            {
                channelName = channelName.Substring(0, 21);
            }

            var fixedChannelName = GetLowerCaseNameWithHypens(channelName);

            return fixedChannelName;
        }

        public string FixHandleNameForSlack(string handleName)
        {
            const string handleSuffix = "Members";
            handleName = handleName + handleSuffix;

            var fixedHandleName = GetLowerCaseNameWithHypens(handleName);

            return fixedHandleName;
        }

        private string GetLowerCaseNameWithHypens(string name)
        {
            // Make sure at least one word is matched below (starts with upper-case character).
            name = UppercaseFirstCharacter(name);

            var regExPattern = string.Empty;

            // Only dash is allowed for channel/handle names, replace underscore with dash.
            if (name.Contains("_"))
            {
                name = name.Replace("_", "-");
            }

            if (name.Contains("-"))
            {
                regExPattern = @"[a-zA-Z1-9]+-[a-zA-Z1-9]+";
            }
            else
            {
                regExPattern = @"([A-Z1-9][a-z1-9]+)";
            }

            var lowerCaseWords = Regex.Matches(name, regExPattern)
            .Cast<Match>()
            .Select(m => m.Value.ToLower());

            var lowerCaseNameWithHypens = string.Join("-", lowerCaseWords);

            return lowerCaseNameWithHypens;
        }

        private string UppercaseFirstCharacter(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}