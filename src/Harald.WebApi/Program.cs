using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Linq;

namespace Harald.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var sourcesToRemove = config.Sources
                        .Where(s => s.GetType() == typeof(JsonConfigurationSource))
                        .ToArray();

                    foreach (var source in sourcesToRemove)
                    {
                        config.Sources.Remove(source);
                    }

                    config
                        .AddJsonFile(
                            path: "appsettings.json",
                            optional: true,
                            reloadOnChange: false
                        )
                        .AddJsonFile(
                            path: "appsettings." + builderContext.HostingEnvironment.EnvironmentName + ".json",
                            optional: true,
                            reloadOnChange: false
                        );
                })
                .UseStartup<Startup>()
                .UseSerilog();
        }
    }
}