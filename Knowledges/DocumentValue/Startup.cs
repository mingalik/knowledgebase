using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentValue
{
    using DocumentValueWebApi.Consumers;

    using global::MassTransit;
    using global::MassTransit.Definition;

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
                                            /* Один из вариантов регистрирования консюмера
                                             cfg.ReceiveEndpoint(
                                        "DocumentActualized",
                                        cfg => cfg.Consumer<DocumentActualizedConsumer>());*/
                                        });
                                return bus;
                            });
                    a.AddConsumer<DocumentActualizedConsumer>(typeof(DocumentActualizeConsumerDefinition));
                });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
