using Harald.Tests.Builders;
using Harald.WebApi.Domain.Events;
using Xunit;

namespace Harald.Tests.Domain
{
    public class AWSContextAccountCreatedDomainEventTests
    {
        [Fact]
        public void can_create_valid_AWSContextAccountCreatedDomainEvent_when_given_GeneralDomainEvent()
        {
            // Arrange
            var generalDomainEvent = DomainEventBuilder.BuildAWSContextAccountCreatedEventData();
            
            // Act 
            var awsContextAccountCreatedDomainEvent = new AWSContextAccountCreatedDomainEvent(generalDomainEvent);
            
            
            // Assert
            Assert.Equal(DomainEventBuilder.ContextId, awsContextAccountCreatedDomainEvent.Data.ContextId);
            Assert.Equal(DomainEventBuilder.RoleArn, awsContextAccountCreatedDomainEvent.Data.RoleArn);
            Assert.Equal(DomainEventBuilder.RoleEmail, awsContextAccountCreatedDomainEvent.Data.RoleEmail);
            Assert.Equal(DomainEventBuilder.AccountId, awsContextAccountCreatedDomainEvent.Data.AccountId);
        }
    }
}