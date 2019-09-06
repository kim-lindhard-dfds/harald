using Xunit;

namespace Harald.Tests.Domain.StringSubstitutableTests
{
    public class ToStringTests
    {
        [Fact]
        public void ToString_Equals_Source_string()
        {
            // Arrange
            var sourceString = "a nice text";
            
            // Act 
            var stringSubstitutable = new TestStringSubstitutable(sourceString);

            // Assert
            Assert.Equal(sourceString, stringSubstitutable.ToString());
        }
    }
}