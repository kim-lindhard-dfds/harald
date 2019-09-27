using Harald.WebApi.Features.Connections.Domain.Model;
using Xunit;

namespace Harald.Tests.features.Connections.Domain.Model
{
    public class ChannelTypeTests
    {
        [Fact]
        public void Create_will_tell_you_valid_types_WHEN_given_a_wrong_type_input()
        {
            // Arrange
            var name = "ThisIsNotAChannelType";


            // Act
            var exception = Record.Exception(() => ChannelType.Create(name));


            // Assert
            var startText = "Your options are: '";
            var startPosition = exception.Message.LastIndexOf(startText) + startText.Length + 1;
            var endPosition = exception.Message.LastIndexOf("'");
            var lengthOfOptions = endPosition - startPosition;

            Assert.False(lengthOfOptions < 1);
        }
        
        
        [Fact]
        public void Create_will_return_a_ChannelTypeSlack_WHEN_given_string_slack()
        {
            // Arrange
            var name = "sLaCk";


            // Act / Assert
            ChannelType.Create(name);
        }
    }
}