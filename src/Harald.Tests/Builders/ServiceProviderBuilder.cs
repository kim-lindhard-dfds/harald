using System;
using System.Linq;
using Harald.WebApi;
using Harald.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harald.Tests.Builders
{
    public class ServiceProviderBuilder
    {
        readonly IServiceCollection _serviceCollection = new ServiceCollection();

        public IServiceCollection Services => _serviceCollection; 
        
        public ServiceProviderBuilder WithServicesFromStartup()
        {
            var configuration = new ConfigurationBuilder().Build();

            var startup = new Startup(configuration);
            startup.ConfigureServices(_serviceCollection);

            
            return this;
        }

        public ServiceProviderBuilder RemoveType(Type type)
        {
            var dbContextOptionsToRemove = _serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == type);
            if (dbContextOptionsToRemove != null) _serviceCollection.Remove(dbContextOptionsToRemove);
            
            return this;
        }
        public ServiceProviderBuilder WithInMemoryDb()
        {
            RemoveDbContextOptionsBuilder();
            RemoveHaraldDbContext();
            AddInMemoryHaraldDbContext();
            
            return this;
        }
        
        private void RemoveDbContextOptionsBuilder()
        {
            var dbContextOptionsToRemove = _serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(DbContextOptions<HaraldDbContext>)
            );
            if (dbContextOptionsToRemove != null) _serviceCollection.Remove(dbContextOptionsToRemove);
        }

        private void RemoveHaraldDbContext()
        {
            var dbContextOptionsToRemove = _serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(HaraldDbContext)
            );
            if (dbContextOptionsToRemove != null) _serviceCollection.Remove(dbContextOptionsToRemove);
        }

        private void AddInMemoryHaraldDbContext()
        {
            _serviceCollection
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaraldDbContext>((serviceProvider, options) =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
        }
   
        public IServiceProvider Build()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}