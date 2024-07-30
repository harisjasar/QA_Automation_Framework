using Microsoft.Extensions.Configuration;

namespace QA_AutomationFramework.Configs
{
    public class ConfigurationLoader
    {
        public IConfigurationRoot LoadConfiguration(string environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Configs"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public TestSettings GetTestSettings(string environment)
        {
            var configuration = LoadConfiguration(environment);
            var testSettings = new TestSettings();
            configuration.GetSection("TestSettings").Bind(testSettings);
            return testSettings;
        }
    }

}
