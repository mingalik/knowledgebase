namespace DocumentValueWebApi.Controllers
{
    using System.Threading.Tasks;

    using DocumentValueWebApi.Application.Commands;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class DocumentValueControllerV2 : ControllerBase
    {
        private readonly IMediator mediator;

        public DocumentValueControllerV2(IMediator mediator) => this.mediator = mediator;

        // POST: DocumentControllerV2/Create
        [HttpPost]
        public async Task<bool> Create([FromBody] DocumentValueCreateCommand command) => await mediator.Send(command, HttpContext.RequestAborted).ConfigureAwait(false);
    }
}
