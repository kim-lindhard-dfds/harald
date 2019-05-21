using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Infrastructure.Messaging;
using Xunit;

namespace Harald.Tests.Messaging
{
    public class DomainEventRegistryTests
    {
        [Fact]
        public void GetTopicFor_given_registered_event_returns_expected_topic()
        {
            // Arrange
            var topic = "build.capabilities";
            var eventTypeName = "capability_created";

            var sut = new DomainEventRegistry()
            .Register<CapabilityCreatedDomainEvent>(
                eventName: eventTypeName,
                topicName: topic)
            .Register<MemberJoinedCapabilityDomainEvent>(
                eventName: "member_joined_capability",
                topicName: topic)
            .Register<MemberLeftCapabilityDomainEvent>(
                eventName: "member_left_capability",
                topicName: topic);

            // Act
            var actualTopic = sut.GetTopicFor(eventTypeName);

            // Assert
            Assert.Equal(topic, actualTopic);
        }

        [Fact]
        public void GetTopicFor_given_non_registered_event_returns_null()
        {
            // Arrange
            var topic = "build.capabilities";
            
            var sut = new DomainEventRegistry()
            .Register<CapabilityCreatedDomainEvent>(
                eventName: "capability_created",
                topicName: topic)
            .Register<MemberJoinedCapabilityDomainEvent>(
                eventName: "member_joined_capability",
                topicName: topic)
            .Register<MemberLeftCapabilityDomainEvent>(
                eventName: "member_left_capability",
                topicName: topic);

            // Act
            var actualTopic = sut.GetTopicFor("dummyEventType");

            // Assert
            Assert.Null(actualTopic);
        }

        [Fact]
        public void GetAllTopics_given_registered_topics_returns_expected_topics()
        {
            // Arrange
            var topic1 = "build.capabilities";
            var topic2 = "build.dummy";

            var sut = new DomainEventRegistry()
            .Register<CapabilityCreatedDomainEvent>(
                eventName: "eventDummyName1",
                topicName: topic1)
            .Register<MemberJoinedCapabilityDomainEvent>(
                eventName: "eventDummyName2",
                topicName: topic2);

            // Act
            var actualTopic = sut.GetAllTopics();

            // Assert
            Assert.Equal(2, actualTopic.Count());
            Assert.Contains(topic1, actualTopic);
            Assert.Contains(topic2, actualTopic);
        }

        [Fact]
        public void GetInstanceTypeFor_given_registered_event_returns_expected_type()
        {
            // Arrange
            var topic = "build.capabilities";
            var eventTypeName = "capability_created";

            var sut = new DomainEventRegistry()
            .Register<CapabilityCreatedDomainEvent>(
                eventName: eventTypeName,
                topicName: topic)
            .Register<MemberJoinedCapabilityDomainEvent>(
                eventName: "member_joined_capability",
                topicName: topic)
            .Register<MemberLeftCapabilityDomainEvent>(
                eventName: "member_left_capability",
                topicName: topic);

            // Act
            var type = sut.GetInstanceTypeFor(eventTypeName);

            // Assert
            Assert.Equal(typeof(CapabilityCreatedDomainEvent), type);
        }

        [Fact]
        public void GetInstanceTypeFor_given_non_registered_event_throws_expected_exception()
        {
            // Arrange
            var topic = "build.capabilities";
            var eventTypeName = "dummyEventName";

            var sut = new DomainEventRegistry()
            .Register<CapabilityCreatedDomainEvent>(
                eventName: "capability_created",
                topicName: topic)
            .Register<MemberJoinedCapabilityDomainEvent>(
                eventName: "member_joined_capability",
                topicName: topic)
            .Register<MemberLeftCapabilityDomainEvent>(
                eventName: "member_left_capability",
                topicName: topic);

            // Act and assert
            Exception ex = Assert.Throws<MessagingException>(() => sut.GetInstanceTypeFor(eventTypeName));
            Assert.Contains("Error! Could not determine \"event instance type\"", ex.Message);
        }
    }
}