using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace QA_AutomationFramework.Pages
{
    public class BasePage
    {
        protected IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected bool LoaderNotExists => WaitForElementToNotExistInDom(By.Id("loader"));

        public void NavigateToPageByUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public IWebElement WaitForElementToBeClickable(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public IWebElement WaitForElementToBePresent(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public bool WaitForElementToNotExistInDom(By locator)
        {
            
            try {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    return wait.Until(ExpectedConditions.StalenessOf(element));
                }

                return true;

            } catch(StaleElementReferenceException ex) {
                return true;
            } catch(NoSuchElementException ex) {
                return true;
            }
        }

        public static Func<IWebDriver, IWebElement> ElementIsVisible(IWebElement parentElement, By childLocator)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(parentElement.FindElement(childLocator));
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement element)
        {
            return element.Displayed ? element : null;
        }

        public void SwitchToWindow(string windowTitle)
        {
            const int MaxRetryAttempts = 20;
            const int RetryDelayMilliseconds = 3000;
            int attempt = 0;

            while (true)
            {
                try
                {
                    if (TrySwitchToWindow(windowTitle))
                    {
                        return;
                    }

                    attempt++;
                    if (attempt >= MaxRetryAttempts)
                    {
                        throw new Exception($"Window whose title contains '{windowTitle}' not found after {MaxRetryAttempts} attempts!");
                    }

                    Thread.Sleep(RetryDelayMilliseconds);
                }
                catch (Exception)
                {
                    attempt++;
                    if (attempt >= MaxRetryAttempts)
                    {
                        throw;
                    }

                    Thread.Sleep(RetryDelayMilliseconds);
                }
            }
        }

        private bool TrySwitchToWindow(string windowTitle)
        {
            if (driver.Title.Contains(windowTitle))
            {
                return true;
            }

            foreach (var windowHandle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(windowHandle);
                if (driver.Title.Contains(windowTitle))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
