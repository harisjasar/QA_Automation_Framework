using Microsoft.Extensions.DependencyInjection;
using QA_AutomationFramework.Configs;
using QA_AutomationFramework.Contracts;
using QA_AutomationFramework.Utilities;

namespace QA_AutomationFramework.Services
{
    public static class ServiceProviderFactory
    {
        public static ServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            string environment = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            var configurationLoader = new ConfigurationLoader();
            var testSettings = configurationLoader.GetTestSettings(environment);
            serviceCollection.AddSingleton(testSettings);

            var testDataLoader = new TestDataLoader();
            var testData = testDataLoader.LoadTestData(Path.Join(AppContext.BaseDirectory, "Data", "testData.json"));
            serviceCollection.AddSingleton(testData);

            serviceCollection.AddSingleton<IEmailService, GmailService>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
