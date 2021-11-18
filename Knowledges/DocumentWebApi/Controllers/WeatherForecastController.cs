using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentWebApi.Controllers
{
    using System.Threading.Tasks;

    using Contracts;

    using MassTransit;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        private readonly IRequestClient<DocumentLinkCreateRequest> _requestClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPublishEndpoint publishEndpoint, IRequestClient<DocumentLinkCreateRequest> requestClient)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var rng = new Random();
            await _publishEndpoint.Publish<IDocumentActualized>(
                new DocumentActualized() { DocumentId = Guid.NewGuid(), IsActual = true });

            var result = await _requestClient.GetResponse<DocumentLinkCreateResponse>(
                             new DocumentLinkCreateRequest()
                                 {
                                     DocumentId = Guid.NewGuid(), LyfeCycleTemplateId = Guid.NewGuid()
                                 }).ConfigureAwait(false);
            return Enumerable.Range(1, 5).Select(
                index => new WeatherForecast
                             {
                                 Date = DateTime.Now.AddDays(index),
                                 TemperatureC = rng.Next(-20, 55),
                                 Summary = Summaries[rng.Next(Summaries.Length)]
                             }).ToArray();
        }
    }
}
