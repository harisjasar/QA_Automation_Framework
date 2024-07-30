using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace QA_AutomationFramework.Utilities
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshotBase64(IWebDriver driver, ExtentTest test)
        {
            try
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                return screenshot.AsBase64EncodedString;
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, "Failed to capture screenshot: " + e.Message);
                return null;
            }
        }
    }

}
