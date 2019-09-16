using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Harald.WebApi.Domain
{
    public class UserGroupHandle : StringSubstitutable

    {
        private UserGroupHandle(string value) : base(value)
        {
        }

        public static UserGroupHandle Create(string source)
        {
            const string handleSuffix = "Members";
            var handleName = source + handleSuffix;

            var fixedHandleName = GetLowerCaseNameWithHypens(handleName);

            return new UserGroupHandle(fixedHandleName);
        }
        
        public static explicit operator UserGroupHandle(String input) 
        {
            return UserGroupHandle.Create(input);
        }
        
        private static string GetLowerCaseNameWithHypens(string name)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            name = char.ToUpper(name[0]) + name.Substring(1);

            // Make sure at least one word is matched below (starts with upper-case character).

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
    }
}