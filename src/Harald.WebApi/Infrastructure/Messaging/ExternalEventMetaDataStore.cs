namespace Harald.WebApi.Infrastructure.Messaging
{
    public class ExternalEventMetaDataStore
    {
        public string Version { get; private set; }
        public string EventName { get; private set; }
        public string XCorrelationId { get; private set; }
        public string XSender { get; private set; }

        public void Store(
            string version,
            string eventName,
            string xCorrelationId,
            string xSender
        )
        {
            Version = version;
            EventName = eventName;
            XCorrelationId = xCorrelationId;
            XSender = xSender;
        }
    }
}