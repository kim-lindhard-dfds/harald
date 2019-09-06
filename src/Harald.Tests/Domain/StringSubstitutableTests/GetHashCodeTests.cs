using Xunit;

namespace Harald.Tests.Domain.StringSubstitutableTests
{
    public class GetHashCodeTests
    {
        [Fact]
        public void GetHashCode_Equals_Source_GetHashCode()
        {
            // Arrange
            var sourceString = "a nice text";
            
            // Act 
            var stringSubstitutable = new TestStringSubstitutable(sourceString);

            // Assert
            Assert.Equal(sourceString.GetHashCode(), stringSubstitutable.GetHashCode());
        }
    }
}