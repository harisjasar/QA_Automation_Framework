using NUnit.Framework;
using QA_AutomationFramework.Pages;
namespace QA_AutomationFramework.Tests
{
    [Category("FileNavigationTests")]
    public class FileNavigationTests : BaseTest
    {
        private LoginPage _loginPage;
        private HomePage _homePage;
        private XlsxPage _xlsxPage;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            _loginPage = new LoginPage(driver, emailService, testData);
            _homePage = new HomePage(driver);
            _xlsxPage = new XlsxPage(driver);
        }

        [Test]
        [Category("@regression")]
        public void NewTab_ShouldOpenXlsxInEditingMode()
        {
            var searchTerm = "Book";
            Step("Login into the app", () => _loginPage.LoginIntoApp());
            Step("Click on the Home menu item", () => _homePage.ClickOnHomeMenuItem());
            Step("Enter search term", () => _homePage.EnterSearchTerm(searchTerm));
            Step("Click on item actions button", () => _homePage.ClickOnItemActionsButton(0));
            Step("Click on open in new tab button", () => _homePage.ClickOnOpenInNewTabButton());
            Step("Switch to the new tab", () => _homePage.SwitchToWindow("Book.xlsx"));
            Step("Verify mode switcher contains text Editing", () => Assert.That(_xlsxPage.ModeSwitcherContainsText("Editing"), Is.True));
        }

        [Test]
        [Category("@regression")]
        public void Preview_ShouldOpenXlsxInViewingMode()
        {
            var searchTerm = "Book";
            Step("Login into the app", () => _loginPage.LoginIntoApp());
            Step("Click on the Home menu item", () => _homePage.ClickOnHomeMenuItem());
            Step("Enter search term", () => _homePage.EnterSearchTerm(searchTerm));
            Step("Click on item actions button", () => _homePage.ClickOnItemActionsButton(0));
            Step("Click on previw", () => _homePage.ClickOnPreviewButton());
            Step("Switch to the new tab", () => _homePage.SwitchToWindow("Book.xlsx"));
            Step("Verify mode switcher contains text Viewing", () => Assert.That(_xlsxPage.ModeSwitcherContainsText("Viewing"), Is.True));
        }
    }
}
