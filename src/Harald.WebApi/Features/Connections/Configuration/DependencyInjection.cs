using System.Collections.Generic;
using Harald.WebApi.Domain.Queries;
using Harald.WebApi.Features.Connections.Domain.Model;
using Harald.WebApi.Features.Connections.Domain.Queries;
using Harald.WebApi.Features.Connections.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.WebApi.Features.Connections.Configuration
{
    public static class DependencyInjection
    {
        public static void AddConnectionDependencies(this IServiceCollection services)
        {
            services.AddTransient<
                IQueryHandler<FindConnectionsBySenderTypeSenderIdChannelTypeChannelId, IEnumerable<Connection>>,
                FindConnectionsBySenderTypeSenderIdChannelTypeChannelIdHandler
            >();
            
            
        }
    }
}