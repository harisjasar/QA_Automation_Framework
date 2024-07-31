using OpenQA.Selenium;

namespace QA_AutomationFramework.Pages
{
    public class XlsxPage : BasePage
    {
        public delegate T MethodDelegate<T>();

        public XlsxPage(IWebDriver driver) : base(driver) {}

        public IWebElement XlsxIframe => WaitForElementToBeClickable(By.Id("office_frame"));
        public IWebElement ModeSwitcher => WaitForElementToBeClickable(By.Id("ModeSwitcher"));

        public void ClickOnModeSwitcher()
        {
            if (LoaderNotExists) { 
                ExecuteInFrame(() =>
                {
                    ModeSwitcher.Click();
                });
            }
        }

        public bool ModeSwitcherContainsText(string textToCheck){
            return ExecuteInFrame(() => {
                var spans = ModeSwitcher.FindElements(By.TagName("span"));
                foreach (var span in spans)
                {
                    if (span.Text.Contains(textToCheck))
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        private void ExecuteInFrame(Action testMethod)
        {
            try
            {
                driver.SwitchTo().Frame(XlsxIframe);
                testMethod();
            }
            finally
            {
                driver.SwitchTo().DefaultContent();
            }
        }

        private T ExecuteInFrame<T>(MethodDelegate<T> testMethod)
        {
            try
            {
                driver.SwitchTo().Frame(XlsxIframe);
                return testMethod();
            }
            finally
            {
                driver.SwitchTo().DefaultContent();
            }
        }
    }
}
