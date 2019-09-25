using System;
using System.Collections.Generic;
using System.Linq;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Harald.WebApi.Infrastructure.Serialization;
using Xunit;

namespace Harald.Tests.Infrastructure.Facades.Slack
{
    public class ChannelNameTests
    {
        [Fact]
        public void FixChannelNameForSlack_PascalCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "PascalCasing";

            // Act
            var fixedChannelName = ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CamelCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "camelCasing";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCasing_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "lowercase";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_DashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "dash-separated";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CharAndNumbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "Numbers123";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_CharsAndNumbersDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "Num123-MoreNumbers456";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndCharsDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123Numbers-456MoreNumbers";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCaseAndNumbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "numbers123";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_LowerCaseAndNumbersDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "numbers123-moreNumbers456";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndLowerCaseDashSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123numbers-456moreNumbers";
            

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_Numbers_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }

        [Fact]
        public void FixChannelNameForSlack_NumbersAndLowerCaseUnderscoreSeparated_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "123numbers_456moreNumbers";

            // Act
            var fixedChannelName =  ChannelName.Create(channelName);

            // Assert
            AssertValidSlackChannelName(fixedChannelName);
        }
        
        
        [Fact]
        public void FixChannelNameForSlack_NameWithMultipleUpperCaseLetters_ReturnsValidSlackChannelName()
        {
            // Arrange
            var channelName = "TestNoDoubleNSCreation";

            // Act
            var fixedChannelName = ChannelName.Create(channelName);

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
            var fixedChannelName = ChannelName.Create(channelNameOfLength100);

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
