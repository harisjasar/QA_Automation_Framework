using NUnit.Framework;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Extensions.DependencyInjection;
using QA_AutomationFramework.Services;
using QA_AutomationFramework.Contracts;
using QA_AutomationFramework.Utilities;
using OpenQA.Selenium.Chrome;
using QA_AutomationFramework.Configs;
using QA_AutomationFramework.Data;

namespace QA_AutomationFramework.Tests
{
    public abstract class BaseTest
    {
        protected ServiceProvider serviceProvider;
        protected IWebDriver driver;
        protected IEmailService emailService;
        protected ExtentReports extent;
        private ExtentSparkReporter spark;
        protected ExtentTest test;
        private TestSettings testSettings;
        protected string reportFolder;
        protected string reportPath;
        protected string recordingsFolder;
        protected TestData testData;
        

        [OneTimeSetUp]
        protected virtual void OneTimeSetUp()
        {
            serviceProvider = ServiceProviderFactory.CreateServiceProvider();

            testSettings = serviceProvider.GetService<TestSettings>();
            testData = serviceProvider.GetService<TestData>();

            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            reportFolder = Path.Combine(AppContext.BaseDirectory, "Reports", $"Run_{timeStamp}");
            Directory.CreateDirectory(reportFolder);
            reportPath = Path.Combine(reportFolder, "report.html");
            extent = new ExtentReports();
            spark = new ExtentSparkReporter(reportPath);
            extent.AttachReporter(spark);

            recordingsFolder = Path.Combine(reportFolder, "Recordings");
            Directory.CreateDirectory(recordingsFolder);
        }

        [SetUp]
        protected virtual void SetUp()
        {
            driver = new ChromeDriver(new ChromeOptions());
            driver.Manage().Window.Maximize();
            emailService = serviceProvider.GetService<IEmailService>();
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            if (testSettings.IncludeVideoRecording)
            {
                VideoHelper.StartRecording(TestContext.CurrentContext.Test.Name, recordingsFolder);
            }
        }

        [TearDown]
        protected virtual void TearDown()
        {
            if (testSettings.IncludeVideoRecording)
            {
                VideoHelper.StopRecording();
            }

            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                LogTestFailure(errorMessage, stackTrace);
            }
            else if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                LogTestSuccess();
            }

            if (testSettings.IncludeVideoRecording)
            {
                LogTestVideo();
            }

