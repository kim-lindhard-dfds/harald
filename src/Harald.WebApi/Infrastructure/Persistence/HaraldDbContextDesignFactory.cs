using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Harald.WebApi.Infrastructure.Persistence
{
	public class HaraldDbContextDesignFactory : IDesignTimeDbContextFactory<HaraldDbContext>
	{
		public HaraldDbContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<HaraldDbContext>()
				.UseNpgsql(config.GetConnectionString(nameof(HaraldDbContext)));

			return new HaraldDbContext(optionsBuilder.Options);
		}
	}
}
