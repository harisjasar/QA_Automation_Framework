using OpenQA.Selenium;

namespace QA_AutomationFramework.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        private IWebElement HomeMenuItem => WaitForElementToBeClickable(By.Id("menu-/home"));
        private IWebElement SearchInputField => WaitForElementToBeClickable(By.CssSelector("input[type=search]"));

        private IList<IWebElement> TableItems => driver.FindElements(By.CssSelector("[data-testid='table-row']"));

        public void EnterSearchTerm(string textToSearch) => SearchInputField.SendKeys(textToSearch);

        public IList<IWebElement> GetTableItems()
        {
            if (LoaderNotExists)
            {
                return TableItems;
            }

            throw new Exception("Unable to get Table items");
        }
        public void ClickOnHomeMenuItem()
        {
            if (LoaderNotExists)
            {
                HomeMenuItem.Click();
            }
        }

        public string GetItemName(int index)
        {
            if (LoaderNotExists && TableItems.Count() > index)
            {
                var tableRow = TableItems[index];
                string selector = $"td#fp-home-recentfiles-recenttable-body-{index.ToString()}-{index.ToString()}_name button span:not([aria-label])";
                var spanElement = tableRow.FindElements(By.CssSelector(selector));
                if (spanElement != null && spanElement.Count == 1)
                {
                    return spanElement[0].Text;
                }
            }

            return string.Empty;
        }
    }
}
