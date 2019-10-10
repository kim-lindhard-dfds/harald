﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Application;
using Harald.WebApi.Application.EventHandlers;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Features.Connections.Configuration;
using Harald.WebApi.Infrastructure.Messaging;
using Harald.WebApi.Infrastructure.Persistence;
using Harald.WebApi.Infrastructure.Serialization;
using Harald.WebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Harald.WebApi
{
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionString = Configuration["HARALD_DATABASE_CONNECTIONSTRING"];

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaraldDbContext>((serviceProvider, options) => { options.UseNpgsql(connectionString); });

            services.AddTransient<ICapabilityRepository, CapabilityEntityFrameworkRepository>();

            services.AddHttpClient<ISlackFacade, SlackFacade>(cfg =>
            {
                var baseUrl = Configuration["SLACK_API_BASE_URL"];
                if (baseUrl != null)
                {
                    cfg.BaseAddress = new Uri(baseUrl);
                }

                var authToken = Configuration["SLACK_API_AUTH_TOKEN"];
                if (authToken != null)
                {
                    cfg.DefaultRequestHeaders
                    .Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken}");
                }
            });

            services.AddTransient<JsonSerializer>();

            services.AddTransient<ISlackService, SlackService>();

            ConfigureDomainEvents(services);

            services.AddConnectionDependencies();
            
            services.AddHostedService<MetricHostedService>();
            services.AddHostedService<ConsumerHostedService>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddNpgSql(connectionString, tags: new[] { "backing services", "postgres" });

            services.AddSwaggerDocument();
        }

        private static void ConfigureDomainEvents(IServiceCollection services)
        {
            services.AddTransient<IEventHandler<CapabilityCreatedDomainEvent>, SlackCapabilityCreatedDomainEventHandler>();
            services.AddTransient<IEventHandler<MemberJoinedCapabilityDomainEvent>, SlackMemberJoinedCapabilityDomainEventHandler>();
            services.AddTransient<IEventHandler<MemberLeftCapabilityDomainEvent>, SlackMemberLeftCapabilityDomainEventHandler>();
            services.AddTransient<IEventHandler<ContextAddedToCapabilityDomainEvent>, SlackContextAddedToCapabilityDomainEventHandler>();
            services.AddTransient<IEventHandler<AWSContextAccountCreatedDomainEvent>, SlackAwsContextAccountCreatedEventHandler>();
            services.AddTransient<IEventHandler<K8sNamespaceCreatedAndAwsArnConnectedDomainEvent>, K8SNamespaceCreatedAndAwsArnConnectedDomainEventHandler>();
            services.AddTransient<EventHandlerFactory>();

            var topic = "build.selfservice.events.capabilities";

            var eventRegistry = new DomainEventRegistry()
                .Register<CapabilityCreatedDomainEvent>(
                    eventName: "capability_created",
                    topicName: topic)
                .Register<MemberJoinedCapabilityDomainEvent>(
                    eventName: "member_joined_capability",
                    topicName: topic)
                .Register<MemberLeftCapabilityDomainEvent>(
                    eventName: "member_left_capability",
                    topicName: topic)
                .Register<ContextAddedToCapabilityDomainEvent>(
                    eventName: "context_added_to_capability",
                    topicName: topic)
                .Register<AWSContextAccountCreatedDomainEvent>(
                    eventName: "aws_context_account_created",
                    topicName: topic)
                .Register<K8sNamespaceCreatedAndAwsArnConnectedDomainEvent>(
                    eventName: "k8s_namespace_created_and_aws_arn_connected",
                    topicName: topic);
           
                var serviceProvider = services.BuildServiceProvider();

          
                services.AddSingleton(eventRegistry);

            services.AddTransient<IEventDispatcher, EventDispatcher>();

            services.AddScoped<ExternalEventMetaDataStore>();
            services.AddTransient<KafkaConsumerFactory.KafkaConfiguration>();
            services.AddTransient<KafkaConsumerFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseHttpMetrics();

            app.UseMvc();
        }
    }

    public class MetricHostedService : IHostedService
    {
        private const string Host = "0.0.0.0";
        private const int Port = 8080;

        private IMetricServer _metricServer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Staring metric server on {Host}:{Port}");

            _metricServer = new KestrelMetricServer(Host, Port).Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (_metricServer)
            {
                Console.WriteLine("Shutting down metric server");
                await _metricServer.StopAsync();
                Console.WriteLine("Done shutting down metric server");
            }
        }
    }
}