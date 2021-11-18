namespace LyfecycleTemplateWebApi.Consumers
{
    using System.Threading.Tasks;

    using Contracts;

    using MassTransit;

    public class DocumentLinkCreateConsumer : IConsumer<DocumentLinkCreateRequest>
    {
        public async Task Consume(ConsumeContext<DocumentLinkCreateRequest> context)
        {
            var templateid = context.Message.LyfeCycleTemplateId;

            var response = new DocumentLinkCreateResponse() { Result = true };

            await context.RespondAsync(response).ConfigureAwait(false);
        }
    }
}
