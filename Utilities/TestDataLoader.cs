using Newtonsoft.Json;
using QA_AutomationFramework.Data;

namespace QA_AutomationFramework.Utilities
{
    public class TestDataLoader
    {
        public TestData LoadTestData(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TestData>(json);
        }
    }
}
