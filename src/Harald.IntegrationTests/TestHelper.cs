using Microsoft.Extensions.Configuration;

namespace Harald.IntegrationTests
{
    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("3ef49f6c-9399-4f25-a41e-6a3d4cd216cb")
                .AddEnvironmentVariables()
                .Build();
        }

        public static TestConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new TestConfiguration();
            var configRoot = GetIConfigurationRoot(outputPath);

            configRoot.Bind(configuration);

            return configuration;
        }
    }
}
