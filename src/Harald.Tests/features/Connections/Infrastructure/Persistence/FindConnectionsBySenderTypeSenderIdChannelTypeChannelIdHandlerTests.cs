using System;
using System.Linq;
using System.Threading.Tasks;
using Harald.Tests.TestDoubles;
using Harald.WebApi.Domain;
using Harald.WebApi.Features.Connections.Domain.Model;
using Harald.WebApi.Features.Connections.Domain.Queries;
using Harald.WebApi.Features.Connections.Infrastructure.Persistence;
using Xunit;

namespace Harald.Tests.features.Connections.Infrastructure.Persistence
{
    public class FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandlerTests
    {
        // GIVEN query with ChannelType not equal to ChannelTypeSlack
        // EXPECT empty result
        [Fact]
        public async Task GIVEN_query_with_ChannelType_not_equal_to_ChannelTypeSlack_EXPECT_empty_result()
        {
            // Arrange
            var sut = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(null);
            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
                null,
                null,
                new ChannelTypeTest(),
                null
            );

            // Act 
            var results = await sut.HandleAsync(query);


            // Assert
            Assert.Empty(results);
        }

        // GIVEN query with SenderType not equal to SenderTypeCapability
        // EXPECT empty result
        [Fact]
        public async Task GIVEN_query_with_SenderType_not_equal_to_SenderTypeCapability_EXPECT_empty_result()
        {
            // Arrange
            var sut = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(null);
            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
                new SenderTypeTest(),
                null,
                null,
                null
            );

            // Act 
            var results = await sut.HandleAsync(query);


            // Assert
            Assert.Empty(results);
        }

        // GIVEN query with ChannelId set
        // EXPECT only capabilities with given id
        [Fact]
        public async Task GIVEN_query_with_ChannelId_set_EXPECT_only_capabilities_with_given_id()
        {
            // Arrange
            var idOfWantedChannels = "theChannelWeWant";
            var capabilityRepository = new StubCapabilityRepository();
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "foo",
                idOfWantedChannels,
                "slackUserGroupId"
            ));
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "bar",
                idOfWantedChannels,
                "slackUserGroupId"
            ));
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "sheep",
                "NotTheChannelWeWant",
                "slackUserGroupId"
            ));

            var sut = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(capabilityRepository);

            var channelIdOfWantedChannel = new ChannelId(idOfWantedChannels);
            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
                new SenderTypeCapability(),
                null,
                new ChannelTypeSlack(),
                channelIdOfWantedChannel
            );

            // Act
            var results = await sut.HandleAsync(query);

            // Assert
            Assert.All(
                results,
                connection =>
                    Assert.Equal(channelIdOfWantedChannel, connection.ChannelId)
            );
        }

        // GIVEN query with SenderId set
        // EXPECT only capabilities with given id
        [Fact]
        public async Task GIVEN_query_with_SenderId_set_EXPECT_only_capabilities_with_given_id()
        {
            // Arrange
            var idOfWantedSender = Guid.NewGuid();
            var capabilityRepository = new StubCapabilityRepository();
            await capabilityRepository.Add(Capability.Create(
                idOfWantedSender,
                "foo",
                "slackChannelId",
                "slackUserGroupId"
            ));
            await capabilityRepository.Add(Capability.Create(
                idOfWantedSender,
                "bar",
                "slackChannelId",
                "slackUserGroupId"
            ));
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "sheep",
                "slackChannelId",
                "slackUserGroupId"
            ));

            var sut = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(capabilityRepository);

            var senderIdOfWantedSender = new SenderId(idOfWantedSender.ToString());
            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
                new SenderTypeCapability(),
                senderIdOfWantedSender,
                new ChannelTypeSlack(),
                null
            );


            // Act
            var results = await sut.HandleAsync(query);

            // Assert
            Assert.All(
                results,
                connection =>
                    Assert.Equal(senderIdOfWantedSender, connection.SenderId)
            );
        }

        // GIVEN empty query
        // EXPECT connections for all capabilities
        [Fact]
        public async Task GIVEN_empty_query_EXPECT_connections_for_all_capabilities()
        {
            // Arrange
            var capabilityRepository = new StubCapabilityRepository();
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "foo",
                "slackChannelId",
                "slackUserGroupId"
            ));
            await capabilityRepository.Add(Capability.Create(
                Guid.NewGuid(),
                "bar",
                "slackChannelId",
                "slackUserGroupId"
            ));


            var sut = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler(capabilityRepository);

            var query = new FindConnectionsBySenderTypeSenderIdChannelTypeChannelId(
                null,
                null,
                null,
                null
            );

            // Act
            var results = await sut.HandleAsync(query);
            Assert.All(
                capabilityRepository.GetAll().Result,
                capability =>
                    results.Single(r =>
                        r.SenderId.ToString() == capability.Id.ToString()
                    )
            );
        }
    }
}