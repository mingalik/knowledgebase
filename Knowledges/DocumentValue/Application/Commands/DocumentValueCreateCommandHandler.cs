namespace DocumentValueWebApi.Application.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    public class DocumentValueCreateCommandHandler : IRequestHandler<DocumentValueCreateCommand, bool>
        {
            private readonly IMediator _mediator;

            public DocumentValueCreateCommandHandler(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            public Task<bool> Handle(DocumentValueCreateCommand message, CancellationToken cancellationToken)
            {
                // send message to MassTransit for Mongo Handler
                // save to database
                return Task.FromResult(true);
            }
        }
}