            driver?.Quit();
            driver = null;
        }

        [OneTimeTearDown]
        protected virtual void OneTimeTearDown()
        {
            serviceProvider?.Dispose();
            extent.Flush();
        }

        protected T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

        protected void Step(string stepDescription, Action stepAction)
        {
            try
            {
                string screenshotBase64Before = testSettings.IncludeStepScreenshots ? ScreenshotHelper.CaptureScreenshotBase64(driver, test) : null;
                stepAction.Invoke();
                string screenshotBase64After = testSettings.IncludeStepScreenshots ? ScreenshotHelper.CaptureScreenshotBase64(driver, test) : null;

                LogStepResult(stepDescription, screenshotBase64Before, screenshotBase64After, true);
            }
            catch (Exception ex)
            {
                string screenshotBase64 = testSettings.IncludeStepScreenshots ? ScreenshotHelper.CaptureScreenshotBase64(driver, test) : null;
                LogStepResult(stepDescription, screenshotBase64, null, false, ex.Message);
                throw;
            }
        }

        private void LogStepResult(string stepDescription, string screenshotBase64Before, string screenshotBase64After, bool isSuccess, string errorMessage = null)
        {
            string screenshotsHtml = $@"
                <div style='display: flex; flex-direction: column;'>
                    <div style='margin-bottom: 10px;'>
                        <p>Step: {stepDescription}</p>
                        {(isSuccess ? "" : $"<p>Error message: {errorMessage}</p>")}
                    </div>" +
                    (screenshotBase64After == null || screenshotBase64Before == null ? "" : $@"
                    <div style='display: flex; justify-content: flex-start; align-items: center;'>
                        <div style='padding-right: 20px;'>
                            <p>Before:</p>
                            {GetScreenshotHtml(screenshotBase64Before)}
                        </div>
                        <div style='padding-right: 20px;'>
                            <p>After:</p>
                            {GetScreenshotHtml(screenshotBase64After)}
                        </div>
                    </div>") +
                "</div>";

            test.Log(isSuccess ? Status.Pass : Status.Fail, screenshotsHtml);
        }

        private string GetScreenshotHtml(string screenshotBase64)
        {
            if (string.IsNullOrEmpty(screenshotBase64)) return "";
            return $"<a href='data:image/png;base64,{screenshotBase64}' data-featherlight='image'><img src='data:image/png;base64,{screenshotBase64}' style='width: 200px;' /></a>";
        }

        private void LogTestFailure(string errorMessage, string stackTrace)
        {
            string screenshotBase64 = ScreenshotHelper.CaptureScreenshotBase64(driver, test);
            string screenshotHtml = $@"
                <div style='display: flex; flex-direction: column;'>
                    <div style='margin-bottom: 10px;'>
                        <p>Test status: {NUnit.Framework.Interfaces.TestStatus.Failed.ToString()}</p>
                        <button style='padding: 5px 10px; border: none; border-radius: 5px; background-color: #007bff; color: white; cursor: pointer; margin-right: 10px; width: 120px;' onclick='toggleVisibility(this)'>Error Message</button>
                        <div style='display: none;'>
                            <p>Error message: {errorMessage}</p>
                        </div>
                        <br/><button style='padding: 5px 10px; border: none; border-radius: 5px; background-color: #007bff; color: white; cursor: pointer; margin-right: 10px; width: 120px;margin-top: 10px;' onclick='toggleVisibility(this)'>Stack Trace</button>
                        <div style='display: none;'>
                            <p>Stack trace: {stackTrace}</p>
                        </div>
                    </div>
                    <div style='display: flex; justify-content: flex-start; align-items: center;'>
                        <div style='padding-right: 20px;'>
                            <a href='data:image/png;base64,{screenshotBase64}' data-featherlight='image'><img src='data:image/png;base64,{screenshotBase64}' style='width: 200px;' /></a>
                        </div>
                    </div>
                </div>
                <script>
                    function toggleVisibility(button) {{var details = button.nextElementSibling;
                        if (details.style.display === 'none') {{
                            details.style.display = 'block';
                        }} else {{
                            details.style.display = 'none';
                        }}
                    }}
                </script>";

            test.Log(Status.Fail, screenshotHtml);
        }

        private void LogTestSuccess()
        {
            string screenshotBase64 = ScreenshotHelper.CaptureScreenshotBase64(driver, test);
            if (!string.IsNullOrEmpty(screenshotBase64))
            {
                string screenshotHtml = $@"
                <div style='display: flex; flex-direction: column;'>
                    <div style='margin-bottom: 10px;'>
                        <p>Test status: Passed</p>
                    </div>
                    <div style='display: flex; justify-content: flex-start; align-items: center;'>
                        <div style='padding-right: 20px;'>
                            <a href='data:image/png;base64,{screenshotBase64}' data-featherlight='image'><img src='data:image/png;base64,{screenshotBase64}' style='width: 200px;' /></a>
                        </div>
                    </div>
                </div>";
                test.Log(Status.Pass, screenshotHtml);
            }
            else
            {
                test.Pass("Test passed but screenshot could not be captured.");
            }
        }

        private void LogTestVideo()
        {
            string videoFilePath = VideoHelper.FindVideoFile(TestContext.CurrentContext.Test.Name, recordingsFolder);
            if (videoFilePath != null)
            {
                string videoLink = Path.Combine("Recordings", Path.GetFileName(videoFilePath));
                test.Log(Status.Info, $"Test recording:<br/><video width='320' height='240' controls><source src='{videoLink}' type='video/mp4'>Your browser does not support the video tag.</video>");
            }
        }
    }
}
