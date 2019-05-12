using System;
using Harald.WebApi.Domain.Events;
using Newtonsoft.Json.Linq;

namespace Harald.Tests.Builders
{
    public class DomainEventBuilder
    {
        public static Guid ContextId { get; set; } = Guid.NewGuid();
        public static string AccountId { get; set; } = "1234567890";
        public static string RoleArn { get; set; } = "arn:aws:iam::1234567890:role/pax-bookings-A43aS";
        public static string RoleEmail { get; set; } = "aws.pax-bookings-a43as@dfds.com";


        public static GeneralDomainEvent BuildAWSContextAccountCreatedEventData()
        {

            dynamic data = new JObject();
            data.contextId = ContextId;
            data.accountId = AccountId;
            data.roleArn = RoleArn;
            data.roleEmail = RoleEmail;

            return BuildGeneralDomainEvent("aws_context_account_created", data);
        }

        private static GeneralDomainEvent BuildGeneralDomainEvent(string type, object data)
        {
            var messageId = Guid.NewGuid();
            var generalDomainEvent = new GeneralDomainEvent(
                messageId: messageId,
                type: type,
                data: data
            );
            return generalDomainEvent;

        }

    }
}