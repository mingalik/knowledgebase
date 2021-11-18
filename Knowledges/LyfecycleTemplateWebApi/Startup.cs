using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfecycleTemplateWebApi
{
    using global::MassTransit;
    using global::MassTransit.Definition;

    using LyfecycleTemplateWebApi.Consumers;

    using MassTransit.Definition;

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
                                        });
                                return bus;
                            });
                    a.AddConsumer<DocumentLinkCreateConsumer>();
                });

            services.AddMassTransitHostedService();
        }
    }
}
