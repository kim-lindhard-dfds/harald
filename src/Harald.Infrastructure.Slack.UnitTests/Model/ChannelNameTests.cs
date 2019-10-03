using System;
using System.Linq;
using Harald.Infrastructure.Slack.Model;
using Xunit;

namespace Harald.Infrastructure.Slack.UnitTests.Model
{
    public class ChannelNameTests
    {
        [Fact]
        public void FixChannelNameForSlack_PascalCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "PascalCasing";

            // Act
            var fixedChannelName = new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CamelCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "camelCasing";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "lowercase";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_DashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "dash-separated";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CharAndNumbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "Numbers123";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CharsAndNumbersDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "Num123-MoreNumbers456";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndCharsDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123Numbers-456MoreNumbers";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCaseAndNumbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "numbers123";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCaseAndNumbersDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "numbers123-moreNumbers456";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndLowerCaseDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123numbers-456moreNumbers";
            

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_Numbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndLowerCaseUnderscoreSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123numbers_456moreNumbers";

            // Act
            var fixedChannelName =  new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }
        
        
        [Fact]
        public void FixChannelNameForSlack_NameWithMultipleUpperCaseLetters_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "TestNoDoubleNSCreation";

            // Act
            var fixedChannelName = new SlackChannelName(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }


        [Fact]
        public void FixChannelNameForSlack_NameWithLength100_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelNameOfLength100 = string.Join("", Enumerable
                .Repeat(0, 100)
                .Select(n => 
                        (char)new Random().Next(97,123)
                    )
                );
            
            // Act
            var fixedChannelName = new SlackChannelName(channelNameOfLength100);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
            
            
        }

        private void AssertValidSlackChannelName(string name)
        {
            Assert.True(name.Length <= 80);
            AssertValidSlackName(name);
        }

        private void AssertValidSlackName(string name)
        {
            var lowerCaseName = name.ToLower();

            Assert.Equal(lowerCaseName, name);
            Assert.DoesNotContain("_", name);
        }
    }
}
