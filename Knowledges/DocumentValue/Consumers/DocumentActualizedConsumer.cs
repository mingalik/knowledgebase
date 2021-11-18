namespace DocumentValueWebApi.Consumers
{
    using System.Threading.Tasks;

    using Contracts;

    using MassTransit;

    public class DocumentActualizedConsumer : IConsumer<IDocumentActualized>
    {
        public Task Consume(ConsumeContext<IDocumentActualized> context)
        {
           return Task.CompletedTask;
        }
    }
}
