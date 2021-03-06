﻿using System;
using System.Linq;
using System.Net;
using Harald.Infrastructure.Slack;
using Harald.WebApi.Application.EventHandlers;
using Harald.WebApi.Domain;
using Harald.WebApi.Domain.Events;
using Harald.WebApi.Enablers.KafkaMessageConsumer.Configuration;
using Harald.WebApi.Enablers.Metrics.Configuration;
using Harald.WebApi.Enablers.PrometheusHealthCheck.Configuration;
using Harald.WebApi.Enablers.SlackHealthCheck;
using Harald.WebApi.Features.Connections.Configuration;
using Harald.WebApi.Infrastructure.Messaging;
using Harald.WebApi.Infrastructure.Persistence;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prometheus;
using JsonSerializer = Harald.WebApi.Infrastructure.Serialization.JsonSerializer;

namespace Harald.WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddOptions();
            services.Configure<SlackOptions>(Configuration);
            
            var connectionString = Configuration.GetConnectionString("HaraldDbContext") ?? Configuration["HARALD_DATABASE_CONNECTIONSTRING"];

            AddRepositories(services);
            AddSlackServices(services);

            services.AddTransient<JsonSerializer>();

            services.AddTransient<ISlackService, SlackService>();

            ConfigureDomainEvents(services);

            services.AddConnectionDependencies();
            services.AddKafkaMessageConsumer();
            AddMetricServices(services);
            
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddCheck<SlackHealthProbe>(name: "slack", tags: new [] {"backing services", "slack"})
                .AddNpgSql(connectionString, tags: new[] {"backing services", "postgres"});

            services.AddSwaggerDocument();
        }

        protected virtual void AddRepositories(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("HaraldDbContext") ?? Configuration["HARALD_DATABASE_CONNECTIONSTRING"];

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaraldDbContext>((serviceProvider, options) => { options.UseNpgsql(connectionString); });

            services.AddTransient<ICapabilityRepository, CapabilityEntityFrameworkRepository>();
        }

        protected virtual void AddSlackServices(IServiceCollection services)
        {
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
        }

        protected virtual void AddMetricServices(IServiceCollection services)
        {
            var shouldStartMetricHostedService = Configuration["HARALD_START_METRIC_SERVER"] != "false";
         
            services.AddMetrics(shouldStartMetricHostedService);
        }

        private static void ConfigureDomainEvents(IServiceCollection services)
        {
            services.AddTransient<IEventHandler<CapabilityCreatedDomainEvent>, SlackCapabilityCreatedDomainEventHandler>();
            services.AddTransient<IEventHandler<CapabilityDeletedDomainEvent>, CapabilityDeletedEventNotifyDedHandler>();
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
                .Register<CapabilityDeletedDomainEvent>(
                    eventName: "capability_deleted",
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

            services.AddSingleton(eventRegistry);

            services.AddTransient<IEventDispatcher, EventDispatcher>();

            services.AddScoped<ExternalEventMetaDataStore>();
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpMetrics();

            app.UseHealthChecks("/health");
            
            // Same as above, but in JSON and more descriptive/expanded
            app.UseHealthChecks("/health-json", new HealthCheckOptions()
            {
                ResponseWriter = async (HttpContext httpContext, HealthReport result) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("status", result.Status.ToString()),
                        new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString()),
                                new JProperty("description", pair.Value.Description),
                                new JProperty("data", new JObject(pair.Value.Data.Select(
                                    p => new JProperty(p.Key, p.Value))))))))));
                    await httpContext.Response.WriteAsync(
                        json.ToString(Formatting.Indented));
                }
            });

            app.UseMvc();

            app.UsePrometheusHealthCheck();
        }
    }
}