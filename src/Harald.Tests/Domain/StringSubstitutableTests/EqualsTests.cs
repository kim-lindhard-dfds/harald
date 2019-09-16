using System;
using Xunit;

namespace Harald.Tests.Domain.StringSubstitutableTests
{
    public class EqualsTests
    {
        [Fact]
        public void GIVEN_not_same_not_string_EXPECT_false()
        {
            // Arrange
            var testStringSubstitutable = new TestStringSubstitutable("");
            var @object = new Object();
            
            // Act
            var equals = testStringSubstitutable.Equals(@object);
            
            // Assert
            Assert.False(equals);
        }
        
        [Fact]
        public void GIVEN_same_text_EXPECT_true()
        {
            // Arrange
            var testStringSubstitutable1 = new TestStringSubstitutable("foo");
            var testStringSubstitutable2 = new TestStringSubstitutable("foo");
            
            // Act
            var equals = testStringSubstitutable1.Equals(testStringSubstitutable2);
            
            // Assert
            Assert.True(equals);
        }
        
        [Fact]
        public void GIVEN_string_with_same_text_EXPECT_true()
        {
            // Arrange
            var testStringSubstitutable1 = new TestStringSubstitutable("foo");
            var @string = "foo";
            
            // Act
            var equals = testStringSubstitutable1.Equals(@string);
            
            // Assert
            Assert.True(equals);
        }
    }
}