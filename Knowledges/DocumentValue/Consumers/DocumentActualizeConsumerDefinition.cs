namespace DocumentValueWebApi.Consumers
{
    using MassTransit.Definition;

    public class DocumentActualizeConsumerDefinition : ConsumerDefinition<DocumentActualizedConsumer>
    {
        public DocumentActualizeConsumerDefinition()
        {
            EndpointName = "QueueName";
        }
    }
}
