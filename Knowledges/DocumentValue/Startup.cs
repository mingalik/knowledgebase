using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentValue
{
    using System.Reflection;

    using DocumentValueWebApi.Consumers;

    using MassTransit;

    using MediatR;

    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
            ConfigureMassTransit(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var massTransitsSection = Configuration.GetSection("MassTransit");
            var url = massTransitsSection.GetValue<string>("Url");
            var port = massTransitsSection.GetValue<string>("Port");
            var userName = massTransitsSection.GetValue<string>("UserName");
            var password = massTransitsSection.GetValue<string>("Password");

            services.AddMassTransit(a =>
                {
                    a.AddBus(
                        busFactory =>
                            {
                                var bus = Bus.Factory.CreateUsingRabbitMq(
                                    cfg =>
                                        {
                                            cfg.Host(
                                                $"amqp://{url}",
                                                configurator =>
                                                    {
                                                        configurator.Username(userName);
                                                        configurator.Password(password);
                                                    });
                                            cfg.ConfigureEndpoints(busFactory, KebabCaseEndpointNameFormatter.Instance);
                                            cfg.UseJsonSerializer();
                                            cfg.ReceiveEndpoint(
                                        "DocumentActualized",
                                        cfg => cfg.Consumer<DocumentActualizedConsumer>());
                                        });
                                return bus;
                            });
                });
            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                                                 {
                                                     Title = "knowledge - DocumentValue HTTP API",
                                                     Version = "v1",
                                                     Description = "The Document Value Service HTTP API"
                                                 });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => c.RouteTemplate = "/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/v1/swagger.json", "IntegrationGateway");
                });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
